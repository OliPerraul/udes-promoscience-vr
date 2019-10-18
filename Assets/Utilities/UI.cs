using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace UdeS.Promoscience.Utils
{

    public static class UI
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
