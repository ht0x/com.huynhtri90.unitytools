using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class MyUtilities
{
    public static Vector3 ConvertScreenDeltaToWorldDelta(Vector2 screenDelta, Camera orthoCamera)
    {
        if (!orthoCamera.orthographic) return default;
        var wordPoint = orthoCamera.ScreenToWorldPoint(new Vector3(screenDelta.x, screenDelta.y, orthoCamera.nearClipPlane));
        var anchoredWorldPoint = orthoCamera.ScreenToWorldPoint(new Vector3(0, 0, orthoCamera.nearClipPlane));
        var delta = wordPoint - anchoredWorldPoint;
        delta.y = 0;
        return delta;
    }
}
