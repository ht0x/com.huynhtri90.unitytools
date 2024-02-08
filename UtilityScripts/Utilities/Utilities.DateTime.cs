using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

public partial class MyUtilities
{
    #region ----- Variables -----     
    
    #endregion
    
    #region ----- Default Datetime Format -----
    
    public static DateTime GetDateTimeNow()
    {
        return DateTime.Now;
    }
    
    public static DateTime GetDateTimeUtcNow()
    {
        return DateTime.UtcNow;
    }
    
    public static DateTime GetLocalDateTimeNow()
    {
        return DateTime.Now.ToLocalTime();
    }
    
    public static DateTime GetLocalDateTimeUtcNow()
    {
        return DateTime.UtcNow.ToLocalTime();
    }
    
    #endregion

    #region ----- String Datetime Format -----

    public static string GetDateTimeString(DateTime dt, CultureInfo cultureInfo)
    {
        return dt.ToString(cultureInfo);
    }

    public static string GetDateTimeNowString(CultureInfo cultureInfo)
    {
        return DateTime.Now.ToString(cultureInfo);
    }
    
    public static string GetDateTimeUtcNowString(CultureInfo cultureInfo)
    {
        return DateTime.UtcNow.ToString(cultureInfo);
    }

    public static string GetLocalDateTimeNowString(CultureInfo cultureInfo)
    {
        return DateTime.Now.ToLocalTime().ToString(cultureInfo);
    }

    public static string GetLocalDateTimeUtcNowString(CultureInfo cultureInfo)
    {
        return DateTime.UtcNow.ToLocalTime().ToString(cultureInfo);
    }

    public static string Get_Local_DateTimeNow_File_Format_String(CultureInfo cultureInfo)
    {
        DateTime localTime = DateTime.Now.ToLocalTime();
        string day = localTime.Day.ToString();
        string month = localTime.Month.ToString();
        string year = localTime.Year.ToString();
        string hour = localTime.Hour.ToString();
        string minute = localTime.Minute.ToString();
        
        return string.Format("{0}h{1}p_{2}-{3}-{4}", hour, minute, day, month, year);
    }
    
    #endregion
}
