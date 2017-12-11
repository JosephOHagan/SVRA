using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// See NVRSteamVRInputDevice for next step

public class SVRA_ButtonAssistant : MonoBehaviour {
    public enum SVRA_Buttons
    {
        System,
        ApplicationMenu,
        Grip,
        DPad_Left,
        DPad_Up,
        DPad_Right,
        DPad_Down,
        A,
        B,
        X,
        Y,
        Axis0,
        Axis1,
        Axis2,
        Axis3,
        Axis4,
        Touchpad,
        Trigger,
        Back,
        Stick
    }

    public class SVRA_ButtonsHelper
    {
        private static SVRA_Buttons[] array = null;
        public static SVRA_Buttons[] Array
        {
            get
            {
                if (array == null)
                {
                    array = (SVRA_Buttons[])System.Enum.GetValues(typeof(SVRA_Buttons));
                }
                return array;
            }
        }
    }
}
