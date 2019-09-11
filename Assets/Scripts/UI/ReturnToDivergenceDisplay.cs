using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{
    public class ReturnToDivergenceDisplay : MonoBehaviour
    {
        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        ScriptableBoolean isDiverging;

        [SerializeField]
        ScriptableBoolean returnToDivergencePointAnswer;

        [SerializeField]
        ScriptableAction returnToDivergencePointRequest;

        [SerializeField]
        GameObject confirmationPanel;


        void OnEnable()
        {
            controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
            returnToDivergencePointRequest.action += OnScriptableAction;
            isDiverging.valueChangedEvent += OnIsDivergingValueChanged;
        }

        void OnControlsEnableValueChanged()
        {
            if (!controls.IsControlsEnabled)
            {
                OnIsDivergingValueChanged();
            }
        }

        void OnScriptableAction()
        {
            if (isDiverging.Value)
            {
                controls.IsPlayerControlsEnabled = false;
                confirmationPanel.SetActive(true);
            }
            else
            {
                returnToDivergencePointAnswer.Value = false;
            }
        }

        void OnIsDivergingValueChanged()
        {
            if (confirmationPanel.activeSelf)
            {
                confirmationPanel.SetActive(false);
                controls.IsPlayerControlsEnabled = true;
                returnToDivergencePointAnswer.Value = false;
            }
        }
    }
}
