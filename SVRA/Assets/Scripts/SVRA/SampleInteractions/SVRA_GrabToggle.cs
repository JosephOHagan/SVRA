using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_GrabToggle : MonoBehaviour {
    [Tooltip("Does the item start grabbable (true) or not")]
    public bool startGrabbable = false;

    public void ToggleGrab()
    {
        GetComponent<SVRA_GrabbableObject>().enabled = startGrabbable;
        startGrabbable = !startGrabbable;
    }
}
