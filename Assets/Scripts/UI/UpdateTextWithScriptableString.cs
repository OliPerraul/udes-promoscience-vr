using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.UI
{
    public class UpdateTextWithScriptableString : MonoBehaviour
    {
        [SerializeField]
        ScriptableString scriptableString;

        [SerializeField]
        Text text;

        void Start()
        {
            scriptableString.valueChangedEvent += OnValueChangedEvent;
        }

        void OnValueChangedEvent()
        {
            text.text = scriptableString.Value;
        }
    }
}
