using System;
using System.Collections;
using UnityEngine;

namespace UdeS.Promoscience
{
    [Serializable]
    public class LocalizeInlineString
    {
        [SerializeField]
        private LocalizeStringAsset localizeString;

        [SerializeField]
        private string defaultString = "[?]";

        public string Value => localizeString == null ? defaultString : localizeString.Value;

        public LocalizeInlineString(string value)
        {
            defaultString = value;
        }
    }


    [Serializable]
    public class LocalizeString
    {
        [SerializeField]
        private LocalizeStringAsset localizeString;

        [SerializeField]
        [TextArea]
        private string defaultString = "adasdasdasdasdasdasdasdasdasdasd\nadasdasdasdasdasdasdasdasdasdasd\nadasdasdasdasdasdasdasdasdasdasd\n";

        public string Value
        {
            get
            {
                if (localizeString == null)
                    return defaultString;

                else return localizeString.Value;
            }
        }
    }


    //[Sysm.Serializable]
    //public class LocalizeText
    //{
    //    [SerializeField]
    //    private ScriptableLocalizeString localizeString;

    //    [SerializeField]
    //    private string defaultString = "[?]";

    //    public string Value
    //    {
    //        get
    //        {
    //            if (localizeString == null)
    //                return defaultString;

    //            else return localizeString.Value;
    //        }
    //    }
    //}

    [CreateAssetMenu(fileName = "Data", menuName = "Data/LocalizeString", order = 1)]
    public class LocalizeStringAsset : ScriptableObject
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

