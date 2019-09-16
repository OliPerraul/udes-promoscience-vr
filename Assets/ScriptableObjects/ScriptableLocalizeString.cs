using System;
using System.Collections;
using UnityEngine;

namespace UdeS.Promoscience.ScriptableObjects
{
    [System.Serializable]
    public class LocalizeString
    {
        [SerializeField]
        private ScriptableLocalizeString localizeString;

        [SerializeField]
        private string defaultString = "[?]";

        public string Value
        {
            get {
                if (localizeString == null)
                    return defaultString;

                else return localizeString.Value;
            }
        }
    }


    [CreateAssetMenu(fileName = "Data", menuName = "Data/LocalizeString", order = 1)]
    public class ScriptableLocalizeString : ScriptableObject
    {
        [SerializeField]
        string english;

        [SerializeField]
        string french;

        public string Value
        {
            get
            {
                if (Application.systemLanguage == SystemLanguage.French)
                {
                    return french;
                }
                else
                {
                    return english;
                }
            }
        }
    }
}

