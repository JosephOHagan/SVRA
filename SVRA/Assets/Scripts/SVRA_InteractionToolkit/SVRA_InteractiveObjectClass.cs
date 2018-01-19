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


    /// <summary>
    /// The PauseCollisions method temporarily pauses all collisions on the object at grab time by removing the object's rigidbody's ability to detect collisions. This can be useful for preventing clipping when initially grabbing an item.
    /// </summary>
    /// <param name="delay">The amount of time to pause the collisions for.</param>
    public virtual void PauseCollisions(float delay)
    {
        if (delay > 0f)
        {
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.detectCollisions = false;
            }
            Invoke("UnpauseCollisions", delay);
        }
    }
}