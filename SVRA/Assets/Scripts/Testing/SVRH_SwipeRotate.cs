using UnityEngine;

// The idea here will be to implement a 90 degree rotation when
// swipe motions are detected on the controllers
public class SVRH_SwipeRotate : MonoBehaviour {

    public float _mMoveSpeed = 2.5f;
    public float _mHorizontalTurnSpeed = 180f;
    public float _mVerticalTurnSpeed = 2.5f;
    public bool _mInverted = false;
    public const float VERTICAL_LIMIT = 60f;


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
        Valve.VR.InteractionSystem.Player player = Valve.VR.InteractionSystem.Player.instance;
        if (!player)
        {
            return;
        }

        // Get the touchpad 
        Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

        if (null != player.rightController)
        {

            
            // Touch down, possible chance for a swipe
            if (player.rightController.GetTouchDown(Valve.VR.EVRButtonId.k_EButton_Axis0))
            {
                trackingSwipe = true;
                // Record start time and position
                mStartPosition = new Vector2(player.rightController.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).x,
                    player.rightController.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y);
                mSwipeStartTime = Time.time;
            }
            // Touch up , possible chance for a swipe
            else if (player.rightController.GetTouchUp(Valve.VR.EVRButtonId.k_EButton_Axis0))
            {
                trackingSwipe = false;
                trackingSwipe = true;
                checkSwipe = true;
            }
            else if (trackingSwipe)
            {
                endPosition = new Vector2(player.rightController.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).x,
                                          player.rightController.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).y);

            }

            if (checkSwipe)
            {
                checkSwipe = false;

                float deltaTime = Time.time - mSwipeStartTime;

                Vector2 swipeVector = endPosition - mStartPosition;

                float velocity = swipeVector.magnitude / deltaTime;
                Debug.Log(velocity);
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
                        RotateRight();
                    }
                    else if ((180.0f - angleOfSwipe) < mAngleRange)
                    {
                        RotateLeft();
                    }
                    
                }
            }            
        }
    }

    void RotateRight()
    {
        Valve.VR.InteractionSystem.Player player = Valve.VR.InteractionSystem.Player.instance;
        if (!player)
        {
            return;
        }

        Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

        Quaternion orientation = player.transform.rotation;
        var touchPadVector = player.rightController.GetAxis(touchPad);

        Vector3 euler = transform.rotation.eulerAngles;
        float angle;
        if (_mInverted)
        {
            angle = GetAngle(touchPadVector.y);
        }
        else
        {
            angle = GetAngle(-touchPadVector.y);
        }
        euler.y += euler.y += (touchPadVector.x - 90.0f);
        player.transform.rotation = Quaternion.Euler(euler);
    }

    void RotateLeft()
    {
        Valve.VR.InteractionSystem.Player player = Valve.VR.InteractionSystem.Player.instance;
        if (!player)
        {
            return;
        }

        Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

        Quaternion orientation = player.transform.rotation;
        var touchPadVector = player.rightController.GetAxis(touchPad);

        Vector3 euler = transform.rotation.eulerAngles;
        float angle;
        if (_mInverted)
        {
            angle = GetAngle(touchPadVector.y);
        }
        else
        {
            angle = GetAngle(-touchPadVector.y);
        }
        euler.y += (touchPadVector.x + 90.0f);
        player.transform.rotation = Quaternion.Euler(euler);
    }

}
