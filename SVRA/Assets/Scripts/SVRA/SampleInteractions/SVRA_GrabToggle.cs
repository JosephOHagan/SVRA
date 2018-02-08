using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_GrabToggle : MonoBehaviour {
    [Tooltip("Does the item start grabbable (true) or not")]
    public bool grabToggle = false;

    void Start()
    {
        ToggleGrab();
    }

    public void ToggleGrab()
    {
        GetComponent<SVRA_GrabbableObject>().enabled = grabToggle;
        grabToggle = !grabToggle;
    }
}
