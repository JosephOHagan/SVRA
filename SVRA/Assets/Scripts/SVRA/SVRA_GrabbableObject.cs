using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class SVRA_GrabbableObject : SVRA_InteractiveObject
{
    public enum RotationMode
    {
        Disabled,
        ApplyGrip,
        // ApplyGripAndOrientation
    }

    [System.Serializable]
    public class Position
    {
        [Tooltip("Enable object pickup snapping")]
        public bool enable = false;
        [Tooltip("The local position that will be gripped if enabled.")]
        public Vector3 localPosition = Vector3.zero;
    }

    [System.Serializable]
    public class Rotation
    {
        [Tooltip("The rotations that will be applied to a grabbed object.")]
        public RotationMode mode = RotationMode.ApplyGrip;
        [Tooltip("The local orientation that can be snapped to when grabbed.")]
        public Vector3 localOrientation = Vector3.zero;
    }

    // public Transform interactionPoint;

    public Position snapPoint;
    public Rotation rotation;
    private Vector3 grabCentre; 

    public Vector3 RotatedAnchor()
    {
        return transform.rotation * snapPoint.localPosition;
    }

    public void GrabFrom(Vector3 jointLocation)
    {
        grabCentre = snapPoint.enable ? snapPoint.localPosition : (jointLocation - transform.position);     

        /*
        
        // If there is an interaction point, snap object to that point
        if (interactionPoint != null)
        {
            // Set the position of the object to the inverse of the interaction point's local position.
            transform.localPosition = -interactionPoint.localPosition;

            // Set the local rotation of the object to the inverse of the rotation of the interaction point.
            // When you're setting your interaction point the blue arrow (Z) should be pointing in the direction you want your hand to be pointing
            // and the green arrow (Y) should be pointing "up".
            transform.localRotation = Quaternion.Inverse(interactionPoint.localRotation);
        }

        */
    }

    public Vector3 WorldAnchorPosition()
    {
       return transform.position + (transform.rotation * grabCentre);
    }

    public bool ApplyGripRotation()
    {
        return rotation.mode != RotationMode.Disabled;
    }

    /*
    
    public bool SnapToOrientation()
    {
        return rotation.mode == RotationMode.ApplyGripAndOrientation;
    }
    
    */
}
