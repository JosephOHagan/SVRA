using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_Slider : MonoBehaviour {

    private SVRA_ControllerManager controller;

    private Vector3 previousPosition;
    private Vector3 newPosition;

    [Tooltip("The vibration duration in milliseconds")]
    public int VIBRATION_DURATION = 50;

    public float MAX_VIBRATION_STRENGTH = 0.2f;
    public float MAX_VIBRATION_DISTANCE = 0.03f;    

    void Start () {
        previousPosition = transform.position;
    }
   
    void Update () {
        newPosition = transform.position;

        if (controller != null)
        {           
            float distance = Mathf.Min(minimumDifference(newPosition, previousPosition), MAX_VIBRATION_DISTANCE);
            float vibrationStrength = (distance / MAX_VIBRATION_DISTANCE) * MAX_VIBRATION_STRENGTH;

            controller.Vibration(VIBRATION_DURATION, vibrationStrength);
        }

        previousPosition = newPosition;
    }

    void SVRAGrabStart(SVRA_GrabPoint grabPoint)
    {
        controller = grabPoint.controller;
    }

    void SVRAGrabStop()
    {
        controller = null;
    }

    float minimumDifference(Vector3 newPosition, Vector3 previousPosition)
    {
        float[] xyzArray = { Mathf.Abs(newPosition.x - previousPosition.x),
            Mathf.Abs(newPosition.y - previousPosition.y),
            Mathf.Abs(newPosition.z - previousPosition.z) };

        return Mathf.Max(xyzArray);    
    }
}
