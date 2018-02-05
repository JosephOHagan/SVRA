using System.Collections;
using UnityEngine;

public class SVRA_InteractButton : MonoBehaviour {

    public enum MotionAxis { X, Y, Z };

    [System.Serializable]
    public class Vibration
    {
        public bool toggleVibration = false;
        [Tooltip("Duration of vibration (milliseconds)")]
        public int duration = 25;
        [Tooltip("Intensity of vibration")]
        public float strength = 0.4f;
    }

    public Vibration vibration;

    [Tooltip("Axis on which to move")]
    public MotionAxis motionAxis;

    [Tooltip("Reverse the direction of movement")]
    public bool reverseDirection;

    [Tooltip("The distance to move")]
    public float movementDistance = 0.02f;

    [Tooltip("Speed of button animation")]
    public float animationSpeed = 0.08f;
  
    private float distance;
    private int direction = 1;

    void Start () {
        if (reverseDirection)
        {
            direction *= -1;
        }

        Reset();	
	}

    void SVRAInteractionStart(SVRA_GrabPoint grabPoint)
    {
        if (vibration.toggleVibration)
        {
            grabPoint.controller.Vibration(vibration.duration, vibration.strength);
        }

        ButtonEventTest();

        // GetComponent<SVRA_InteractiveObject>().enabled = false;
        // StartCoroutine("Move");
    }

    public void ButtonEventTest()
    {
        // GetComponent<SVRA_InteractiveObject>().enabled = false;
        StartCoroutine("Move");

    }

    IEnumerator Move()
    {
        while (distance > 0)
        {
            Increment();
            yield return null;
        }

        yield return StartCoroutine("MoveBack");
    }

    IEnumerator MoveBack ()
    {
        direction *= -1;
        Reset();

        while (distance > 0)
        {
            Increment();
            yield return null;
        }

        direction *= -1;
        Reset();
       // GetComponent<SVRA_InteractiveObject>().enabled = true;
    }

    void Increment ()
    {     
        float increment = Time.deltaTime * animationSpeed;
        increment = Mathf.Min(increment, distance);

        switch (motionAxis)
        {
            case MotionAxis.X:
                transform.Translate(increment * direction, 0, 0);
                break;
            case MotionAxis.Y:
                transform.Translate(0, increment * direction, 0);
                break;
            case MotionAxis.Z:
                transform.Translate(0, 0, increment * direction);
                break;
        }

        distance -= increment;
    }

    void Reset()
    {
        distance = movementDistance;
    }
}
