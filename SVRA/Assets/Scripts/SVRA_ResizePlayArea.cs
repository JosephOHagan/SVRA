using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_ResizePlayArea : MonoBehaviour {

    [SerializeField]
    public Vector3 shrinkScale = new Vector3(0.1f, 0.1f, 0.1f);

    private SteamVR_PlayArea playArea;

    private void Awake()
    {
        playArea = GetComponentInParent<SteamVR_PlayArea>();
        GetComponent<SteamVR_TrackedController>().Gripped += ToggleScale;
    }

    private void ToggleScale(object sender, ClickedEventArgs e)
    {
        if (playArea.transform.localScale == Vector3.one)
        {
            playArea.transform.localScale = shrinkScale;
        }
        else
        {
            playArea.transform.localScale = Vector3.one;
        }
    }
}
