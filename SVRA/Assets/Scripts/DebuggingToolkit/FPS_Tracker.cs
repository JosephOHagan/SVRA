using UnityEngine;
using UnityEngine.UI;
    
public class FPS_Tracker : MonoBehaviour
{
    [Tooltip("Toggles whether the FPS text is visible.")]
    public bool displayFPS = true;
    [Tooltip("The frames per second deemed acceptable that is used as the benchmark to change the FPS text colour.")]
    public int targetFPS = 90;
    [Tooltip("The size of the font the FPS is displayed in.")]
    public int fontSize = 32;
    [Tooltip("The position of the FPS text within the headset view.")]
    public Vector3 position = Vector3.zero;
    [Tooltip("The colour of the FPS text when the frames per second are within reasonable limits of the Target FPS.")]
    public Color goodColor = Color.green;
    [Tooltip("The colour of the FPS text when the frames per second are falling short of reasonable limits of the Target FPS.")]
    public Color warnColor = Color.yellow;
    [Tooltip("The colour of the FPS text when the frames per second are at an unreasonable level of the Target FPS.")]
    public Color badColor = Color.red;

    protected const float updateInterval = 0.5f;
    protected int framesCount;
    protected float framesTime;

    protected Canvas canvas;
    protected Text displayText;

    private SteamVR_PlayArea playArea;
    private SteamVR_Camera headCamera;

    protected virtual void OnEnable()
    {
        InitCanvas();
    }

    protected virtual void Update()
    {
        // Press 'F' key to toggle show / hide
        if (Input.GetKeyDown(KeyCode.F))
        {
            displayFPS = !displayFPS;
        }

        framesCount++;
        framesTime += Time.unscaledDeltaTime;

        if (framesTime > updateInterval)
        {
            if (displayText != null)
            {
                if (displayFPS)
                {
                    float fps = framesCount / framesTime;
                    displayText.text = string.Format("{0:F2} FPS", fps);
                    displayText.color = (fps > (targetFPS - 5) ? goodColor :
                                        (fps > (targetFPS - 30) ? warnColor :
                                         badColor));
                }
                else
                {
                    displayText.text = "";
                }
            }
            framesCount = 0;
            framesTime = 0;
        }    

    }

    protected virtual void InitCanvas()
    {
        canvas = transform.GetComponentInParent<Canvas>();
        displayText = GetComponent<Text>();

        if (canvas != null)
        {
            canvas.planeDistance = 0.5f;
        }

        if (displayText != null)
        {
            displayText.fontSize = fontSize;
            displayText.transform.localPosition = position;
        }

        canvas.worldCamera = GameObject.Find("Camera (eye)").GetComponent<Camera>();
    }

}
