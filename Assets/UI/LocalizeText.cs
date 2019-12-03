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
        Text text;

        public void Awake()
        {
            if (text != null)
                text.text = localizeString.Value;
        }

    }
}
