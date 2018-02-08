using System.Collections.Generic;
using UnityEngine;

public class SVRA_CollisionDetection : MonoBehaviour {
    
    /* List of interactive objects within collision radius */
    private List<SVRA_InteractiveObject> collidingObjects = new List<SVRA_InteractiveObject>();

    void Start()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        SVRA_InteractiveObject component = ValidActiveComponent(other.gameObject);

        if (component == null)
        {
            return;
        }

        collidingObjects.Add(component);     
    }

    void OnTriggerExit(Collider other)
    {
        SVRA_InteractiveObject component = ValidActiveComponent(other.gameObject);

        if (component == null)
        {
            return;
        }
        
        collidingObjects.Remove(component);               
    }

    /* Determine nearest object to interact with */
    public GameObject NearestObject()
    {
        float closestDistance = Mathf.Infinity;
        GameObject touchedObject = null;

        foreach (SVRA_InteractiveObject component in collidingObjects)
        {            
            /* Check for new closest distance */
            if (Vector3.Distance(transform.position, component.transform.position) < closestDistance)
            {
                touchedObject = component.gameObject;
                closestDistance = Vector3.Distance(transform.position, component.transform.position);
            }
        }
        return touchedObject;
    }


    /* TODO: Look this code over and possibly make it recursive */

    /* Verify that a GameObject contains the SVRA_InteractiveObject script */
    SVRA_InteractiveObject ValidActiveComponent(GameObject gameObject)
    {
        /* On Destroy() a GameObject may set to null occasionally so handle that case*/ 
        if (gameObject == null)
        {
            return null;
        }

        SVRA_InteractiveObject component = ValidInteractiveComponent(gameObject.transform);

        if (component == null)
        {
            component = ValidInteractiveComponent(gameObject.transform.parent);
        }

        if (component != null)
        {
            return component;
        }

        return null;
    }

    /* Determine if a GameObject contains the SVRA_InteractiveObject script */
    SVRA_InteractiveObject ValidInteractiveComponent(Transform transform)
    {
        if (transform == null)
        {
            return null;
        }

        SVRA_InteractiveObject component = transform.GetComponent<SVRA_InteractiveObject>();

        if (component != null && component.enabled)
        {
            return component;
        }

        return null;
    }
}
