using UnityEngine;

[AddComponentMenu("")]  // Internally used script so hide from user in the Inspector window
public static class SVRA_JointFactory {

    // Alter these for a stronger or weaker grip
    public static float LINEAR_DRIVE_MULTIPLIER = 1f;
    public static float ANGULAR_DRIVE_MULTIPLIER = 1f;

    public static ConfigurableJoint JointToConnect(GameObject jointObject, Rigidbody desiredObject, Quaternion controllerRotation)
    {
        SVRA_GrabbableObject grabbable = desiredObject.gameObject.GetComponent<SVRA_GrabbableObject>();
        ConfigurableJoint joint = jointObject.AddComponent<ConfigurableJoint>();
        SetLinearDrive(joint, desiredObject.mass);

        if (grabbable.snapPoint.enable)
        {
            SetAnchor(joint, desiredObject, grabbable.RotatedAnchor());
        }

        if (grabbable.ApplyGripRotation())
        {
            SetAngularDrive(joint, desiredObject.mass);
        }
        
        if (grabbable.SnapToOrientation())
        {
            SetTargetRotation(joint, desiredObject, grabbable.rotation.localOrientation, controllerRotation);
        }       

        joint.connectedBody = desiredObject;

        return joint;
    }
    
    private static void SetAnchor(ConfigurableJoint joint, Rigidbody desiredObject, Vector3 anchor)
    {
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = desiredObject.transform.InverseTransformVector(anchor);
    }

    private static JointDrive LinearJointDrive(float strength, float damper, float maxForce)
    {
        JointDrive jointDrive = new JointDrive();
        jointDrive.positionSpring = strength;
        jointDrive.positionDamper = damper;
        jointDrive.maximumForce = maxForce;

        return jointDrive;
    }

    private static JointDrive AngularJointDrive(JointDrive baseJointDrive, float strength, float damper)
    {
        JointDrive jointDrive = baseJointDrive;
        jointDrive.positionSpring = strength;
        jointDrive.positionDamper = damper;

        return jointDrive;
    }

    private static void SetLinearDrive(ConfigurableJoint joint, float mass)
    {
        float multiplier = LINEAR_DRIVE_MULTIPLIER;
        float gripStrength = 15000f * mass * multiplier;
        float gripDamper = 50f * mass * multiplier;
        float maxForce = 350f * mass * multiplier;

        joint.xDrive = LinearJointDrive(gripStrength, gripDamper, maxForce);
        joint.yDrive = LinearJointDrive(gripStrength, gripDamper, maxForce);
        joint.zDrive = LinearJointDrive(gripStrength, gripDamper, maxForce);
    }

    private static void SetAngularDrive(ConfigurableJoint joint, float mass)
    {
        float multiplier = ANGULAR_DRIVE_MULTIPLIER;
        float gripStrength = 300f * mass * multiplier;
        float gripDamper = 10f * mass * multiplier;

        joint.rotationDriveMode = RotationDriveMode.XYAndZ;
        joint.angularYZDrive = AngularJointDrive(joint.angularYZDrive, gripStrength, gripDamper);
        joint.angularXDrive = AngularJointDrive(joint.angularXDrive, gripStrength, gripDamper);
    }
     
    private static void SetTargetRotation(ConfigurableJoint joint, Rigidbody desiredObject, Vector3 desiredOrientation, Quaternion controllerRotation)
    {
        // Undo current rotation, apply the desired orientation, and translate that to controller space
        joint.targetRotation = controllerRotation;
        joint.targetRotation *= Quaternion.Euler(desiredOrientation);
        joint.targetRotation *= Quaternion.Inverse(desiredObject.transform.rotation);
    }
}
