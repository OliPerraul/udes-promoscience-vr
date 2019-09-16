﻿using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{

    public class LocalizeText : MonoBehaviour
    {
        [SerializeField]
        ScriptableLocalizeString localizeString;

        [SerializeField]
        Text text;

        void OnEnable()
        {
            if(text != null  && localizeString != null)
                text.text = localizeString.Value;

        }

    }
}