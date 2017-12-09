using UnityEngine;
using Valve.VR;

public class SVRA_InteractiveObjectClass : MonoBehaviour
{
/*
    /// <summary>
    /// Called when controller passes into trigger
    /// </summary>
    /// <param name="controller"></param>
    public virtual void TriggerEnter(SVRA_ControllerSetup controller)
    {
    }

    /// <summary>
    /// Called when controller passes out of trigger
    /// </summary>
    /// <param name="controller"></param>
    public virtual void TriggerExit(SVRA_ControllerSetup controller)
    {
    }
*/
    /// <summary>
    /// Called when button is pressed down while controller is inside object
    /// </summary>
    /// <param name="controller"></param>
    public virtual void ButtonPressDown(EVRButtonId button, SVRA_ControllerSetup controller) {}

    /// <summary>
    /// Called when button is released after an object has been "grabbed".
    /// </summary>
    /// <param name="controller"></param>
    public virtual void ButtonPressUp(EVRButtonId button, SVRA_ControllerSetup controller) {}
    
}