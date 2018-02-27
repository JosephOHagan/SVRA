using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_ColorToggle : MonoBehaviour
{
    [Tooltip("Make the change of color permanent")]
    public bool permanentChange = false;

    [Tooltip("The list of colors to change between")]
    public Color[] colorList = new Color[1];

    private bool singleColorToggle = true;
    private Color originalColor;
    private Queue colorQueue = new Queue();
        
    private void Awake()
    {
        originalColor = GetComponent<Renderer>().material.color;

        for (int i = 0; i < colorList.Length; i++)
        {
            colorQueue.Enqueue(colorList[i]);
        }
    }

    /* Change the color of the attached to object */
    public void ChangeColor()
    {
        if (colorList.Length == 1)
        {
            SingleColorChange();
        }
        else
        {
            ColorQueueChange();
        }
    }

    /* Toggle between the original and specified color */
    void SingleColorChange()
    {
        if (permanentChange)
        {
            GetComponent<Renderer>().material.color = colorList[0];
        }
        else
        {
            GetComponent<Renderer>().material.color = (singleColorToggle ? colorList[0] : originalColor);
            singleColorToggle = !singleColorToggle;
        }        
    }

    /* Cycle through a queue of colors back to the original color */
    void ColorQueueChange()
    {
        if (permanentChange)
        {
            if (colorQueue.Count > 0)
            {
                GetComponent<Renderer>().material.color = (Color)colorQueue.Dequeue();
            }

            return;            
        }
        else
        {
            colorQueue.Enqueue(GetComponent<Renderer>().material.color);
            GetComponent<Renderer>().material.color = (Color)colorQueue.Dequeue();
        }
    }
}