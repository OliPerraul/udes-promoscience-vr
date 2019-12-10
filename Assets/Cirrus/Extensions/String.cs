using UnityEngine;
using System.Collections;


namespace Cirrus.Extensions
{

    public static class StringExtension
    {
        // Use this for initialization
        public static bool IsNullOrEmpty(this string val)
        {
            return string.IsNullOrEmpty(val);
        }
    }
}

