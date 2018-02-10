using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_CopyObject : MonoBehaviour {
    public GameObject copyObject;
    public Transform spawnPoint;

    public void CopyObject()
    {
        GameObject newObject = GameObject.Instantiate(copyObject);
        newObject.transform.position = spawnPoint.position;
        newObject.transform.localScale = copyObject.transform.lossyScale;        
    }
}
