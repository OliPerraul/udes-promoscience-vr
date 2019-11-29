using System;
using System.Collections;
using UnityEngine;

namespace UdeS.Promoscience
{
    [Serializable]
    public class LocalizeInlineString
    {
        [SerializeField]
        private string french = "[?]";

        [SerializeField]
        private string english = "[?]";

        public string Value => 
            Application.systemLanguage == SystemLanguage.French  ? 
                french == "[?]" || french == "" ? 
                    english : french : english;

        public LocalizeInlineString(string english)
        {
            this.english = english;
        }

        public LocalizeInlineString(string english, string french)
        {
            this.english = english;
            this.french = french;
        }
    }


    [Serializable]
    public class LocalizeString
    {
        [SerializeField]
        [TextArea]
        [UnityEngine.Serialization.FormerlySerializedAs("defaultString")]
        private string english = "adasdasdasdasdasdasdasdasdasdasd\nadasdasdasdasdasdasdasdasdasdasd\nadasdasdasdasdasdasdasdasdasdasd\n";

        [SerializeField]
        [TextArea]
        private string french = "[?]";

        public string Value =>
            Application.systemLanguage == SystemLanguage.French ?
                french == "[?]" || french == "" ?
                    english : french : english;

        public LocalizeString(string english)
        {
            this.english = english;
        }

        public LocalizeString(string english, string french)
        {
            this.english = english;
            this.french = french;
        }

    }
}

