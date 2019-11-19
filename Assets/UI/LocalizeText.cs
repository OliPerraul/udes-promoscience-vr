using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{

    public class LocalizeText : MonoBehaviour
    {
        [SerializeField]
        LocalizeStringAsset localizeString;

        [SerializeField]
        LocalizeInlineString inlineLocalizeString = new LocalizeInlineString("[?]");

        [SerializeField]
        Text text;

        void OnEnable()
        {
            if (text != null)
                text.text = localizeString == null ? inlineLocalizeString.Value : localizeString.Value;

        }

    }
}
