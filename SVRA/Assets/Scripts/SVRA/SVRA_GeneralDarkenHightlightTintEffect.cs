using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVRA_GeneralDarkenHightlightTintEffect : SVRA_HighlightEffect
{
    private Color tintColor = new Color(-0.2f, -0.2f, -0.2f, 0f);
    private Queue<Color> oldColors = new Queue<Color>();

    public void setTintColor(Color color)
    {
        this.tintColor = color;
    }

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
