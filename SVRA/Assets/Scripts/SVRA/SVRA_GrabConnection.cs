using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_GrabConnection : MonoBehaviour {

    public GameObject jointObject;
    public ConfigurableJoint joint;

    void SVRAGrabStart(SVRA_GrabPoint grabPoint)
    {
        if (!this.enabled)
        {
            return;
        }

        jointObject = InstantiateJointParent();
        GrabWith(grabPoint);
    }

    void SVRAGrabStop(SVRA_GrabPoint gripPoint)
    {
        if (!this.enabled)
        {
            return;
        }

        Destroy(jointObject);
    }

    void GrabWith(SVRA_GrabPoint grabPoint)
    {
        Rigidbody desiredBody = grabPoint.TouchedObject().GetComponent<Rigidbody>();
        desiredBody.gameObject.GetComponent<SVRA_GrabbableObject>().GrabFrom(transform.position);
        joint = SVRA_JointFactory.JointToConnect(jointObject, desiredBody, transform.rotation);
    }

    GameObject InstantiateJointParent()
    {
        GameObject newJointObject = new GameObject("SVRA_Joint");
        newJointObject.transform.parent = transform;
        newJointObject.transform.localPosition = Vector3.zero;
        newJointObject.transform.localScale = Vector3.one;
        newJointObject.transform.rotation = Quaternion.identity;

        Rigidbody jointRigidbody = newJointObject.AddComponent<Rigidbody>();
        jointRigidbody.useGravity = true;
        jointRigidbody.isKinematic = true;

        return newJointObject;
    }

    public GameObject ConnectedGameObject()
    {
        return joint.connectedBody.gameObject;
    }

    public bool HoldingSomething()
    {
        return jointObject != null;
    }

}
