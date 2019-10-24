using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace UdeS.Promoscience.UI
{
    public static class Utils
    {
        public static bool IsUIElementActive()
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                return true;                
            }

            return false;
        }
    }
}
