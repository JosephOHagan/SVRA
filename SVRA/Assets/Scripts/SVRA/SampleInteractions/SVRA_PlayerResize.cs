using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_PlayerResize : MonoBehaviour {

    private Vector3 originalScale;
    private SteamVR_PlayArea playArea;

    [Tooltip("Should the player start default or resized")]
    public bool sizeToggle = false;

    [Tooltip("Vector size of the world to rescale to")]
    public Vector3 toggleScale = new Vector3(0.5f, 0.5f, 0.5f);

    private void Awake()
    {
        playArea = GetComponentInParent<SteamVR_PlayArea>();
        originalScale = playArea.transform.localScale;
    }

    void Start () {       
        if (sizeToggle)
        {
            ResizePlayArea();
        }

        sizeToggle = !sizeToggle;
    }
	
	/* Resize the play area to the specified value */
	public void ResizePlayArea () {
        playArea.transform.localScale = sizeToggle ? toggleScale : originalScale;

    }
    
}
