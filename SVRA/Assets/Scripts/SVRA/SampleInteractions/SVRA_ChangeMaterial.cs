using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_ChangeMaterial : MonoBehaviour
{   
    [Tooltip("Make the change of material permanent")]
    public bool permanentChange = false;

    [Tooltip("The list of materials to change between")]
    public Material[] materialList = new Material[1];

    private bool singleMaterialToggle = true;
    private Material originalMaterial;
    private Queue materialQueue = new Queue();

    private void Awake()
    {
        originalMaterial = GetComponent<Renderer>().material;

        for (int i = 0; i < materialList.Length; i++)
        {
            materialQueue.Enqueue(materialList[i]);
        }
    }

    /* Change the material of the attached to object */
    public void ChangeMaterial()
    {
        if (materialList.Length == 1)
        {
            SingleMaterialChange();
        }
        else
        {
            MaterialQueueChange();
        }
    }

    /* Toggle between the original and specified material */
    void SingleMaterialChange()
    {       
        if (permanentChange)
        {
            GetComponent<Renderer>().material = materialList[0];
        }
        else
        {
            GetComponent<Renderer>().material = (singleMaterialToggle ? materialList[0] : originalMaterial);
            singleMaterialToggle = !singleMaterialToggle;
        }
    }

    /* Cycle through a queue of colors back to the original material */
    void MaterialQueueChange()
    {
        if (permanentChange)
        {
            if (materialQueue.Count > 0)
            {
                GetComponent<Renderer>().material = (Material) materialQueue.Dequeue();
            }

            return;
        }
        else
        {
            materialQueue.Enqueue(GetComponent<Renderer>().material);
            GetComponent<Renderer>().material = (Material) materialQueue.Dequeue();
        }
    }

}
