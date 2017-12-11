﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRH_TouchpadLocomotion : MonoBehaviour {
    public float _mMoveSpeed = 2.5f;
    public float _mHorizontalTurnSpeed = 180f;
    public float _mVerticalTurnSpeed = 2.5f;
    public bool _mInverted = false;
    public const float VERTICAL_LIMIT = 60f;

    private Valve.VR.InteractionSystem.Player player;
    private SteamVR_Controller.Device movementController;
    private Valve.VR.EVRButtonId touchPad;
    public bool leftHandMovement = true;

    // TODO: Option for player to toggle between which controller controls movement and which rotation
    // TODO: Override function of trackpad with this feature - market this as debugging feature

/*
    private void OnGUI()
    {
        Valve.VR.InteractionSystem.Player player = Valve.VR.InteractionSystem.Player.instance;
        if (!player)
        {
            return;
        }

        Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

        if (null != player.leftController)
        {
            var touchPadVector = player.leftController.GetAxis(touchPad);
            GUILayout.Label(string.Format("Left X: {0:F2}, {1:F2}", touchPadVector.x, touchPadVector.y));
        }

        if (null != player.rightController)
        {
            var touchPadVector = player.rightController.GetAxis(touchPad);
            GUILayout.Label(string.Format("Right X: {0:F2}, {1:F2}", touchPadVector.x, touchPadVector.y));
        }
    }
*/
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
        player = Valve.VR.InteractionSystem.Player.instance;
        if (!player)
        {
            return;
        }

        // Get the touchpad 
        touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

        if (leftHandMovement)
        {
            movementController = player.leftController;
        }
        else
        {
            movementController = player.rightController;
        }

        if (null != movementController)
        {
            Quaternion orientation = Camera.main.transform.rotation;
            var touchPadVector = movementController.GetAxis(touchPad);
            Vector3 moveDirection = orientation * Vector3.forward * touchPadVector.y + orientation * Vector3.right * touchPadVector.x;
            Vector3 pos = player.transform.position;
            pos.x += moveDirection.x * _mMoveSpeed * Time.deltaTime;
            pos.z += moveDirection.z * _mMoveSpeed * Time.deltaTime;
            player.transform.position = pos;
        }        
    }
}