using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class MyUtilities
{
    public static string GetOriginNameFromPath(string path, char indicatorChar)
    {
        if (string.IsNullOrEmpty(path))
            return path;

        int lastIndex = path.LastIndexOf(indicatorChar);
        if (lastIndex == -1 || (lastIndex + 1) >= path.Length)
            return path;

        return path.Substring(lastIndex + 1);
    }

    public static string GetUpperCaseFirtCharInString(string originStr)
    {
        string makeUpDes = originStr;
        makeUpDes = makeUpDes.ToLower();
        makeUpDes = char.ToUpper(makeUpDes[0]) + makeUpDes.Substring(1);

        return makeUpDes;
    }
}
