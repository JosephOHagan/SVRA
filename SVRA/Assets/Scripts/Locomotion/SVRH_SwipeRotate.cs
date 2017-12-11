using UnityEngine;

// The idea here will be to implement a 90 degree rotation when
// swipe motions are detected on the controllers
public class SVRH_SwipeRotate : MonoBehaviour {

    // public bool _mInverted = false;
    public const float VERTICAL_LIMIT = 60f;

    public bool rightHandRotation = true;
    public float rotationDegrees = 90.0f;

    private SteamVR_Controller.Device rotationController;
    private Valve.VR.EVRButtonId touchPad;
    Valve.VR.InteractionSystem.Player player;

    private readonly Vector2 mXAxis = new Vector2(1, 0);
    private readonly Vector2 mYAxis = new Vector2(0, 1);
    private bool trackingSwipe = false;
    private bool checkSwipe = false;
    private const float mAngleRange = 30;
    private const float mMinSwipeDist = 0.2f;
    private const float mMinVelocity = 4.0f;
    private Vector2 mStartPosition;
    private Vector2 endPosition;
    private float mSwipeStartTime;


    private int debugCount = 0;

    float GetAngle(float input)
    {
        if (input < 0f)
        {
            return -Mathf.LerpAngle(0, VERTICAL_LIMIT, -input);
        }
        else if (input > 0f)
        {
            return Mathf.LerpAngle(0, VERTICAL_LIMIT, input);
        }
        return 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        // Get the player instance
        player = Valve.VR.InteractionSystem.Player.instance;
        if (!player)
        {
            return;
        }

        // Get the touchpad 
        touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

        if (rightHandRotation)
        {
            rotationController = player.rightController;
        } else
        {
            rotationController = player.leftController;
        }

        if (null != rotationController)
        {
            // Touch down possible chance for a swipe
            if (rotationController.GetTouchDown(Valve.VR.EVRButtonId.k_EButton_Axis0))
            {
                trackingSwipe = true;

                // Record start time and position
                mStartPosition = new Vector2(rotationController.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).x, rotationController.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y);
                mSwipeStartTime = Time.time;
            }
            // Touch up possible chance for a swipe
            else if (rotationController.GetTouchUp(Valve.VR.EVRButtonId.k_EButton_Axis0))
            {
                trackingSwipe = false;
                trackingSwipe = true;
                checkSwipe = true;
            }
            else if (trackingSwipe)
            {
                endPosition = new Vector2(rotationController.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).x, rotationController.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y);
            }

            if (checkSwipe)
            {
                checkSwipe = false;

                float deltaTime = Time.time - mSwipeStartTime;

                Vector2 swipeVector = endPosition - mStartPosition;

                float velocity = swipeVector.magnitude / deltaTime;

                // Debug.Log(velocity);

                if (velocity > mMinVelocity &&
                    swipeVector.magnitude > mMinSwipeDist)
                {
                    // if the swipe has enough velocity and enough distance
                    swipeVector.Normalize();

                    float angleOfSwipe = Vector2.Dot(swipeVector, mXAxis);
                    angleOfSwipe = Mathf.Acos(angleOfSwipe) * Mathf.Rad2Deg;

                    // Detect left and right swipe
                    if (angleOfSwipe < mAngleRange)
                    {
                        Debug.Log(debugCount++);
                        Debug.Log(player.feetPositionGuess);

                        player.transform.Rotate(0, rotationDegrees, 0);
                        // RotatePlayerSnap(player, touchPad, rotationController, rotationDegrees);
                        
                        Debug.Log(player.feetPositionGuess);                        
                    }
                    else if ((180.0f - angleOfSwipe) < mAngleRange)
                    {
                        Debug.Log(player.feetPositionGuess);

                        player.transform.Rotate(0, -rotationDegrees, 0);
                        
                        /*
                        player.hmdTransform
                        var pos = playArea.TransformPoint(bodyCollider.center);
                        playArea.Rotate(Vector3.up, angle);
                        pos -= playArea.TransformPoint(bodyCollider.center);
                        playArea.position += pos;
                        */

                        // RotatePlayerSnap(player, touchPad, rotationController, -rotationDegrees);        <- same effect

                        Debug.Log(player.feetPositionGuess);
                    }
                    
                }
            }            
        }
    }

    // OLD METHOD
    void RotatePlayerSnap(Valve.VR.InteractionSystem.Player player, Valve.VR.EVRButtonId touchPad, SteamVR_Controller.Device rotationController, float rotationDegrees)
    {
        Vector2 touchPadVector = rotationController.GetAxis(touchPad);
        Vector3 euler = transform.rotation.eulerAngles;
        float angle = GetAngle(touchPadVector.y);
        euler.y += (touchPadVector.x + rotationDegrees);
        player.transform.rotation = Quaternion.Euler(euler);
    }
}
