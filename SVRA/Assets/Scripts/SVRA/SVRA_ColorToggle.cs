using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_ColorToggle : MonoBehaviour
{
    private bool toggle = false;
    private Color originalColor;

    [Tooltip("The toggle color")]
    public Color toggleColor;

    void Start()
    {
        originalColor = GetComponent<Renderer>().material.color;
        ToggleColor();
    }

    public void ToggleColor()
    {
        GetComponent<Renderer>().material.color = (toggle ? toggleColor : originalColor);
        toggle = !toggle;
    }

}

