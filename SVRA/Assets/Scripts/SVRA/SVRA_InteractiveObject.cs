using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVRA.TypeReferences;

public class SVRA_InteractiveObject : MonoBehaviour {
    [ClassImplements(typeof(SVRA_HighlightEffect))]
    public ClassTypeReference highlightEffect = typeof(SVRA_GeneralLightenHighlightTintEffect);
    private SVRA_HighlightObject highlighter;

    public void Awake()
    {
        highlighter = GetComponent<SVRA_HighlightObject>();

        if (highlighter == null)
        {
            highlighter = gameObject.AddComponent<SVRA_HighlightObject>();
        }

        highlighter.UpdateEffect(highlightEffect.Type);
    }

    void OnDisable()
    {
        if (highlighter == null) { return; }
        highlighter.RemoveHighlight();
    }

    void OnEnable()
    {
        if (highlighter == null) { return; }
        highlighter.AddHighlight();
    }

    void OnValidate()
    {
        if (highlighter == null) { return; }
        highlighter.UpdateEffect(highlightEffect.Type);
    }
}
