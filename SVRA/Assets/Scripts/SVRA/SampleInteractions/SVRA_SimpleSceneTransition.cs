using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_SimpleSceneTransition : MonoBehaviour {

    [Tooltip("The name of the scene to transition to (NOTE: Scene must be included in the build settings scenes to build")]
    public string levelName;

    [Tooltip("The fade out time of the scene")]
    public float fadeOutTime = 0.5f;

    [Tooltip("The color background during the scene transition")]
    public Color backgroundRGBAColor;

    [Tooltip("Display the grid in the transition zone")]
    public bool showGrid = false;

    public void TransitionLevel()
    {
        SteamVR_LoadLevel.Begin(levelName, showGrid, fadeOutTime, backgroundRGBAColor.r, backgroundRGBAColor.g, backgroundRGBAColor.b, backgroundRGBAColor.a);
    }
}
