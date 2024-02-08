using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MyUtilities
{
    #region ----- Static Methods -----

    public static T[] ShuffleList<T>(T[] objList)
    {
        if (objList == null)
            return null;
        
        for (int i = 0; i < objList.Length; i++)
        {
            T tempPair = objList[i];
            int randomIndex = UnityEngine.Random.Range(i, objList.Length);
            objList[i] = objList[randomIndex];
            objList[randomIndex] = tempPair;
        }

        return objList;
    }

    public static List<T> ShuffleList<T>(List<T> objList)
    {
        if (objList == null)
            return null;
        
        for (int i = 0; i < objList.Count; i++)
        {
            T tempPair = objList[i];
            int randomIndex = UnityEngine.Random.Range(i, objList.Count);
            objList[i] = objList[randomIndex];
            objList[randomIndex] = tempPair;
        }

        return objList;
    }

    public static T[] RemoveCollectionDuplicate<T>(T[] collection)
    {
        if (collection == null || collection.Length == 0)
            return collection;

        List<T> newCollection = new List<T>();
        for (int i = 0; i < collection.Length; i++)
        {
            T obj = collection[i];
            if (!newCollection.Contains(obj))
                newCollection.Add(obj);
        }

        return newCollection.ToArray();
    }

    public static List<T> InitValue<T>(List<T> ls, T defaultValue)
    {
        if (ls == null)
            return null;
        
        for (var i = 0; i < ls.Count; i++) 
            ls[i] = defaultValue;

        return ls;
    }
    
    public static T[] InitValue<T>(T[] arr, T defaultValue)
    {
        if (arr == null)
            return null;
        
        for (var i = 0; i < arr.Length; i++) 
            arr[i] = defaultValue;

        return arr;
    }

    public static T[] ConvertToComponentType<T>(Component[] components) where T: Component
    {
        List<T> collections = new List<T>();
        for (var i = 0; i < components.Length; i++)
            if (components[i] is T castedComponent)
                collections.Add(castedComponent);

        return collections.ToArray();
    }

    #endregion
}
