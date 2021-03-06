﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_HoverSnapZonePosition : MonoBehaviour {

    public Transform snapZoneTransform;
    private bool entered = false;

    void Update()
    {
        GetComponent<MeshRenderer>().enabled = entered;
    }

    void OnTriggerStay(Collider other)
    {
        if (CapsuleIs(other.gameObject))
        {
            SetEnteredTo(!ObjectSeated());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (CapsuleIs(other.gameObject))
        {
            SetEnteredTo(false);
        }
    }

    void SetEnteredTo(bool state)
    {
        entered = state;
        snapZoneTransform.gameObject.GetComponent<SVRA_HoverSnapZone>().inZone = state;
    }

    bool ObjectSeated()
    {
        return snapZoneTransform.gameObject.GetComponent<SVRA_HoverSnapZone>().seated;
    }

    bool CapsuleIs(GameObject gameObject)
    {
        return snapZoneTransform.gameObject.GetInstanceID() == gameObject.GetInstanceID();
    }

}
