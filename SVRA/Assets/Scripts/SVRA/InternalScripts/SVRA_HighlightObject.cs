using UnityEngine;
using System.Collections.Generic;
using SVRA.TypeReferences;
using System.Reflection;

public interface SVRA_HighlightEffect
{
    void Start(GameObject gameObject);
    void Stop(GameObject gameObject);
}

public class SVRA_HighlightObject : MonoBehaviour
{    
    private SVRA_HighlightEffect effect = null;
    private bool highlighted = false;
    private HashSet<SVRA_GrabPoint> grabSet = new HashSet<SVRA_GrabPoint>();

    void Update()
    {
        if (highlighted && grabSet.Count == 0)
        {
            RemoveHighlight();
        }

        if (!highlighted && grabSet.Count != 0)
        {
            AddHighlight();
        }
    }

    public void RemoveHighlight()
    {
        if (effect == null)
        {
            return;
        }

        effect.Stop(gameObject);
        highlighted = false;
    }

    public void AddHighlight()
    {
        if (effect == null)
        {
            return;
        }

        effect.Start(gameObject);
        highlighted = true;
    }

    public SVRA_HighlightEffect UpdateEffect(System.Type effectType)
    {
        if (effectType == null || typeof(SVRA_HighlightEffect).IsAssignableFrom(effectType))
        {
            if (effect != null)
            {
                effect.Stop(gameObject);
            }

            AssignEffect(effectType);
        }
        else
        {
            Debug.LogError(effectType + " does not implement the SVRA_HighlightEffect interface");
        }
        return effect;
    }

    public SVRA_HighlightEffect CurrentEffect()
    {
        return effect;
    }

    void AssignEffect(System.Type effectType)
    {
        if (effectType == null)
        {
            effect = null;
        }
        else
        {
            effect = System.Activator.CreateInstance(effectType) as SVRA_HighlightEffect;
        }

        foreach (SVRA_InteractiveObject obj in GetComponents<SVRA_InteractiveObject>())
        {
            obj.highlightEffect = effectType;
        }
    }

    void SVRAHighlightStart(SVRA_GrabPoint grabPoint)
    {
        if (!this.enabled)
        {
            return;
        }

        grabSet.Add(grabPoint);
    }

    void SVRAHighlightStop(SVRA_GrabPoint grabPoint)
    {
        if (!this.enabled)
        {
            return;
        }

        grabSet.Remove(grabPoint);
    }

    void OnDisable()
    {
        RemoveHighlight();
    }
}
