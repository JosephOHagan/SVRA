using System.Collections;
using UnityEngine;

public class SVRA_InteractButton : MonoBehaviour {

    [System.Serializable]
    public class AnimationMotion
    {
        [Tooltip("Move on the x axis")]
        public bool xAxis;

        [Tooltip("Move on the y axis")]
        public bool yAxis;

        [Tooltip("Move on the z axis")]
        public bool zAxis;

        [Tooltip("Reverse the direction of movement")]
        public bool reverseDirection;

        [Tooltip("The distance to move")]
        public float movementDistance = 0.02f;

        [Tooltip("Speed of button animation")]
        public float animationSpeed = 0.08f;
    }


    [System.Serializable]
    public class Vibration
    {
        public bool toggleVibration = false;

        [Tooltip("Duration of vibration (milliseconds)")]
        public int duration = 25;

        [Tooltip("Intensity of vibration")]
        public float strength = 0.4f;
    }

    public AnimationMotion animationMotion;
    public Vibration vibration;

    private float distance;
    private int direction = 1;

    void Start () {
        if (animationMotion.reverseDirection)
        {
            direction *= -1;
        }

        distance = animationMotion.movementDistance;
    }

    void SVRAInteractionStart(SVRA_GrabPoint grabPoint)
    {
        if (vibration.toggleVibration)
        {
            grabPoint.controller.Vibration(vibration.duration, vibration.strength);
        }

        ButtonEvent();
    }

    public void ButtonEvent()
    {
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
        distance = animationMotion.movementDistance;

        while (distance > 0)
        {
            Increment();
            yield return null;
        }

        direction *= -1;
        distance = animationMotion.movementDistance;
    }

    void Increment ()
    {     
        float increment = Time.deltaTime * animationMotion.animationSpeed;
        increment = Mathf.Min(increment, distance);

        transform.Translate(increment * direction * BoolToInt(animationMotion.xAxis),
            increment * direction * BoolToInt(animationMotion.yAxis),
            increment * direction * BoolToInt(animationMotion.zAxis));

        distance -= increment;
    }

    int BoolToInt(bool input)
    {
        return input ? 1 : 0;
    }
}
