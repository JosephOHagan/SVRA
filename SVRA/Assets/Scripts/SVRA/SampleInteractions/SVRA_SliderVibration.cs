using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_SliderVibration : MonoBehaviour {

    private SVRA_ControllerManager controller;
    private SVRA_GrabPoint grabPoint;

    private Vector3 previousPosition;
    private Vector3 newPosition;

 //   public Transform boundaryPointA;
 //   public Transform boundaryPointB;

    [Tooltip("The vibration duration in milliseconds")]
    public int VIBRATION_DURATION = 50;

    public float MAX_VIBRATION_STRENGTH = 0.2f;
    public float MAX_VIBRATION_DISTANCE = 0.03f;    

    void Start ()
    {
        previousPosition = transform.position;
    }
   
    void Update ()
    {
//        if (checkBoundaries(transform.position, boundaryPointA.position))
//        {


            newPosition = transform.position;

            if (controller != null)
            {
                float distance = Mathf.Min(minimumDifference(newPosition, previousPosition), MAX_VIBRATION_DISTANCE);
                float vibrationStrength = (distance / MAX_VIBRATION_DISTANCE) * MAX_VIBRATION_STRENGTH;

                controller.Vibration(VIBRATION_DURATION, vibrationStrength);
            }

            previousPosition = newPosition;

 //       }

/*        
 *      else
        {
            Debug.Log("Past Boundary");
            if (grabPoint != null)
            {
                // Stop the object from moving
                GetComponentInParent<Rigidbody>().position = boundaryPointA.position;

                // De-grab item
                grabPoint.DestroyConnection();                
            }
        }
*/             
    }

    // Possibly add select axis and just compare those instead
    bool checkBoundaries(Vector3 transform, Vector3 boundary)
    {       
        if (transform.z > boundary.z)
        {
            return true;
        }

        return false;
    }

    void SVRAGrabStart(SVRA_GrabPoint grabPoint)
    {
        controller = grabPoint.controller;
//        this.grabPoint = grabPoint;
    }

    void SVRAGrabStop()
    {
        controller = null;
//        this.grabPoint = null;
    }

    float minimumDifference(Vector3 newPosition, Vector3 previousPosition)
    {
        float[] xyzArray = { Mathf.Abs(newPosition.x - previousPosition.x),
            Mathf.Abs(newPosition.y - previousPosition.y),
            Mathf.Abs(newPosition.z - previousPosition.z) };

        return Mathf.Max(xyzArray);    
    }
}
