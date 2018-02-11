using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_SceneTransition : MonoBehaviour {

    private SteamVR_LoadLevel SteamVR_LoadLevel;

    public string levelName;

    public void TransitionLevel()
    {
        SteamVR_LoadLevel.Begin(levelName);
    }

}
