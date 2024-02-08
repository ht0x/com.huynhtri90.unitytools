using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MyUtilities
{
    #region ----- Static Methods -----

    public static bool IsEqualFloat(float a, float b, float epsilon = 0.001f)
    {
        if (a - epsilon <= b && a + epsilon >= b)
            return true;

        return false;
    }

    public static bool IsGreaterOrEqualFloat(float a, float b, float epsilon = 0.01f)
    {
        if (a - epsilon >= b)
            return true;

        return false;
    }

    public static bool IsGreaterFloat(float a, float b, float epsilon = 0.01f)
    {
        if (a - epsilon > b)
            return true;

        return false;
    }

    public static bool IsLesserOrEqualFloat(float a, float b, float epsilon = 0.01f)
    {
        if (a + epsilon <= b)
            return true;

        return false;
    }

    public static bool IsLesserFloat(float a, float b, float epsilon = 0.01f)
    {
        if (a + epsilon < b)
            return true;

        return false;
    }

    public static bool IsEqualDouble(double a, double b, double epsilon = 0.001f)
    {
        if (a - epsilon <= b && a + epsilon >= b)
            return true;

        return false;
    }
    
    public static bool IsGreaterOrEqualDouble(double a, double b, double epsilon = 0.01f)
    {
        if (a - epsilon >= b)
            return true;

        return false;
    }

    public static bool IsGreaterDouble(double a, double b, double epsilon = 0.01f)
    {
        if (a - epsilon > b)
            return true;

        return false;
    }

    public static bool IsLesserOrEqualDouble(double a, double b, double epsilon = 0.01f)
    {
        if (a + epsilon <= b)
            return true;

        return false;
    }

    public static bool IsLesserDouble(double a, double b, double epsilon = 0.01f)
    {
        if (a + epsilon < b)
            return true;

        return false;
    }
    
    public static bool IsEqualColor(Color first, Color second, bool ignoreAlpha = false, float epsilon = 0.01f)
    {
        bool rgbResult = false;
        bool alphaResult = ignoreAlpha;

        if (IsEqualFloat(first.r, second.r, epsilon)
         && IsEqualFloat(first.g, second.g, epsilon)
         && IsEqualFloat(first.b, second.b, epsilon))
        {
            rgbResult = true;
        }

        alphaResult |= IsEqualFloat(first.a, second.a, epsilon);

        return rgbResult && alphaResult;
    }

    public static bool IsEqualVector2(Vector2 a, Vector2 b, float epsilon = 0.01f)
    {
        if (IsEqualFloat(a.x, b.x, epsilon) && IsEqualFloat(a.y, b.y, epsilon))
            return true;

        return false;
    }

    public static bool IsEqualVector3(Vector3 a, Vector3 b, float epsilon = 0.01f)
    {
        if (IsEqualFloat(a.x, b.x, epsilon) && IsEqualFloat(a.y, b.y, epsilon) && IsEqualFloat(a.z, b.z, epsilon))
            return true;

        return false;
    }

    #endregion
}
