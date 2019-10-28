using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{

    public class ReturnToDivergenceControls : MonoBehaviour
    {
        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        ScriptableBoolean isDiverging;

        [SerializeField]
        ScriptableAction playerReachedTheEnd;

        [SerializeField]
        ScriptableBoolean returnToDivergencePointAnswer;

        [SerializeField]
        GameObject confirmatioPanel;

        [SerializeField]
        GameObject returnToDivergenceButton;


        void Start()
        {
            controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
            isDiverging.valueChangedEvent += OnIsDivergingValueChanged;
            playerReachedTheEnd.action += OnPlayerReachedTheEnd;
            returnToDivergencePointAnswer.valueChangedEvent += OnReturnToDivergencePointAnswer;
            Client.Instance.clientStateChangedEvent += OnClientStateChanged;
        }


        void OnClientStateChanged()
        {
            switch (Client.Instance.State)
            {
                case ClientGameState.Playing:
                case ClientGameState.PlayingTutorial:
                    transform.GetChild(0).gameObject.SetActive(true);
                    break;

                default:
                    transform.GetChild(0).gameObject.SetActive(false);
                    break;

            }
        }


        void OnControlsEnableValueChanged()
        {
            if (!controls.IsControlsEnabled)
            {
                confirmatioPanel.SetActive(false);
                returnToDivergenceButton.SetActive(false);
            }
        }


        void OnIsDivergingValueChanged()
        {
            if (isDiverging.Value)
            {
                returnToDivergenceButton.SetActive(true);
            }
            else
            {
                confirmatioPanel.SetActive(false);
                returnToDivergenceButton.SetActive(false);
            }
        }

        void OnPlayerReachedTheEnd()
        {
            confirmatioPanel.SetActive(false);
            returnToDivergenceButton.SetActive(false);
        }

        void OnReturnToDivergencePointAnswer()
        {
            if (!returnToDivergencePointAnswer.Value)
            {
                if (controls.IsControlsEnabled && controls.IsPlayerControlsEnabled)
                {
                    OnIsDivergingValueChanged();
                }
            }
        }
    }
}
