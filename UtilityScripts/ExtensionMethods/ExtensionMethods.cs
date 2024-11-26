using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector4 Parse(this Vector4 v4, string data, CultureInfo cultureInfo = null)
    {
        if (data != null)
        {
            data = data.Replace("(", "").Replace(")", "");

            string[] temp = data.Split(',');

            float xPos = float.Parse(temp[0], cultureInfo ?? CultureInfo.InvariantCulture);
            float yPos = float.Parse(temp[1], cultureInfo ?? CultureInfo.InvariantCulture);
            float zPos = float.Parse(temp[2], cultureInfo ?? CultureInfo.InvariantCulture);
            float tPos = float.Parse(temp[3], cultureInfo ?? CultureInfo.InvariantCulture);

            return new Vector4(xPos, yPos, zPos, tPos);
        }

        return Vector4.zero;
    }

    public static Vector3 Parse(this Vector3 v3, string data, CultureInfo cultureInfo = null)
    {
        if (data != null)
        {
            data = data.Replace("(", "").Replace(")", "");

            string[] temp = data.Split(',');

            float xPos = float.Parse(temp[0], cultureInfo ?? CultureInfo.InvariantCulture);
            float yPos = float.Parse(temp[1], cultureInfo ?? CultureInfo.InvariantCulture);
            float zPos = float.Parse(temp[2], cultureInfo ?? CultureInfo.InvariantCulture);

            return new Vector3(xPos, yPos, zPos);
        }

        return default;
    }
    
    public static bool TryParse(this string data, out Vector3 v3, CultureInfo cultureInfo = null)
    {
        v3 = default;

        if (data != null)
        {
            data = data.Replace("(", "").Replace(")", "");

            string[] temp = data.Split(',');

            float xPos = float.Parse(temp[0], cultureInfo ?? CultureInfo.InvariantCulture);
            float yPos = float.Parse(temp[1], cultureInfo ?? CultureInfo.InvariantCulture);
            float zPos = float.Parse(temp[2], cultureInfo ?? CultureInfo.InvariantCulture);

            v3 = new Vector3(xPos, yPos, zPos);
            return true;
        }

        return false;
    }

    public static void SetTransform(this Transform trans, Vector3 pos, Vector3 rot, Vector3 scale, Transform parent = null, bool isLocal = false)
    {
        if (!isLocal)
        {
            trans.position = pos;
            trans.rotation = Quaternion.Euler(rot);
        }
        else
        {
            trans.localPosition = pos;
            trans.localRotation = Quaternion.Euler(rot);
        }

        trans.localScale = scale;

        if (parent)
            trans.parent = parent;
    }

    public static void RemoveCloneName(this GameObject go)
    {
        RemoveCloneName(go.transform);
    }
    
    public static void RemoveCloneName(this Transform trans)
    {
        trans.name = trans.name.Replace("(Clone)", string.Empty);
    }

    public static Vector2 Parse(this Vector2 v2, string data, CultureInfo cultureInfo = null)
    {
        if (data != null)
        {
            data = data.Replace("(", "").Replace(")", "");

            string[] temp = data.Split(',');

            float xPos = float.Parse(temp[0], cultureInfo ?? CultureInfo.InvariantCulture);
            float yPos = float.Parse(temp[1], cultureInfo ?? CultureInfo.InvariantCulture);

            return new Vector2(xPos, yPos);
        }

        return Vector2.zero;
    }

    public static void RemoveComponent<Component>(this GameObject go, bool immediate = false)
    {
        var component = go.GetComponent<Component>();
        if (component == null) 
            return;
        
        if (immediate)
            Object.DestroyImmediate(component as Object);
        else
            Object.Destroy(component as Object);
    }

    public static Vector3 AbsVector3(this Mathf mathf, Vector3 v3)
    {
        v3.x = Mathf.Abs(v3.x);
        v3.y = Mathf.Abs(v3.y);
        v3.z = Mathf.Abs(v3.z);

        return v3;
    }

    public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera)
    {
        return CountCornersVisibleFrom(rectTransform, camera) == 4;
    }
   
    public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera)
    {
        return CountCornersVisibleFrom(rectTransform, camera) > 0; 
    }

    private static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera)
    {
        Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height); 
        Vector3[] objectCorners = new Vector3[4];

        if (rectTransform == null)
            return 0;

        rectTransform.GetWorldCorners(objectCorners);

        int visibleCorners = 0;
        Vector3 tempScreenSpaceCorner;

        if (objectCorners == null)
            return 0;

        for (var i = 0; i < objectCorners.Length; i++) 
        {
            tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]);

            if (screenBounds.Contains(tempScreenSpaceCorner)) 
                visibleCorners++;
        }

        return visibleCorners;
    }   

    public static string GetListToString<T>(this List<T> list)
    {
        if (list == null)
            return string.Empty;
        
        StringBuilder strBuilder = new StringBuilder();
        foreach (var t in list)
            strBuilder.AppendLine("Product ID: " + t.ToString());

        return strBuilder.ToString();
    }

    public static int ConvertLayerMaskToLayerNumber(this LayerMask mask)
    {
        var bitmask = mask.value;
        var result = bitmask > 0 ? 0 : 31;
        while (bitmask > 1)
        {
            bitmask = bitmask >> 1;
            result++;
        }

        return result;
    }

    public static void Resize<T>(this List<T> list, int sz, T c)
    {
        int cur = list.Count;
        if (sz < cur)
            list.RemoveRange(sz, cur - sz);
        else if (sz > cur)
        {
            if (sz > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
                list.Capacity = sz;
            list.AddRange(Enumerable.Repeat(c, sz - cur));
        }
    }
    
    public static void Resize<T>(this List<T> list, int sz) where T : new()
    {
        Resize(list, sz, new T());
    }
}
