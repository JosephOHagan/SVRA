using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SVRA_PhysicsObject : SVRA_InteractiveObjectClass {

    public Valve.VR.EVRButtonId pickupButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    public Rigidbody rigidBody;

    protected Transform pickupTransform;

    protected SVRA_ControllerSetup playerController;

    void Reset()
    {
        rigidBody = this.GetComponent<Rigidbody>();    
    }

    void Awake()
    {       
        // Set the max angular velocity for a more realistic rotation
        rigidBody.maxAngularVelocity = 100;
    }

    void FixedUpdate()
    {
        if (playerController != null)
        {
            PhysicsUpdate();
        }
    }

    public override void ButtonPressDown(Valve.VR.EVRButtonId button, SVRA_ControllerSetup controller)
    {
        if (button == pickupButton)
            PhysicsPickup(controller);
    }

    public override void ButtonPressUp(Valve.VR.EVRButtonId button, SVRA_ControllerSetup controller)
    {
        if (button == pickupButton)
            PhysicsRelease(controller);
    }

    protected void PhysicsPickup(SVRA_ControllerSetup controller)
    {
        pickupTransform = new GameObject(string.Format("[{0}] PickupTransform", gameObject.name)).transform;
        pickupTransform.parent = controller.transform;
        pickupTransform.position = transform.position;
        pickupTransform.rotation = transform.rotation;        

        playerController = controller;
    }

    protected void PhysicsUpdate()
    {
        Quaternion rotationDelta = pickupTransform.rotation * Quaternion.Inverse(rigidBody.transform.rotation);
        Vector3 positionDelta = (pickupTransform.position - rigidBody.transform.position);
        float deltaPoses = Time.fixedDeltaTime;

        float angle;
        Vector3 axis;
        rotationDelta.ToAngleAxis(out angle, out axis);

        if (angle > 180)
        {
            angle -= 360;
        }            

        if (angle != 0)
        {
            Vector3 AngularTarget = angle * axis;
            rigidBody.angularVelocity = Vector3.MoveTowards(rigidBody.angularVelocity, AngularTarget, 10f * (deltaPoses * 1000));
        }

        Vector3 VelocityTarget = positionDelta / deltaPoses;
        rigidBody.velocity = Vector3.MoveTowards(rigidBody.velocity, VelocityTarget, 10f);
    }

    protected void PhysicsRelease(SVRA_ControllerSetup controller)
    {
        // Ensure item is still being held
        if (playerController == controller)
        {
            playerController = null;
            Destroy(pickupTransform.gameObject);
            pickupTransform = null;
            
            rigidBody.velocity = controller.controller.velocity;
            rigidBody.angularVelocity = controller.controller.angularVelocity;
        }
    }

    protected void ThrowObject(SVRA_ControllerSetup controller)
    {
        //Set object's velocity and angular velocity to that of controller
        rigidBody.velocity = controller.controller.velocity;
        rigidBody.angularVelocity = controller.controller.angularVelocity;
    }

}
