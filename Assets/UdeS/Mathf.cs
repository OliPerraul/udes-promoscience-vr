using UnityEngine;
using System.Collections;

namespace UdeS
{

    public static class FloatExtension
    {

        // Use this for initialization
        public static bool Approximately(this float val, float other, float epsilon = 0.01f)
        {
            return UnityEngine.Mathf.Approximately(val, other);
        }
    }

}