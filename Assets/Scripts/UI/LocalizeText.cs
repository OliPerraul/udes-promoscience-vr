using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Game;

namespace UdeS.Promoscience.UI
{

    public class LocalizeText : MonoBehaviour
    {
        [SerializeField]
        ScriptableLocalizeString localizeString;

        [SerializeField]
        Text text;

        void Start()
        {
            text.text = localizeString.Value;
        }

    }
}
