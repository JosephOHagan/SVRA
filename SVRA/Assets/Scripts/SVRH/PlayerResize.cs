using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResize : MonoBehaviour
{
    Valve.VR.InteractionSystem.Player player;

    [Tooltip("The default height which to set the player")]
    public float defaultHeight = 1.71f;   

    private void Resize()
    {
        // Get the player instance
        player = Valve.VR.InteractionSystem.Player.instance;
        if (!player)
        {
            return;
        }

        // SteamVR_Camera camera = SteamVR_Render.Top();

        Debug.Log(player.hmdTransform.localPosition.y);

        float headHeight = player.hmdTransform.localPosition.y;
        float scale = defaultHeight / headHeight;

        // Debug.Log(headHeight);
        // Debug.Log(scale);

        // float eyeHeight = Valve.VR.InteractionSystem.Player.instance.eyeHeight;

        // camera.head.transform.localScale = Vector3.one * scale;

        player.transform.localScale = Vector3.one * scale;
    }

    void OnEnable()
    {
        Resize();
    }
}