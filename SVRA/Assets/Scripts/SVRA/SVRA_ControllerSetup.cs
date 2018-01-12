using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SVRA_ControllerSetup : MonoBehaviour
{    
    protected Dictionary<Valve.VR.EVRButtonId, List<SVRA_InteractiveObjectClass>> pressedDownObjects;
    protected List<SVRA_InteractiveObjectClass> overlappedObjects;
    protected List<Valve.VR.EVRButtonId> trackedButtons;

    // Setup controller reference    
    protected SteamVR_TrackedObject trackedObject;
    public SteamVR_Controller.Device controller
    {
        get
        {
            return SteamVR_Controller.Input((int)trackedObject.index);
        }
    }

    void Awake()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();

        // Intstantiate the lists
        pressedDownObjects = new Dictionary<Valve.VR.EVRButtonId, List<SVRA_InteractiveObjectClass>>();
        overlappedObjects = new List<SVRA_InteractiveObjectClass>();

        //List the buttons you send inputs to the controller for
        trackedButtons = new List<Valve.VR.EVRButtonId>()
        {
            // Todo : Add more here
            Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger,
            Valve.VR.EVRButtonId.k_EButton_Grip
        };
    }

    // Used to configure the default state of the required components
    void Reset()
    {
        this.GetComponent<SphereCollider>().isTrigger = true;
        this.GetComponent<SphereCollider>().center = new Vector3(0.0f, -0.04f, 0.0f);
        this.GetComponent<SphereCollider>().radius = 0.05f;
    }

    void Update()
    {
        //Check through all desired buttons to see if any have been released
        Valve.VR.EVRButtonId[] pressKeys = pressedDownObjects.Keys.ToArray();

        for (int i = 0; i < pressKeys.Length; i++)
        {
            //If tracked button is released
            if (controller.GetPressUp(pressKeys[i]))
            {
                //Get all tracked objects in that button's "pressed" list
                List<SVRA_InteractiveObjectClass> releaseObjects = pressedDownObjects[pressKeys[i]];

                for (int j = 0; j < releaseObjects.Count; j++)
                {
                    //Send button release through to interactive script
                    releaseObjects[j].ButtonPressUp(pressKeys[i], this);
                }

                //Clear button's associated list
                pressedDownObjects[pressKeys[i]].Clear();
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {       
        if (collider.attachedRigidbody != null)
        {
            // If the rigidbody's object has interactive object scripts 
            // attached then iterate through them
            SVRA_InteractiveObjectClass[] interactiveObjects = collider.attachedRigidbody.GetComponents<SVRA_InteractiveObjectClass>();

            for (int i = 0; i < interactiveObjects.Length; i++)
            {
                SVRA_InteractiveObjectClass interactiveObject = interactiveObjects[i];

                for (int b = 0; b < trackedButtons.Count; b++)
                {
                    // If a tracked button is pressed
                    Valve.VR.EVRButtonId button = trackedButtons[b];
                    if (controller.GetPressDown(button))
                    {
                        // If we haven't already sent the button press message to this object
                        // Safeguard against objects that have multiple colliders for one interactive script
                        if (!pressedDownObjects.ContainsKey(button) || !pressedDownObjects[button].Contains(interactiveObject))
                        {
                            // Send button press through to interactive script
                            interactiveObject.ButtonPressDown(button, this);

                            // Add interactive script to a dictionary flagging it to recieve notice
                            // when that same button is released
                            if (!pressedDownObjects.ContainsKey(button))
                            {
                                pressedDownObjects.Add(button, new List<SVRA_InteractiveObjectClass>());
                            }                                

                            pressedDownObjects[button].Add(interactiveObject);
                        }
                    }
                }
            }
        }
    }

    /*
        void OnTriggerEnter(Collider collider)
        {
            if (collider.attachedRigidbody != null)
            {
                SVRA_InteractiveObjectClass[] interactiveObjects = collider.attachedRigidbody.GetComponents<SVRA_InteractiveObjectClass>();

                for (int i = 0; i < interactiveObjects.Length; i++)
                {
                    SVRA_InteractiveObjectClass interactiveObject = interactiveObjects[i];
                    interactiveObject.TriggerEnter(this);
                }
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.attachedRigidbody != null)
            {
                SVRA_InteractiveObjectClass[] interactiveObjects = collider.attachedRigidbody.GetComponents<SVRA_InteractiveObjectClass>();

                for (int i = 0; i < interactiveObjects.Length; i++)
                {
                    SVRA_InteractiveObjectClass interactiveObject = interactiveObjects[i];
                    interactiveObject.TriggerExit(this);
                }
            }
        }
    */

}
