using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SVRA_InteractiveObject))]
public class SVRA_InteractSwitch : MonoBehaviour {

    private int switchDirection = -1;

    public enum RotationAxis { X, Y, Z };
    public RotationAxis rotationAxis;
    
    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;

        switch (rotationAxis)
        {
            case RotationAxis.X:
                rotation.x *= switchDirection;
                break;
            case RotationAxis.Y:
                rotation.y *= switchDirection;
                break;
            case RotationAxis.Z:
                rotation.z *= switchDirection;
                break;
        }

        transform.eulerAngles = rotation;
    }
}
