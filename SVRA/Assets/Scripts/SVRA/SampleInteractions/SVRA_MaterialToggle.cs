using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_MaterialToggle : MonoBehaviour
{   
    private bool toggle = false;
    private Material originalMaterial;

    [Tooltip("The toggle material")]
    public Material toggleMaterial;

    // TODO : Add the ability to cycle through materials (material queue)
    // public Material[] test;

    void Start()
    {
        originalMaterial = GetComponent<Renderer>().material;
        ToggleMaterial();
    }
   
    public void ToggleMaterial()
    {
        GetComponent<Renderer>().material = (toggle ? toggleMaterial : originalMaterial);
        toggle = !toggle;
    }

}
