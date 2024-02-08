using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class MyUtilities
{
    static Color _cachedColor = Color.white;
    
    public static Color ConvertNameToColor(string colorName)
    {
        colorName = colorName.ToLower();
        switch (colorName)
        {
            case "red":
                return GetNewColor(1f, 0f, 0f);
            case "orange":
                return GetNewColor(1f, 165f/255f, 0f);
            case "yellow":
                return GetNewColor(1f, 1f, 0f);
            case "lime":
                return GetNewColor(0f, 1f, 0f);
            case "green":
                return GetNewColor(0f, 0.5f, 0f);
            case "blue":
                return GetNewColor(0f, 0f);
            case "purple":
                return GetNewColor(0.5f, 0f, 0.5f);
            case "pink":
                return GetNewColor(1f, 192f/255f, 203f/255f);
            default:
                return GetNewColor();        
        }
    }

    public static Color GetNewColor(float r = 1f, float g = 1f, float b = 1f, float a = 1f)
    {
        _cachedColor.r = r;
        _cachedColor.g = g;
        _cachedColor.b = b;
        _cachedColor.a = a;
        return _cachedColor;
    }
}
