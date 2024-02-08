using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CanvasExtension 
{
    public static Canvas GetTopmostCanvas(this Component component)
    {
        Canvas[] parentCanvases = component.GetComponentsInParent<Canvas>();
        if (parentCanvases != null && parentCanvases.Length > 0)
        {
            return parentCanvases[parentCanvases.Length - 1];
        }
        return null;
    }
}
