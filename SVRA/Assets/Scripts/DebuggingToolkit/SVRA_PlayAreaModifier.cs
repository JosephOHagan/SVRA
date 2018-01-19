using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_PlayAreaModifier : MonoBehaviour
{   
    private SteamVR_PlayArea playArea;

    [SerializeField]
    public Vector3 shrinkScale = new Vector3(0.1f, 0.1f, 0.1f);

    private void Awake()
    {
        playArea = GetComponentInParent<SteamVR_PlayArea>();        
    }

    private void Update()
    {        
        // Press 'G' key to rotate play area +90 degrees
        if (Input.GetKeyDown(KeyCode.G))
        {
            playArea.transform.Rotate(0, 90, 0);            
        }

        // Press 'H' key to rotate play area -90 degrees
        if (Input.GetKeyDown(KeyCode.H))
        {
            playArea.transform.Rotate(0, -90, 0);
        }

        // Press 'J' key to change the play area scale
        if (Input.GetKeyDown(KeyCode.J))
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
}