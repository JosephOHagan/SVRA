using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_ColorToggle : MonoBehaviour
{
    private bool toggle = false;
    private Color originalColor;

    [Tooltip("The toggle color")]
    public Color toggleColor;

    // TODO : Add the ability to cycle through colors (color queue)
    // public Color[] test;

    private void Awake()
    {
        originalColor = GetComponent<Renderer>().material.color;
    }

    public void ToggleColor()
    {
        GetComponent<Renderer>().material.color = (toggle ? toggleColor : originalColor);
        toggle = !toggle;
    }

}