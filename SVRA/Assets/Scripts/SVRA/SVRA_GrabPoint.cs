using UnityEngine;

[DisallowMultipleComponent]
public class SVRA_GrabPoint : MonoBehaviour
{
    [Tooltip("The distance from which you can touch objects.")]
    public float touchRadius = 0.1f;

    [Tooltip("The distance from which objects will automatically drop.")]
    public float holdRadius = 0.3f;

    [Tooltip("Display sphere collider for interactions (Debugging assistance)")]
    public bool visibleCollider = false;

    [Tooltip("Enable toggle grabbing (press once to pickup and once to release)")]
    public bool toggleGrabMode = false;

    [HideInInspector]
    public SVRA_ControllerManager controller;

    [HideInInspector]
    public SVRA_GrabConnection grabConnection;

    public const string COLLIDER_SPHERE_NAME = "SVRA_Touch_Sphere";
    private SVRA_CollisionDetection collisionDetection;

    private bool firmlyGrabbed = false;
    private bool externalGrabTriggered = false;

    private GameObject lastCollidingObject;
    private GameObject lastInteractedObject;

    void Start()
    {
        grabConnection = gameObject.AddComponent<SVRA_GrabConnection>();

        controller = GetComponent<SVRA_ControllerManager>();

        /* Setup sphere collider for controller */
        GameObject collisionSphere = InstantiateTouchSphere();
        collisionDetection = collisionSphere.AddComponent<SVRA_CollisionDetection>();
        collisionDetection.transform.localScale = Vector3.one * touchRadius;                           
    }

    void Update()
    {
        GameObject collidingObject = TouchedObject();

        BroadcastTouch(collidingObject);
        BroadcastGrab(collidingObject);
        BroadcastInteraction(collidingObject);
        GrabConnectionStatus();

        lastCollidingObject = collidingObject;
    }

    void BroadcastGrab(GameObject givenObject)
    {
        if (!GrabTriggered() && !externalGrabTriggered)
        {
            return;
        }

        externalGrabTriggered = false;

        if (grabConnection.HoldingSomething())
        {
            if (givenObject != null)
            {
                Message("SVRAHighlightStart", givenObject);
            }

            DestroyConnection();
        }
        else if (givenObject != null && givenObject.GetComponent<SVRA_GrabbableObject>() != null)
        {
            Message("SVRAGrabStart", givenObject);
            Message("SVRAHighlightStop", givenObject);
        }
    }

    bool GrabTriggered()
    {
        if (controller == null)
        {
            return false;
        }

        if (toggleGrabMode)
        {
            return controller.Pressed(SVRA_ControllerManager.ActionType.Grab);
        }

        return grabConnection.HoldingSomething() ? controller.Released(SVRA_ControllerManager.ActionType.Grab) : controller.Pressed(SVRA_ControllerManager.ActionType.Grab);
    }    

    void GrabConnectionStatus()
    {
        if (grabConnection.HoldingSomething())
        {
            float grabDistance = CalculateGrabDistance();
            bool withinRadius = grabDistance <= holdRadius;

            firmlyGrabbed = firmlyGrabbed || withinRadius;

            if (firmlyGrabbed && !withinRadius)
            {
                DestroyConnection();
            }
        }
    }

    float CalculateGrabDistance()
    {
        SVRA_GrabbableObject grabbable = grabConnection.ConnectedGameObject().GetComponent<SVRA_GrabbableObject>();
        Vector3 grabbedAnchorPosition = grabbable.WorldAnchorPosition();

        return Vector3.Distance(transform.position, grabbedAnchorPosition);
    }

    void BroadcastInteraction(GameObject givenObject)
    {
        if (grabConnection.HoldingSomething())
        {
            givenObject = grabConnection.ConnectedGameObject();
        }

        /*

        if (givenObject == null || givenObject.GetComponent<SVRA_Interactable>() == null)
        {
            return;
        }

        */

        if (controller.Pressed(SVRA_ControllerManager.ActionType.Interact))
        {
            lastInteractedObject = givenObject;
            Message("SVRAInteractionStart", givenObject);
        }

        if (controller.Released(SVRA_ControllerManager.ActionType.Interact))
        {
            Message("SVRAInteractionStop", lastInteractedObject);
            lastInteractedObject = null;
        }
    }

    void BroadcastTouch(GameObject givenObject)
    {
        // if (GameObjectsEqual(lastCollidingObject, givenObject))
        if (Object.ReferenceEquals(lastCollidingObject, givenObject))
        {
            return;
        }

        if (lastCollidingObject != null)
        {
            Message("SVRATouchStop", lastCollidingObject);
            Message("SVRAHighlightStop", lastCollidingObject);
        }

        if (grabConnection.HoldingSomething())
        {
            return;
        }

        if (givenObject != null)
        {
            Message("SVRATouchStart", givenObject);
            Message("SVRAHighlightStart", givenObject);
        }
    }

    GameObject InstantiateTouchSphere()
    {
        GameObject gripSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Renderer sphereRenderer = gripSphere.GetComponent<Renderer>();
        sphereRenderer.enabled = visibleCollider;

        if (visibleCollider)
        {
            sphereRenderer.material = new Material(Shader.Find("SVRA/TouchSphereDebug"));
            sphereRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            sphereRenderer.receiveShadows = false;
        }

        gripSphere.transform.localScale = Vector3.one;
        gripSphere.transform.position = transform.position;
        gripSphere.transform.position = transform.position + new Vector3(0, -0.04f, 0);
        gripSphere.transform.SetParent(transform);
        gripSphere.AddComponent<Rigidbody>().isKinematic = true;
        gripSphere.layer = gameObject.layer;
        gripSphere.name = COLLIDER_SPHERE_NAME;

        return gripSphere;
    }       

    public bool HoldingSomething()
    {
        if (grabConnection == null)
        {
            return false;
        }

        return grabConnection.HoldingSomething();
    }

    public GameObject HeldObject()
    {
        if (grabConnection == null)
        {
            return null;
        }

        if (!grabConnection.HoldingSomething())
        {
            return null;
        }

        return grabConnection.ConnectedGameObject();
    }

    public GameObject TouchedObject()
    {
        if (collisionDetection == null)
        {
            return null;
        }

        return collisionDetection.NearestObject();
    }

    void DestroyConnection()
    {
        firmlyGrabbed = false;
        Message("SVRAGrabStop", HeldObject());
    }

    void Message(string name, GameObject objectToMessage)
    {
        controller.gameObject.BroadcastMessage(name, this, SendMessageOptions.DontRequireReceiver);

        if (objectToMessage == null)
        {
            return;
        }

        objectToMessage.SendMessage(name, this, SendMessageOptions.DontRequireReceiver);
    }

    void OnDisable()
    {
        if (TouchedObject() == null)
        {
            return;
        }

        SVRA_HighlightObject highlighter = TouchedObject().GetComponent<SVRA_HighlightObject>();

        if (highlighter == null)
        {
            return;
        }

        highlighter.RemoveHighlight();
    }

    void OnEnable()
    {
        if (TouchedObject() == null)
        {
            return;
        }

        SVRA_HighlightObject highlighter = TouchedObject().GetComponent<SVRA_HighlightObject>();

        if (highlighter == null)
        {
            return;
        }

        highlighter.AddHighlight();
    }

    /*
     
    public bool TouchingSomething()
    {
        return TouchedObject() != null;
    }

    */

    /*
    
    bool GameObjectsEqual(GameObject first, GameObject second)
    {
        if (first == null && second == null) { return true; }
        if (first == null || second == null) { return false; }
        return first.GetInstanceID() == second.GetInstanceID();
    }

    */
}
