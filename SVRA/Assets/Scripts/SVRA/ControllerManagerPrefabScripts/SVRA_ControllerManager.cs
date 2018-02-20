using UnityEngine;
using System.Collections;
using System.Collections.Generic;

delegate bool InputFunction(ulong key);

[AddComponentMenu("")]  // Internally used script so hide from user in the Inspector window
[DisallowMultipleComponent]
public class SVRA_ControllerManager : MonoBehaviour {

    [System.Serializable]
    public class VirtualTouchpadButtons
    {
        [Tooltip("The threshold for registering input on the trackpad")]
        public float threshold = 0.3f;

        [Tooltip("The action assigned to the up position of the trackpad")]
        public ActionType upTouchpadButton = ActionType.None;

        [Tooltip("The action assigned to the down position of the trackpad")]
        public ActionType downTouchpadButton = ActionType.None;

        [Tooltip("The action assigned to the left position of the trackpad")]
        public ActionType leftTouchpadButton = ActionType.None;

        [Tooltip("The action assigned to the right position of the trackpad")]
        public ActionType rightTouchpadButton = ActionType.None;

        [Tooltip("The action assigned to the centre position of the trackpad")]
        public ActionType centreTouchpadButton = ActionType.None;
    }

    public enum InputButton
    {
        Grip,
        Trigger,
        Touchpad,
        ApplicationMenu,
        TouchpadUp,
        TouchpadDown,
        TouchpadLeft,
        TouchpadRight,
        TouchpadCentre,
        None
    };

    public enum ActionType
    {
        Grab,
        Interact,
        None
    };

    [Tooltip("The controller giving the input (left to left controller and right to right controller)")]
    public SteamVR_TrackedObject trackedControllerObject;

    [Tooltip("The action assigned to the grip button")]
    public ActionType gripButton = ActionType.Grab;

    [Tooltip("The action assigned to the trigger button")]
    public ActionType triggerButton = ActionType.Grab;

    [Tooltip("The action assigned to the trackpad")]
    public ActionType touchpadButton = ActionType.Interact;

    [Tooltip("The action assigned to the application menu button")]
    public ActionType applicationMenuButton = ActionType.None;

    [Tooltip("The actions assigned to the dpad buttons")]
    public VirtualTouchpadButtons virtualTouchpadButtons;    

    private InputButton gripInput = InputButton.Grip;
    private InputButton triggerInput = InputButton.Trigger;
    private InputButton touchpadInput = InputButton.Touchpad;
    private InputButton applicationMenuInput = InputButton.ApplicationMenu;

    private InputButton touchpadUpDPadInput = InputButton.TouchpadUp;
    private InputButton touchpadDownDPadInput = InputButton.TouchpadDown;
    private InputButton touchpadLeftDPadInput = InputButton.TouchpadLeft;
    private InputButton touchpadRightDPadInput = InputButton.TouchpadRight;
    private InputButton touchpadCentreDPadInput = InputButton.TouchpadCentre;
    
    // NOTE: deviceIndex may be set without having a trackedObject
    private int deviceIndex = -1;

    private const float MAX_VIBRATION_STRENGTH = 3999f;

    private Dictionary<ActionType, ArrayList> controllerDictionary = new Dictionary<ActionType, ArrayList>();

    private Dictionary<ActionType, ArrayList> setupControllerButtons()
    {
        // Initialise all possible actions within hashmap
        ActionType[] actionTypeEnums = (ActionType[])System.Enum.GetValues(typeof(ActionType));
        int i = 0;

        while (i < (actionTypeEnums.Length - 1))
        {
            controllerDictionary.Add(actionTypeEnums[i], new ArrayList());
            i++;
        }

        // Map the physical buttons to the user specified actions        
        DictionaryAddition(gripButton, gripInput);
        DictionaryAddition(triggerButton, triggerInput);
        DictionaryAddition(touchpadButton, touchpadInput);
        DictionaryAddition(applicationMenuButton, applicationMenuInput);
        
        // Map the virtual touchpad buttons to the user specified actions
        DictionaryAddition(virtualTouchpadButtons.upTouchpadButton, touchpadUpDPadInput);
        DictionaryAddition(virtualTouchpadButtons.downTouchpadButton, touchpadDownDPadInput);
        DictionaryAddition(virtualTouchpadButtons.leftTouchpadButton, touchpadLeftDPadInput);
        DictionaryAddition(virtualTouchpadButtons.rightTouchpadButton, touchpadRightDPadInput);
        DictionaryAddition(virtualTouchpadButtons.centreTouchpadButton, touchpadCentreDPadInput);
 
        return controllerDictionary;
    }

    private void DictionaryAddition(ActionType inputAction, InputButton inputButton)
    {
        if ( controllerDictionary.ContainsKey(inputAction) )
        {
            controllerDictionary[inputAction].Add(inputButton);
        }
        else
        {
            controllerDictionary.Add(inputAction, new ArrayList() { inputButton });
        }
    }

    public SteamVR_Controller.Device controller
    {
        get
        {
            return SteamVR_Controller.Input(deviceIndex);
        }
    }

    void Start()
    {
        if (trackedControllerObject)
        {
            deviceIndex = (int)trackedControllerObject.index;
        }

        setupControllerButtons();
    }

    void Update()
    {
        if (controller == null)
        {
            return;
        }
    }

    /* 
     * Support for SteamVR's Interaction System (see Hand#InitController) 
     * Double check / experiment with this method     
     */
    void OnHandInitialized(int index)
    {
        deviceIndex = index;
    }

    /* Check the controllers 3 states (pressed, released, held) */
    public bool Pressed(ActionType action)
    {
        return (controller != null) ? InputPerformed(controllerDictionary[action], controller.GetPressDown) : false;
    }

    public bool Released(ActionType action)
    {
        return (controller != null) ? InputPerformed(controllerDictionary[action], controller.GetPressUp) : false;
    }

    public bool Held(ActionType action)
    {
        return (controller != null) ? InputPerformed(controllerDictionary[action], controller.GetPress) : false;
    }


    /* Used to translate the parameter of the input function to the SteamVR button equivalent */
    bool InputPerformed(ArrayList inputButton, InputFunction function)
    {
        if (inputButton.Count == 0)
        {
            return false;
        }
        else if (inputButton.Count == 1)
        {
            return function( ButtonMask((InputButton) inputButton[0]) );
        }
        else
        {
            for (int i = 0; i < inputButton.Count; i++)
            {
                if ( function( ButtonMask((InputButton) inputButton[i]) ) )
                {
                    return true;
                }
            }

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
            case InputButton.Touchpad:
                return SteamVR_Controller.ButtonMask.Touchpad;
            case InputButton.ApplicationMenu:
                return SteamVR_Controller.ButtonMask.ApplicationMenu;
            case InputButton.TouchpadUp:
                return checkVirtualButtons(InputButton.TouchpadUp);
            case InputButton.TouchpadDown:
                return checkVirtualButtons(InputButton.TouchpadDown);
            case InputButton.TouchpadLeft:
                return checkVirtualButtons(InputButton.TouchpadLeft);
            case InputButton.TouchpadRight:
                return checkVirtualButtons(InputButton.TouchpadRight);
            case InputButton.TouchpadCentre:
                return checkVirtualButtons(InputButton.TouchpadCentre);
            case InputButton.None:
            default:            
                return (1ul << (int)Valve.VR.EVRButtonId.k_EButton_Max + 1);
        }
    }

    /* Check if the user is pressing the touchpad and if its within the specified threshold */
    ulong checkVirtualButtons(InputButton inputButton)
    {
        // Check for pressing the touchpad
        if (controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpadAxis = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

            switch (inputButton)
            {
                case InputButton.TouchpadUp:
                    if (touchpadAxis.y > (1.0f - virtualTouchpadButtons.threshold))
                    {
                        return SteamVR_Controller.ButtonMask.Touchpad;
                    }
                    break;

                case InputButton.TouchpadDown:
                    if (touchpadAxis.y < (-1.0f + virtualTouchpadButtons.threshold))
                    {
                        return SteamVR_Controller.ButtonMask.Touchpad;
                    }
                    break;

                case InputButton.TouchpadLeft:
                    if (touchpadAxis.x < (-1.0f + virtualTouchpadButtons.threshold))
                    {
                        return SteamVR_Controller.ButtonMask.Touchpad;
                    }
                    break;

                case InputButton.TouchpadRight:
                    if (touchpadAxis.x > (1.0f - virtualTouchpadButtons.threshold))
                    {
                        return SteamVR_Controller.ButtonMask.Touchpad;
                    }
                    break;

                case InputButton.TouchpadCentre:
                    if (touchpadAxis.y < ( 1.0f - virtualTouchpadButtons.threshold) &&
                        touchpadAxis.y > (-1.0f + virtualTouchpadButtons.threshold) &&
                        touchpadAxis.x > (-1.0f + virtualTouchpadButtons.threshold) &&
                        touchpadAxis.x < ( 1.0f - virtualTouchpadButtons.threshold)  )
                    {
                        return SteamVR_Controller.ButtonMask.Touchpad;
                    }
                    break;

                default:
                    return 0;           // Assign to invalid value to avoid 'phantom button' / 'learning button' bug
                    // return (1ul << (int)Valve.VR.EVRButtonId.k_EButton_Max + 1);
            }

        }

        // Check for releasing the touchpad
        if (controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpadAxis2 = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

            switch (inputButton)
            {
                case InputButton.TouchpadUp:
                    if (touchpadAxis2.y > (1.0f - virtualTouchpadButtons.threshold))
                    {
                        return SteamVR_Controller.ButtonMask.Touchpad;
                    }
                    break;

                case InputButton.TouchpadDown:
                    if (touchpadAxis2.y < (-1.0f + virtualTouchpadButtons.threshold))
                    {
                        return SteamVR_Controller.ButtonMask.Touchpad;
                    }
                    break;

                case InputButton.TouchpadLeft:
                    if (touchpadAxis2.x < (-1.0f + virtualTouchpadButtons.threshold))
                    {
                        return SteamVR_Controller.ButtonMask.Touchpad;
                    }
                    break;

                case InputButton.TouchpadRight:
                    if (touchpadAxis2.x > (1.0f - virtualTouchpadButtons.threshold))
                    {
                        return SteamVR_Controller.ButtonMask.Touchpad;
                    }
                    break;

                case InputButton.TouchpadCentre:
                    if (touchpadAxis2.y < ( 1.0f - virtualTouchpadButtons.threshold) &&
                        touchpadAxis2.y > (-1.0f + virtualTouchpadButtons.threshold) &&
                        touchpadAxis2.x > (-1.0f + virtualTouchpadButtons.threshold) &&
                        touchpadAxis2.x < ( 1.0f - virtualTouchpadButtons.threshold)  )
                    {
                        return SteamVR_Controller.ButtonMask.Touchpad;
                    }
                    break;

                default:
                    return 0;           // Assign to invalid value to avoid 'phantom button' / 'learning button' bug
                    // return (1ul << (int)Valve.VR.EVRButtonId.k_EButton_Max + 1);
            }
        }
        
        return 0;           // Assign to invalid value to avoid 'phantom button' / 'learning button' bug
        // return (1ul << (int)Valve.VR.EVRButtonId.k_EButton_Max + 1);
    }

    /* Activate controller vibration on interaction or event */
    public void Vibration(int milliSec, float strength)
    {
        float seconds = milliSec / 1000f;
        StartCoroutine(LongVibration(seconds, strength));
    }

    IEnumerator LongVibration(float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            if (controller != null)
            {
                ushort vibration = (ushort) Mathf.Lerp(0, MAX_VIBRATION_STRENGTH, strength);
                controller.TriggerHapticPulse(vibration);
            }
            yield return null;
        }
    }

}
