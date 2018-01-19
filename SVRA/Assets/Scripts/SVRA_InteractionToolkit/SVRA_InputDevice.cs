using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class SVRA_InputDevice : MonoBehaviour {
    protected SVRA_PlayerHand playerHand;

    public virtual void Initialize(SVRA_PlayerHand hand)
    {
        playerHand = hand;
    }

    public abstract bool IsCurrentlyTracked { get; }

    public abstract Collider[] SetupDefaultPhysicalColliders(Transform ModelParent);

    public abstract GameObject SetupDefaultRenderModel();

    public abstract bool ReadyToInitialise();

    public abstract Collider[] SetupDefaultColliders();

    public abstract string GetDeviceName();

    public abstract void TriggerHapticPulse(ushort durationMicroSec = 500, SVRA_ButtonAssistant button = SVRA_ButtonAssistant.Touchpad);

    public abstract float GetAxis1D(SVRA_ButtonAssistant button);
    public abstract Vector2 GetAxis2D(SVRA_ButtonAssistant button);
    public abstract bool GetPressDown(SVRA_ButtonAssistant button);
    public abstract bool GetPressUp(SVRA_ButtonAssistant button);
    public abstract bool GetPress(SVRA_ButtonAssistant button);
    public abstract bool GetTouchDown(SVRA_ButtonAssistant button);
    public abstract bool GetTouchUp(SVRA_ButtonAssistant button);
    public abstract bool GetTouch(SVRA_ButtonAssistant button);
    public abstract bool GetNearTouchDown(SVRA_ButtonAssistant button);
    public abstract bool GetNearTouchUp(SVRA_ButtonAssistant button);
    public abstract bool GetNearTouch(SVRA_ButtonAssistant button);

}
