using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRH_ControllerSetup : SteamVR_TrackedController {

    // Use this for initialization
    void OnEnable()
    {
        SteamVR_TrackedController controller = GetComponent<SteamVR_TrackedController>();
        controller.TriggerClicked += OnClickTrigger;
        controller.TriggerUnclicked += OnUnclickTrigger;
        controller.PadClicked += OnPadClicked;
    }

    void OnDisable()
    {
        SteamVR_TrackedController controller = GetComponent<SteamVR_TrackedController>();
        controller.TriggerClicked -= OnClickTrigger;
        controller.TriggerUnclicked -= OnUnclickTrigger;
        controller.PadClicked -= OnPadClicked;
    }

    void OnPadClicked(object sender, ClickedEventArgs e)
    {
        Debug.Log("Pad Clicked! X: " + e.padX + " " + e.padY);
    }

    void OnUnclickTrigger(object sender, ClickedEventArgs e)
    {
        Debug.Log("Unclicked trigger!");
    }

    void OnClickTrigger(object sender, ClickedEventArgs e)
    {
        Debug.Log("Clicked trigger!");
    }
}