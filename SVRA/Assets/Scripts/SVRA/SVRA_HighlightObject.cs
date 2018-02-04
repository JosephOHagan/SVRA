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


/*

public class SVRA_DefaultLightenTintEffect : SVRA_HighlightEffect
{
    private Color tintColor = new Color(0.2f, 0.2f, 0.2f, 0f);
    private Queue<Color> oldColors = new Queue<Color>();

    public void Start(GameObject gameObject)
    {
        Stop(gameObject);
        foreach (Material material in MaterialsFrom(RenderersIn(gameObject)))
        {
            PutColor(material);
        }
    }

    public void Stop(GameObject gameObject)
    {
        foreach (Material material in MaterialsFrom(RenderersIn(gameObject)))
        {
            if (oldColors.Count == 0) { break; }
            PopColor(material);
        }
        oldColors.Clear();
    }

    void PutColor(Material material)
    {
        oldColors.Enqueue(material.color);
        material.color = material.color + tintColor;
    }

    void PopColor(Material material)
    {
        material.color = oldColors.Dequeue();
    }

    public virtual Renderer[] RenderersIn(GameObject gameObject)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer == null)
        {
            return new Renderer[0];
        }

        return new Renderer[] { renderer };
    }

    public virtual Material[] MaterialsFrom(Renderer[] renderers)
    {
        if (renderers.Length == 0)
        {
            return new Material[0];
        }

        return renderers[0].materials;
    }
}

*/
