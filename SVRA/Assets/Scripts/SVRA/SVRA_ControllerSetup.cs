using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SVRA_ControllerSetup : MonoBehaviour
{
    public SVRA_ButtonAssistant pickupButton = SVRA_ButtonAssistant.Trigger;
    public SVRA_ButtonAssistant useButton = SVRA_ButtonAssistant.Grip;
    private Dictionary<SVRA_ButtonAssistant, Valve.VR.EVRButtonId> ButtonMapping = new Dictionary<SVRA_ButtonAssistant, Valve.VR.EVRButtonId>(new SVRA_ButtonComparator());

    protected Dictionary<Valve.VR.EVRButtonId, List<SVRA_InteractiveObjectClass>> pressedDownObjects;
    protected List<SVRA_InteractiveObjectClass> overlappedObjects;
    protected List<Valve.VR.EVRButtonId> trackedButtons;

    // Setup controller reference    
    protected SteamVR_TrackedObject trackedObject;
    public SteamVR_Controller.Device controller
    {
        get
        {
            return SteamVR_Controller.Input((int)trackedObject.index);
        }
    }

    void Awake()
    {
        // Setup button name remapping
        SetupButtonMapping();

        trackedObject = GetComponent<SteamVR_TrackedObject>();

        // Intstantiate the lists
        pressedDownObjects = new Dictionary<Valve.VR.EVRButtonId, List<SVRA_InteractiveObjectClass>>();
        overlappedObjects = new List<SVRA_InteractiveObjectClass>();

        //List the buttons you send inputs to the controller for
        trackedButtons = new List<Valve.VR.EVRButtonId>()
        {
            // Todo : Add more here
            Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger,
            Valve.VR.EVRButtonId.k_EButton_Grip
        };
    }

    // Used to configure the default state of the required components
    void Reset()
    {
        this.GetComponent<SphereCollider>().isTrigger = true;
        this.GetComponent<SphereCollider>().center = new Vector3(0.0f, -0.04f, 0.0f);
        this.GetComponent<SphereCollider>().radius = 0.05f;
    }

    void Update()
    {
        //Check through all desired buttons to see if any have been released
        Valve.VR.EVRButtonId[] pressKeys = pressedDownObjects.Keys.ToArray();

        for (int i = 0; i < pressKeys.Length; i++)
        {
            //If tracked button is released
            if (controller.GetPressUp(pressKeys[i]))
            {
                //Get all tracked objects in that button's "pressed" list
                List<SVRA_InteractiveObjectClass> releaseObjects = pressedDownObjects[pressKeys[i]];

                for (int j = 0; j < releaseObjects.Count; j++)
                {
                    //Send button release through to interactive script
                    releaseObjects[j].ButtonPressUp(pressKeys[i], this);
                }

                //Clear button's associated list
                pressedDownObjects[pressKeys[i]].Clear();
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {       
        if (collider.attachedRigidbody != null)
        {
            // If the rigidbody's object has interactive object scripts 
            // attached then iterate through them
            SVRA_InteractiveObjectClass[] interactiveObjects = collider.attachedRigidbody.GetComponents<SVRA_InteractiveObjectClass>();

            for (int i = 0; i < interactiveObjects.Length; i++)
            {
                SVRA_InteractiveObjectClass interactiveObject = interactiveObjects[i];

                for (int b = 0; b < trackedButtons.Count; b++)
                {
                    // If a tracked button is pressed
                    Valve.VR.EVRButtonId button = trackedButtons[b];
                    if (controller.GetPressDown(button))
                    {
                        // If we haven't already sent the button press message to this object
                        // Safeguard against objects that have multiple colliders for one interactive script
                        if (!pressedDownObjects.ContainsKey(button) || !pressedDownObjects[button].Contains(interactiveObject))
                        {
                            // Send button press through to interactive script
                            interactiveObject.ButtonPressDown(button, this);

                            // Add interactive script to a dictionary flagging it to recieve notice
                            // when that same button is released
                            if (!pressedDownObjects.ContainsKey(button))
                            {
                                pressedDownObjects.Add(button, new List<SVRA_InteractiveObjectClass>());
                            }                                

                            pressedDownObjects[button].Add(interactiveObject);
                        }
                    }
                }
            }
        }
    }


    
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

    /*
        void OnTriggerEnter(Collider collider)
        {
            if (collider.attachedRigidbody != null)
            {
                SVRA_InteractiveObjectClass[] interactiveObjects = collider.attachedRigidbody.GetComponents<SVRA_InteractiveObjectClass>();

                for (int i = 0; i < interactiveObjects.Length; i++)
                {
                    SVRA_InteractiveObjectClass interactiveObject = interactiveObjects[i];
                    interactiveObject.TriggerEnter(this);
                }
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.attachedRigidbody != null)
            {
                SVRA_InteractiveObjectClass[] interactiveObjects = collider.attachedRigidbody.GetComponents<SVRA_InteractiveObjectClass>();

                for (int i = 0; i < interactiveObjects.Length; i++)
                {
                    SVRA_InteractiveObjectClass interactiveObject = interactiveObjects[i];
                    interactiveObject.TriggerExit(this);
                }
            }
        }
    */

}
