using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_GrabToggle : MonoBehaviour {
    
    [Tooltip("Does the item start grabbable (true) or not")]
    public bool startGrabbable = true;

    private void Awake()
    {
        GetComponent<SVRA_GrabbableObject>().enabled = startGrabbable;
    }

    public void ToggleGrab()
    {        
        startGrabbable = !startGrabbable;
        GetComponent<SVRA_GrabbableObject>().enabled = startGrabbable;
    }
}
