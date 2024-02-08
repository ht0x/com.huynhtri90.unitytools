using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MyUtilities
{
    #region ----- Static Methods -----

    public static int Module(int a, int b)
    {
        return ((a % b) + b) % b;
    }

    public static float Module(float a, float b)
    {
        return Mathf.Repeat(a, b);
    }

    public static Vector2 MinVector2(Vector2 a, Vector2 b)
    {
        a.x = Mathf.Min(a.x, b.x);
        a.y = Mathf.Min(a.y, b.y);

        return a;
    }

    public static bool IsPowerOfTwo(int value)
    {
        return Mathf.IsPowerOfTwo(value);
    }

    public static bool IsPowerOfTwo(Texture2D tex2D)
    {
        int texWidth = tex2D.width;
        int texHeight = tex2D.height;

        return (texWidth == texHeight && Mathf.IsPowerOfTwo(texWidth) && Mathf.IsPowerOfTwo(texHeight)); 
    }

    public static bool IsMultipleOfFour(int value)
    {
        return value % 4 == 0;
    }

    public static bool IsMultipleOfFour(Texture2D tex2D)
    {
        int texWidth = tex2D.width;
        int texHeight = tex2D.height;

        return texWidth % 4 == 0 && texHeight % 4 == 0;
    }

    public static int GetIndexOfMinValue(params float[] values)
    {
        float minValue = float.MaxValue;
        int minIndex = 0;
        for (var i = 0; i < values.Length; i++)
        {
            float curValue = values[i];
            if (curValue <= minValue)
            {
                minValue = curValue;
                minIndex = i;
            }
        }

        return minIndex;
    }
    
    public static float GetMinValueOfCollection(params float[] values)
    {
        float minValue = float.MaxValue;
        if (values == null)
            return minValue;
        
        for (var i = 0; i < values.Length; i++)
        {
            float curValue = values[i];
            if (curValue <= minValue)
                minValue = curValue;
        }

        return minValue;
    }

    public static bool DoOverlap(Vector4 first, Vector4 second)
    {
        Vector2 firstTopLeftCorner = Vector2.zero;
        Vector2 firstBottomRightCorner = Vector2.zero;
        Vector2 secondTopLeftCorner = Vector2.zero;
        Vector2 secondBottomRightCorner = Vector2.zero;

        firstTopLeftCorner.x = first.x;
        firstTopLeftCorner.y = first.w;
        firstBottomRightCorner.x = first.y;
        firstBottomRightCorner.y = first.z;
        
        secondTopLeftCorner.x = second.x;
        secondTopLeftCorner.y = second.w;
        secondBottomRightCorner.x = second.y;
        secondBottomRightCorner.y = second.z;
        
        if (firstTopLeftCorner.x > secondBottomRightCorner.x || secondTopLeftCorner.x > firstBottomRightCorner.x)
            return false;

        if (firstBottomRightCorner.y > secondTopLeftCorner.y || secondBottomRightCorner.y > firstTopLeftCorner.y)
            return false;

        return true;
    }

    #endregion
}
