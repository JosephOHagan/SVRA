using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SVRA_AttachBase : MonoBehaviour {
    protected bool tracked;
    protected bool climbable;
    protected bool kinematic;
    protected GameObject grabbedObject;
    protected Rigidbody grabbedObjectRigidBody;
    protected SVRA_InteractiveObjectClass grabbedObjectScript;
    protected Transform trackPoint;
    protected Transform grabbedSnapHandle;
    protected Transform initialAttachPoint;
    protected Rigidbody controllerAttachPoint;

}
