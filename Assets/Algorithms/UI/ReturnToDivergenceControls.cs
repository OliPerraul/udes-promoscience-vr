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
        private AvatarControllerAsset controls;

        [SerializeField]
        private Algorithms.AlgorithmRespectAsset algorithmRespect;

        [SerializeField]
        private GameObject confirmatioPanel;

        [SerializeField]
        private GameObject returnToDivergenceButton;


        void Start()
        {
            controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
            algorithmRespect.IsDiverging.OnValueChangedHandler += OnIsDivergingValueChanged;
            controls.OnPlayerReachedTheEndHandler += OnPlayerReachedTheEnd;
            controls.ReturnToDivergencePointAnswer.OnValueChangedHandler += OnReturnToDivergencePointAnswer;
            Client.Instance.clientStateChangedEvent += OnClientStateChanged;

            OnClientStateChanged();
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
            if (!controls.IsControlsEnabled.Value)
            {
                confirmatioPanel.SetActive(false);
                returnToDivergenceButton.SetActive(false);
            }
        }


        void OnIsDivergingValueChanged(bool isdiverg)
        {
            if (isdiverg)
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

        void OnReturnToDivergencePointAnswer(bool answer)
        {
            if (!answer)
            {
                if (controls.IsControlsEnabled.Value && controls.IsPlayerControlsEnabled.Value)
                {
                    OnIsDivergingValueChanged(answer);
                }
            }
        }
    }
}
