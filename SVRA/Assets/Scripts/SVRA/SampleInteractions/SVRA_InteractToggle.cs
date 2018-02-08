using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_InteractToggle : MonoBehaviour {

    [Tooltip("Does the item start interactive (true) or not")]
    public bool interactToggle = false;

    void Start()
    {
        ToggleInteract();
    }

    public void ToggleInteract()
    {
        GetComponent<SVRA_InteractiveObject>().enabled = interactToggle;
        interactToggle = !interactToggle;
    }
}