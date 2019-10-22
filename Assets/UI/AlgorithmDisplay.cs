using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.UI
{
    public class AlgorithmDisplay : MonoBehaviour
    {
        [SerializeField]
        ScriptableClientGameState client;

        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        GameObject display;

        [SerializeField]
        Text text;

        void OnEnable()
        {
            client.OnAlgorithmChangedHandler += OnAlgorithmValueChanged;
            controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
            controls.isPlayerControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
        }

        void OnAlgorithmValueChanged()
        {
            string s = "";
            s += client.Algorithm.Name;
            text.text = s;
        }

        void OnControlsEnableValueChanged()
        {
            if (controls.IsControlsEnabled && controls.IsPlayerControlsEnabled)
            {
                display.gameObject.SetActive(true);
                OnAlgorithmValueChanged();
            }
            else
            {
                display.gameObject.SetActive(false);
            }
        }
    }
}
