using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SVRA_ParentedObject : SVRA_InteractiveObjectClass
{

    public SVRA_ButtonAssistant pickupButtonAlias = SVRA_ButtonAssistant.Trigger;

    private Dictionary<SVRA_ButtonAssistant, Valve.VR.EVRButtonId> ButtonMapping = new Dictionary<SVRA_ButtonAssistant, Valve.VR.EVRButtonId>(new SVRA_ButtonComparator());

    public Valve.VR.EVRButtonId GetButton(SVRA_ButtonAssistant button)
    {
        if (ButtonMapping.ContainsKey(button) == false)
        {
            return Valve.VR.EVRButtonId.k_EButton_System;
            //Debug.LogError("No SteamVR button configured for: " + button.ToString());
        }
        return ButtonMapping[button];
    }

    void SetupButtonMapping()
    {
        ButtonMapping.Add(SVRA_ButtonAssistant.A, Valve.VR.EVRButtonId.k_EButton_A);
        ButtonMapping.Add(SVRA_ButtonAssistant.ApplicationMenu, Valve.VR.EVRButtonId.k_EButton_ApplicationMenu);
        ButtonMapping.Add(SVRA_ButtonAssistant.Axis0, Valve.VR.EVRButtonId.k_EButton_Axis0);
        ButtonMapping.Add(SVRA_ButtonAssistant.Axis1, Valve.VR.EVRButtonId.k_EButton_Axis1);
        ButtonMapping.Add(SVRA_ButtonAssistant.Axis2, Valve.VR.EVRButtonId.k_EButton_Axis2);
        ButtonMapping.Add(SVRA_ButtonAssistant.Axis3, Valve.VR.EVRButtonId.k_EButton_Axis3);
        ButtonMapping.Add(SVRA_ButtonAssistant.Axis4, Valve.VR.EVRButtonId.k_EButton_Axis4);
        ButtonMapping.Add(SVRA_ButtonAssistant.Back, Valve.VR.EVRButtonId.k_EButton_Dashboard_Back);
        ButtonMapping.Add(SVRA_ButtonAssistant.DPad_Down, Valve.VR.EVRButtonId.k_EButton_DPad_Down);
        ButtonMapping.Add(SVRA_ButtonAssistant.DPad_Left, Valve.VR.EVRButtonId.k_EButton_DPad_Left);
        ButtonMapping.Add(SVRA_ButtonAssistant.DPad_Right, Valve.VR.EVRButtonId.k_EButton_DPad_Right);
        ButtonMapping.Add(SVRA_ButtonAssistant.DPad_Up, Valve.VR.EVRButtonId.k_EButton_DPad_Up);
        ButtonMapping.Add(SVRA_ButtonAssistant.Grip, Valve.VR.EVRButtonId.k_EButton_Grip);
        ButtonMapping.Add(SVRA_ButtonAssistant.System, Valve.VR.EVRButtonId.k_EButton_System);
        ButtonMapping.Add(SVRA_ButtonAssistant.Touchpad, Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
        ButtonMapping.Add(SVRA_ButtonAssistant.Trigger, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
        ButtonMapping.Add(SVRA_ButtonAssistant.B, Valve.VR.EVRButtonId.k_EButton_A);
        ButtonMapping.Add(SVRA_ButtonAssistant.X, Valve.VR.EVRButtonId.k_EButton_A);
        ButtonMapping.Add(SVRA_ButtonAssistant.Y, Valve.VR.EVRButtonId.k_EButton_A);
    }

    public Valve.VR.EVRButtonId pickupButton;

    public Transform InteractionPoint;
    public Rigidbody rigidBody;
    protected bool originalKinematicState;
    protected Transform originalParent;

    public bool replaceController = true;

    void Reset()
    {
        rigidBody = this.GetComponent<Rigidbody>();
    }

    void Awake()
    {

        SetupButtonMapping();
        pickupButton = GetButton(pickupButtonAlias);

        if (rigidBody == null)
        {
            rigidBody = this.GetComponent<Rigidbody>();
        }                    

        //Capture object's original parent and kinematic state
        originalParent = transform.parent;
        originalKinematicState = rigidBody.isKinematic;
    }

    public override void ButtonPressDown(Valve.VR.EVRButtonId button, SVRA_ControllerSetup controller)
    {      
        //If pickup button was pressed
        if (button == pickupButton)
        {
            ParentedPickup(controller);
        }            

        if (replaceController)
        {

            SteamVR_RenderModel mod = controller.GetComponent<SteamVR_RenderModel>();
            

            foreach (SteamVR_RenderModel model in this.GetComponentsInChildren<SteamVR_RenderModel>())
            {
                foreach (Renderer renderer in model.GetComponentsInChildren<Renderer>(true))
                {
                    renderer.enabled = false;
                }

                foreach (Renderer renderer in mod.GetComponentsInChildren<Renderer>(true))
                {
                    renderer.enabled = false;
                }

                foreach (var child in model.GetComponentsInChildren<MeshRenderer>())
                    child.enabled = replaceController;
            }
        }
    }

    public override void ButtonPressUp(Valve.VR.EVRButtonId button, SVRA_ControllerSetup controller)
    {
        //If pickup button was released
        if (button == pickupButton)
            ParentedRelease(controller);
    }

    protected void ParentedPickup(SVRA_ControllerSetup controller)
    {              
        //Make object kinematic
        //(Not effected by physics, but still able to effect other objects with physics)
        rigidBody.isKinematic = true;

        //Parent object to hand
        transform.SetParent(controller.gameObject.transform);

        //If there is an interaction point, snap object to that point
        if (InteractionPoint != null)
        {
            //Set the position of the object to the inverse of the interaction point's local position.
            transform.localPosition = -InteractionPoint.localPosition;

            //Set the local rotation of the object to the inverse of the rotation of the interaction point.
            //When you're setting your interaction point the blue arrow (Z) should be pointing in the direction you want your hand to be pointing
            //and the green arrow (Y) should be pointing "up".
            transform.localRotation = Quaternion.Inverse(InteractionPoint.localRotation);
        }
    }

    protected void ParentedRelease(SVRA_ControllerSetup controller)
    {
        //Make sure the hand is still the parent. 
        //Could have been transferred to anothr hand.
        if (transform.parent == controller.gameObject.transform)
        {
            //Return previous kinematic state
            rigidBody.isKinematic = originalKinematicState;

            //Set object's parent to its original parent
            if (originalParent != controller.gameObject.transform)
            {
                //Ensure original parent recorded wasn't somehow the controller (failsafe)
                transform.SetParent(originalParent);
            }
            else
            {
                transform.SetParent(null);
            }

            //Throw object
            ThrowObject(controller);
        }
    }

    protected void ThrowObject(SVRA_ControllerSetup controller)
    {
        //Set object's velocity and angular velocity to that of controller
        rigidBody.velocity = controller.controller.velocity;
        rigidBody.angularVelocity = controller.controller.angularVelocity;
    }
}
