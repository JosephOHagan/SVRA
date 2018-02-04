/* 
 * TODO : Rework these for all buttons (ButtonHelper) 
 * TODO : Add teleportation button and action 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

delegate bool InputFunction(ulong key);

[DisallowMultipleComponent]
public class SVRA_ControllerManager : MonoBehaviour {  

    public enum InputButton {
        Grip,
        Trigger,
        Both,
        None
    };

    public enum ActionType
    {
        Grab,
        Interact
    };

    [Tooltip("The button used for grip actions")]
    public InputButton gripInput = InputButton.Grip;

    [Tooltip("The button used for interaction actions")]
    public InputButton interactionInput = InputButton.Trigger;

    private bool gripOrTriggerHeld = false;
    private bool gripOrTriggerPressed = false;

    /* NOTE: deviceIndex may be set without having a trackedObject */
    private int deviceIndex = -1;

    private const float MAX_VIBRATION_STRENGTH = 3999f;

    /* Setup controller reference and get method for external access and referencing */
    [Tooltip("The controller giving the input (left to left controller and right to right controller)")]
    public SteamVR_TrackedObject trackedControllerObject;

    public SteamVR_Controller.Device controller
    {
        get
        {
            return SteamVR_Controller.Input(deviceIndex);
        }
    }

    void Start ()
    {
	    if (trackedControllerObject)
        {
            deviceIndex = (int)trackedControllerObject.index;
        }
	}
		
	void Update ()
    {
		if (controller == null)
        {
            return;
        }

        /* Case of grip or trigger button being pressed */
        if (InputPerformed(InputButton.Grip, controller.GetPress) ||
            InputPerformed(InputButton.Trigger, controller.GetPress))
        {
            gripOrTriggerPressed = !gripOrTriggerHeld;
            gripOrTriggerHeld = true;
        }

        /* Case of neither button being pressed */
        if (!InputPerformed(InputButton.Grip, controller.GetPress) &&
            !InputPerformed(InputButton.Trigger, controller.GetPress))
        {
            gripOrTriggerPressed = false;
            gripOrTriggerHeld = false;
        }
    }

    /* Support for SteamVR's Interaction System (see Hand#InitController) */
    void OnHandInitialized(int index)
    {
        deviceIndex = index;
    }

    /* Check the controllers 3 states (pressed, released, held) using through the SteamVR device methods */
    public bool Pressed(ActionType action)
    {
        if (controller == null)
        {
            return false;        
        }

        InputButton inputButton = InputAction(action);
        return InputPerformed(inputButton, controller.GetPressDown);
    }

    public bool Released(ActionType action)
    {
        if (controller == null)
        {
            return false;
        }

        InputButton inputButton = InputAction(action);
        return InputPerformed(inputButton, controller.GetPressUp);
    }

    public bool Held(ActionType action)
    {
        if (controller == null)
        {
            return false;
        }

        InputButton inputButton = InputAction(action);
        return InputPerformed(inputButton, controller.GetPress);
    }

    /* Determine the type of the input action and return corresponding input button */
    InputButton InputAction(ActionType action)
    {
        switch (action)
        {
            case ActionType.Grab:
                return gripInput;
            case ActionType.Interact:
                return interactionInput;
            default:
                return InputButton.None;
        }
    }

    /* Used to translate the parameter of the input function to the SteamVR button equivalent */
    bool InputPerformed(InputButton inputButton, InputFunction function)
    {
        switch (inputButton)
        {
            case InputButton.Grip:
                return function(ButtonMask(InputButton.Grip));
            case InputButton.Trigger:
                return function(ButtonMask(InputButton.Trigger));
            case InputButton.Both:
                return BothInputPerformed(function);
            case InputButton.None:
            default:
                return false;
        }
    }

    bool BothInputPerformed(InputFunction function)
    {
        switch (function.Method.Name)
        {
            case "GetPressDown":
                return gripOrTriggerPressed;
            case "GetPress":
                return gripOrTriggerHeld;
            case "GetPressUp":
                return !gripOrTriggerHeld;
            default:
                return false;
        }
    }

    /* Perform the button mapping lookup for SteamVR button equivalent */
    ulong ButtonMask(InputButton inputButton)
    {
        switch (inputButton)
        {
            case InputButton.Grip:
                return SteamVR_Controller.ButtonMask.Grip;
            case InputButton.Trigger:
                return SteamVR_Controller.ButtonMask.Trigger;
            case InputButton.Both:
                return SteamVR_Controller.ButtonMask.Touchpad;
            default:
            case InputButton.None:
                return (1ul << (int) Valve.VR.EVRButtonId.k_EButton_Max + 1);
        }
    }

    /* Activate controller vibration on interaction or event */
    public void Vibration(int milliSec, float strength)
    {
        float seconds = milliSec / 1000f;
        StartCoroutine(LongVibration(seconds, strength));
    }

    IEnumerator LongVibration(float length, float strength)
    {
        for ( float i = 0; i < length; i += Time.deltaTime )
        {
            if ( controller != null )
            {
                ushort vibration = (ushort)Mathf.Lerp(0, MAX_VIBRATION_STRENGTH, strength);
                controller.TriggerHapticPulse(vibration);
            }
            yield return null;
        }
    }
}
