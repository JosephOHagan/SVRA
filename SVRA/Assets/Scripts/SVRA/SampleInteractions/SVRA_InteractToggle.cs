using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_InteractToggle : MonoBehaviour {

    [Tooltip("Does the item start interactive (true) or not")]
    public bool startInteractive = false;

    public void ToggleInteract()
    {
        GetComponent<SVRA_InteractiveObject>().enabled = startInteractive;
        startInteractive = !startInteractive;
    }
}