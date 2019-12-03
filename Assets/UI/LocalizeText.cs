using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{

    public class LocalizeText : MonoBehaviour
    {
        [SerializeField]
        public LocalizeInlineString localizeString = new LocalizeInlineString("[?]", "[?]");

        [SerializeField]
        public LocalizeString localizeStringBlock = new LocalizeString("[?]", "[?]");

        [SerializeField]
        Text text;

        public void Awake()
        {
            if (text != null)
            {
                if (!(localizeString.Value == "[?]" || localizeString.Value == ""))
                {
                    text.text = localizeString.Value;
                }
                else if (!(localizeStringBlock.Value == "[?]" || localizeStringBlock.Value == ""))
                {
                    text.text = localizeStringBlock.Value;
                }
            }
        }

    }
}
