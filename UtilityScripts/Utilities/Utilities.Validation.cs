using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class MyUtilities
{
    private const string EMAIL_MATCH_PATTERN = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                               + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                               + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                               + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    public static bool IsValidEmail(string email)
    {
        return !string.IsNullOrEmpty(email) && Regex.IsMatch(email, EMAIL_MATCH_PATTERN);
    }

    //private const string PHONE_MATCH_PATTERN = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
    private const string PHONE_MATCH_PATTERN = @"^0?[0-9]{9,13}$";

    public static bool IsValidPhoneNumber(string number)
    {
        return !string.IsNullOrEmpty(number) && Regex.IsMatch(number, PHONE_MATCH_PATTERN);
    }

    public static bool IsValidPasswordLength(string password, int minLength = 6)
    {
        return !string.IsNullOrEmpty(password) && password.Length >= minLength;
    }

    public static readonly Dictionary<string, string> PHONE_AREA_CODES = new Dictionary<string, string>()
    {
        ["JAPAN +81"] = "+81",
        ["VIETNAM +84"] = "+84",
        ["S.KOREA +82"] = "+82",
        ["SINGAPORE +65"] = "+65",
    };
}
