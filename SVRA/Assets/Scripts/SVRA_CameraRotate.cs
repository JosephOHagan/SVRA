using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_CameraRotate : MonoBehaviour
{   
    private SteamVR_TrackedController controller;
    private SteamVR_PlayArea playArea;
    private SteamVR_Camera headCamera;

    private void Awake()
    {
        controller = GetComponent<SteamVR_TrackedController>();
        playArea = GetComponentInParent<SteamVR_PlayArea>();
        headCamera = playArea.GetComponentInChildren<SteamVR_Camera>();
    }

    private void Update()
    {
        bool move = controller.padPressed;
        if (move)
        {
            //            Vector3 forwardFlat = new Vector3(headCamera.transform.forward.x, 0f, headCamera.transform.forward.z);
            //            playArea.transform.position += forwardFlat * moveSpeed * Time.deltaTime;

            // headCamera.origin.transform.Rotate(0, 90, 0);

            // playArea.transform.Rotate(0, 90, 0);            
        }
    }
}