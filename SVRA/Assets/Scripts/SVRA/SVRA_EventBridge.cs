using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SVRA_EventBridge : MonoBehaviour {

    public enum SVRAEvent
    {
        InteractionStart,
        InteractionStop,
        GrabStart,
        GrabStop,
        HighlightStart,
        HighlightStop,
        TouchStart,
        TouchStop,
    }

    public SVRAEvent SVRA_Event;
    public UnityEvent attachedFunction;

    void InvokeIf(SVRAEvent thisEvent)
    {
        if (attachedFunction != null && SVRA_Event == thisEvent)
        {
            attachedFunction.Invoke();
        }
    }

    void SVRAInteractionStart()
    {
        InvokeIf(SVRAEvent.InteractionStart);
    }

    void SVRAInteractionStop()
    {
        InvokeIf(SVRAEvent.InteractionStop);
    }

    void SVRAGrabStart()
    {
        InvokeIf(SVRAEvent.GrabStart);
    }

    void SVRAGrabStop()
    {
        InvokeIf(SVRAEvent.GrabStop);
    }

    void SVRAHighlightStart()
    {
        InvokeIf(SVRAEvent.HighlightStart);
    }

    void SVRAHighlightStop()
    {
        InvokeIf(SVRAEvent.HighlightStop);
    }

    void SVRATouchStart()
    {
        InvokeIf(SVRAEvent.TouchStart);
    }

    void SVRATouchStop()
    {
        InvokeIf(SVRAEvent.TouchStop);
    }

}
