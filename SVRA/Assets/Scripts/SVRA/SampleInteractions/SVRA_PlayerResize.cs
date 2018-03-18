using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_PlayerResize : MonoBehaviour {

    private Vector3 originalScale;
    private SteamVR_PlayArea playArea;

    [Tooltip("Should the player start default or modified height")]
    public bool startModified = false;

    [Tooltip("Vector size of the world to rescale to")]
    public Vector3 toggleScale = new Vector3(0.5f, 0.5f, 0.5f);

    private void Awake()
    {
        playArea = GetComponentInParent<SteamVR_PlayArea>();
        originalScale = playArea.transform.localScale;
    }
    
	/* Resize the play area to the specified value */
	public void ResizePlayer () {
        startModified = !startModified;
        playArea.transform.localScale = startModified ? toggleScale : originalScale;       
    }
    
}
