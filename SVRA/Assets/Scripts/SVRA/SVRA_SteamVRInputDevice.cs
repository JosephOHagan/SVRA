using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_SteamVRInputDevice : SVRA_InputDevice
{
    SteamVR_Controller.Device controller;
    private int deviceIndex = -1;
    private bool renderModelInitialised = false;

    private Dictionary<SVRA_ButtonAssistant, Valve.VR.EVRButtonId> buttonMapping = new Dictionary<SVRA_ButtonAssistant, Valve.VR.EVRButtonId>(new SVRA_ButtonComparator());

    protected bool isKnuckles = false;
    protected float indexCurl;
    protected float middleCurl;
    protected float ringCurl;
    protected float pinkyCurl;
    protected bool thumbTouch;
    protected bool wasGripped;
    protected bool isGripped;
    protected float lastChecked;
    protected float curlAmountNeeded = 0.7f;
    private object PlayerHandState;

    public override void Initialize(SVRA_PlayerHand hand)
    {
        SetupButtonMapping();
        base.Initialize(hand);
        SteamVR_Events.RenderModelLoaded.Listen(RenderModelLoaded);
    }

    private void OnDestroy()
    {
        SteamVR_Events.RenderModelLoaded.Remove(RenderModelLoaded);
    }

    private Valve.VR.EVRButtonId GetButton(SVRA_ButtonAssistant button)
    {
        if (!buttonMapping.ContainsKey(button))
        {
            // If mapping not found return this default system button
            return Valve.VR.EVRButtonId.k_EButton_System;
        }

        return buttonMapping[button];
    }

    public override bool IsCurrentlyTracked
    {
        get
        {
            return deviceIndex != -1;
        }
    }

    public override float GetAxis1D(SVRA_ButtonAssistant button)
    {
        if (controller != null)
            return controller.GetAxis(GetButton(button)).x;

        return 0;
    }

    public override Vector2 GetAxis2D(SVRA_ButtonAssistant button)
    {
        if (controller != null)
            return controller.GetAxis(GetButton(button));

        return Vector2.zero;
    }

    public override string GetDeviceName()
    {
        if (playerHand.HasCustomModel == true)
        {
            return "Custom_Hand_Setup";
        }
        else
        {
            return this.GetComponentInChildren<SteamVR_RenderModel>(true).renderModelName;
        }
    }

    public override bool GetPress(SVRA_ButtonAssistant button)
    {
        if (controller != null)
        {
            if (isKnuckles)
            {
                if (button == SVRA_ButtonAssistant.Grip)
                {
                    UpdateKnucklesFingerCurl();
                    return isGripped;
                }
            }
            return controller.GetPress(GetButton(button));
        }
        return false;
    }

    public override bool GetPressDown(SVRA_ButtonAssistant button)
    {
        if (controller != null)
        {
            if (isKnuckles)
            {
                if (button == SVRA_ButtonAssistant.Grip)
                {
                    UpdateKnucklesFingerCurl();
                    return ((!wasGripped) && (isGripped));
                }
            }
            return controller.GetPressDown(GetButton(button));
        }
        return false;
    }

    public override bool GetPressUp(SVRA_ButtonAssistant button)
    {
        if (controller != null)
        {
            if (isKnuckles)
            {
                if (button == SVRA_ButtonAssistant.Grip)
                {
                    UpdateKnucklesFingerCurl();
                    return ((wasGripped) && (!isGripped));
                }
            }
            return controller.GetPressUp(GetButton(button));
        }
        return false;
    }

    public override bool GetTouch(SVRA_ButtonAssistant button)
    {
        if (controller != null)
            return controller.GetTouch(GetButton(button));

        return false;
    }

    public override bool GetTouchDown(SVRA_ButtonAssistant button)
    {
        if (controller != null)
            return controller.GetTouchDown(GetButton(button));

        return false;
    }

    public override bool GetTouchUp(SVRA_ButtonAssistant button)
    {
        if (controller != null)
            return controller.GetTouchUp(GetButton(button));

        return false;
    }

    public override bool GetNearTouch(SVRA_ButtonAssistant button)
    {
        return false;
    }

    public override bool GetNearTouchDown(SVRA_ButtonAssistant button)
    {
        return false;
    }

    public override bool GetNearTouchUp(SVRA_ButtonAssistant button)
    {
        return false;
    }

    public override bool ReadyToInitialise()
    {
        return (renderModelInitialised || playerHand.HasCustomModel) && deviceIndex != -1;
    }

    public override Collider[] SetupDefaultColliders()
    {
        throw new System.NotImplementedException();
    }

    public override Collider[] SetupDefaultPhysicalColliders(Transform ModelParent)
    {
        Collider[] colliders = null;
        string controllerModel = GetDeviceName();

        SteamVR_RenderModel renderModel = this.GetComponentInChildren<SteamVR_RenderModel>();

        switch(controllerModel)
        {
            case "vr_controller_05_wireless_b":
                Transform dk1Trackhat = renderModel.transform.Find("trackhat");
                if (dk1Trackhat == null)
                {
                    // Dk1 controller model has trackhat
                }
                else
                {
                    dk1Trackhat.gameObject.SetActive(true);
                }

                SphereCollider dk1TrackhatCollider = dk1Trackhat.gameObject.GetComponent<SphereCollider>();
                if (dk1TrackhatCollider == null)
                {
                    dk1TrackhatCollider = dk1Trackhat.gameObject.AddComponent<SphereCollider>();
                    dk1TrackhatCollider.isTrigger = true;
                }

                colliders = new Collider[] { dk1TrackhatCollider };
                break;

            case "vr_controller_vive_1_5":
                Transform dk2Trackhat = renderModel.transform.Find("trackhat");
                if (dk2Trackhat == null)
                {
                    dk2Trackhat = new GameObject("trackhat").transform;
                    dk2Trackhat.gameObject.layer = this.gameObject.layer;
                    dk2Trackhat.parent = renderModel.transform;
                    dk2Trackhat.localPosition = new Vector3(0, -0.033f, 0.014f);
                    dk2Trackhat.localScale = Vector3.one * 0.1f;
                    dk2Trackhat.localEulerAngles = new Vector3(325, 0, 0);
                    dk2Trackhat.gameObject.SetActive(true);
                }
                else
                {
                    dk2Trackhat.gameObject.SetActive(true);
                }

                Collider dk2TrackhatCollider = dk2Trackhat.gameObject.GetComponent<SphereCollider>();
                if (dk2TrackhatCollider == null)
                {
                    dk2TrackhatCollider = dk2Trackhat.gameObject.AddComponent<SphereCollider>();
                    dk2TrackhatCollider.isTrigger = true;
                }

                colliders = new Collider[] { dk2TrackhatCollider };
                break;
            case "{knuckles}valve_controller_knu_ev1_3_left":
            case "{knuckles}valve_controller_knu_ev1_3_right":
                isKnuckles = true;

                Transform knucklesTrackpad = renderModel.transform.Find("trackpad").GetChild(0);

                SphereCollider knucklesTrackpadCollider = knucklesTrackpad.gameObject.GetComponent<SphereCollider>();
                if (knucklesTrackpadCollider == null)
                {
                    knucklesTrackpadCollider = knucklesTrackpad.gameObject.AddComponent<SphereCollider>();
                    knucklesTrackpadCollider.isTrigger = true;
                    knucklesTrackpadCollider.radius = 0.04f;
                }

                colliders = new Collider[] { knucklesTrackpadCollider };
                break;
        }

        return colliders;
    }

    public override GameObject SetupDefaultRenderModel()
    {
        GameObject renderModel = new GameObject("Render Model for " + playerHand.gameObject.name);
        renderModel.transform.parent = playerHand.transform;
        renderModel.transform.localPosition = Vector3.zero;
        renderModel.transform.localRotation = Quaternion.identity;
        renderModel.transform.localScale = Vector3.one;
        renderModel.AddComponent<SteamVR_RenderModel>();
        renderModel.GetComponent<SteamVR_RenderModel>().shader = Shader.Find("Standard");

        return renderModel;
    }

    public override void TriggerHapticPulse(ushort durationMicroSec = 500, SVRA_ButtonAssistant button = SVRA_ButtonAssistant.Touchpad)
    {
        if (controller != null)
        {
            if (durationMicroSec < 3000)
            {
                controller.TriggerHapticPulse(durationMicroSec, buttonMapping[button]);
            }
        }
    }

    protected virtual void SetupButtonMapping()
    {
        buttonMapping.Add(SVRA_ButtonAssistant.A, Valve.VR.EVRButtonId.k_EButton_A);
        buttonMapping.Add(SVRA_ButtonAssistant.ApplicationMenu, Valve.VR.EVRButtonId.k_EButton_ApplicationMenu);
        buttonMapping.Add(SVRA_ButtonAssistant.Axis0, Valve.VR.EVRButtonId.k_EButton_Axis0);
        buttonMapping.Add(SVRA_ButtonAssistant.Axis1, Valve.VR.EVRButtonId.k_EButton_Axis1);
        buttonMapping.Add(SVRA_ButtonAssistant.Axis2, Valve.VR.EVRButtonId.k_EButton_Axis2);
        buttonMapping.Add(SVRA_ButtonAssistant.Axis3, Valve.VR.EVRButtonId.k_EButton_Axis3);
        buttonMapping.Add(SVRA_ButtonAssistant.Axis4, Valve.VR.EVRButtonId.k_EButton_Axis4);
        buttonMapping.Add(SVRA_ButtonAssistant.Back, Valve.VR.EVRButtonId.k_EButton_Dashboard_Back);
        buttonMapping.Add(SVRA_ButtonAssistant.DPad_Down, Valve.VR.EVRButtonId.k_EButton_DPad_Down);
        buttonMapping.Add(SVRA_ButtonAssistant.DPad_Left, Valve.VR.EVRButtonId.k_EButton_DPad_Left);
        buttonMapping.Add(SVRA_ButtonAssistant.DPad_Right, Valve.VR.EVRButtonId.k_EButton_DPad_Right);
        buttonMapping.Add(SVRA_ButtonAssistant.DPad_Up, Valve.VR.EVRButtonId.k_EButton_DPad_Up);
        buttonMapping.Add(SVRA_ButtonAssistant.Grip, Valve.VR.EVRButtonId.k_EButton_Grip);
        buttonMapping.Add(SVRA_ButtonAssistant.System, Valve.VR.EVRButtonId.k_EButton_System);
        buttonMapping.Add(SVRA_ButtonAssistant.Touchpad, Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
        buttonMapping.Add(SVRA_ButtonAssistant.Trigger, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);

        buttonMapping.Add(SVRA_ButtonAssistant.B, Valve.VR.EVRButtonId.k_EButton_A);
        buttonMapping.Add(SVRA_ButtonAssistant.X, Valve.VR.EVRButtonId.k_EButton_A);
        buttonMapping.Add(SVRA_ButtonAssistant.Y, Valve.VR.EVRButtonId.k_EButton_A);
    }

    private void RenderModelLoaded(SteamVR_RenderModel renderModel, bool success)
    {
        if ( (int) renderModel.index == deviceIndex )
        {
            renderModelInitialised = success;
        } 

        if (playerHand != null && playerHand.CurrentHandState != PlayerHandState)
        {
            playerHand.Initialize();
        }
    }

    private void SetDeviceIndex(int index)
    {
        deviceIndex = index;
        controller= SteamVR_Controller.Input(index);
    }

    protected void UpdateKnucklesFingerCurl()
    {
        if (Time.unscaledTime != lastChecked)
        {
            wasGripped = isGripped;

            indexCurl = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis3).x;
            middleCurl = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis3).y;
            ringCurl = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis4).x;
            pinkyCurl = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis4).y;
            thumbTouch = controller.GetTouch(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

            isGripped = IsKnucklesGripped();
        }

        lastChecked = Time.unscaledTime;
    }

    protected bool IsKnucklesGripped()
    {
        return (indexCurl > curlAmountNeeded) || (middleCurl > curlAmountNeeded) || (ringCurl > curlAmountNeeded) || (pinkyCurl > curlAmountNeeded);
    }
}
    