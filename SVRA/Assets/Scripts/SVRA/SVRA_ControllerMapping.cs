using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_ControllerMapping : MonoBehaviour {

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
}
