using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_SceneTransition : MonoBehaviour {

    public string levelName;

    public void TransitionLevel()
    {
        SteamVR_LoadLevel.Begin(levelName);
    }
}
