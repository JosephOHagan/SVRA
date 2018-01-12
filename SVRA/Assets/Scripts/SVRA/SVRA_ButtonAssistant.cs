using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum SVRA_ButtonAssistant
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
        private static SVRA_ButtonAssistant[] array = null;
        public static SVRA_ButtonAssistant[] Array
        {
            get
            {
                if (array == null)
                {
                    array = (SVRA_ButtonAssistant[])System.Enum.GetValues(typeof(SVRA_ButtonAssistant));
                }
                return array;
            }
        }
    }  

