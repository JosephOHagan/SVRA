using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SVRA_PlayerHand : MonoBehaviour {

    public SVRA_ButtonAssistant HoldButton = SVRA_ButtonAssistant.Grip;
    public bool HoldButtonDown = false;
    public bool HoldButtonUp = false;
    public bool HoldButtonPressed = false;
    public float HoldButtonAxis = 0f;

    public SVRA_ButtonAssistant UseButton = SVRA_ButtonAssistant.Trigger;
    public bool UseButtonDown = false;
    public bool UseButtonUp = false;
    public bool UseButtonPressed = false;
    public float UseButtonAxis = 0f;
    internal bool HasCustomModel;
    internal object CurrentHandState;

    internal void Initialize()
    {
        throw new NotImplementedException();
    }
}
