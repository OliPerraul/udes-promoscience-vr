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
        ScriptableAlgorithm algorithm;

        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        ScriptableLocalizeString algorithmString;

        [SerializeField]
        ScriptableLocalizeString longestStraightString;

        [SerializeField]
        ScriptableLocalizeString rightHandString;

        [SerializeField]
        ScriptableLocalizeString shortestFlightString;

        [SerializeField]
        ScriptableLocalizeString standardAlgorithmString;

        [SerializeField]
        GameObject display;

        [SerializeField]
        Text text;

        void OnEnable()
        {
            algorithm.valueChangedEvent += OnAlgorithmValueChanged;
            controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
            controls.isPlayerControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
        }

        void OnAlgorithmValueChanged()
        {
            string s = algorithmString.Value + " : ";

            if (algorithm.Value == Utils.Algorithm.LongestStraight)
            {
                s += longestStraightString.Value;
            }
            else if (algorithm.Value == Utils.Algorithm.RightHand)
            {
                s += rightHandString.Value;
            }
            else if (algorithm.Value == Utils.Algorithm.ShortestFlightDistance)
            {
                s += shortestFlightString.Value;
            }
            else if (algorithm.Value == Utils.Algorithm.Standard)
            {
                s += standardAlgorithmString.Value;
            }

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
