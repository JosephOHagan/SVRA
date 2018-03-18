using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_InteractToggle : MonoBehaviour {

    [Tooltip("Does the item start interactive (true) or not")]
    public bool startInteractive = true;

    private void Awake()
    {
        GetComponent<SVRA_InteractiveObject>().enabled = startInteractive;
    }

    public void ToggleInteract()
    {
        startInteractive = !startInteractive;
        GetComponent<SVRA_InteractiveObject>().enabled = startInteractive;
    }
    
}