using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MyUtilities
{
    #region ----- Static Methods -----

    public static T Random<T>(List<T> list)
    {
        if (list.Count == 0)
            return default;

        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        return list[randomIndex];
    }

    public static T Random<T>(params T[] arr)
    {
        if (arr.Length == 0)
            return default;

        int randomIndex = UnityEngine.Random.Range(0, arr.Length);
        return arr[randomIndex];
    }

    public static T RandomWithRestriction<T>(T restricted, params T[] arr)
    {
        if (arr.Length == 0)
            return default;

        bool valid = false;
        int loopBreakThreshold = 1000;
        int curLoopCounter = 0;

        while (!valid)
        {
            if (curLoopCounter >= loopBreakThreshold)
                break;

            curLoopCounter++;
            int randomIndex = UnityEngine.Random.Range(0, arr.Length);

            if (restricted.ToString() != arr[randomIndex].ToString())
            {
                valid = true;
                return arr[randomIndex];
            }
        }
        return default;
    }

    //HTri: Don't use this method to work with color type.
    [Obsolete("This method won't work with Color type because of float point precision problem!")]
    public static T RandomWithRestriction<T>(List<T> commonList, List<T> restrictedList)
    {
        List<T> tempList = new List<T>(commonList);

        for (int i = 0; i < restrictedList.Count; i++)
        {
            T item = restrictedList[i];
            tempList.Remove(item);
        }

        return Random<T>(tempList);
    }
 
    public static Color RandomColorWithRestriction(List<Color> commonList, List<Color> restrictedList)
    {
        List<Color> tempList = new List<Color>(commonList);

        for (int i = tempList.Count - 1; i >= 0; i--)
        {
            Color first = tempList[i];

            for (int k = 0; k < restrictedList.Count; k++)
            {
                Color second = restrictedList[k];

                if (MyUtilities.IsEqualColor(first, second))
                {
                    tempList.Remove(first);
                    break;
                }
            }
        }

        return Random<Color>(tempList);
    }

    #endregion
}
