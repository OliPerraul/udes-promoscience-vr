using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Controls;
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
            algorithmRespect.IsDiverging.OnValueChangedHandler += OnIsDivergingValueChanged;
            algorithmRespect.OnReturnToDivergencePointRequestHandler += OnReturnToDivergenceRequest;

            controls.OnPlayerReachedTheEndHandler += OnPlayerReachedTheEnd;
            controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;

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
                    OnIsDivergingValueChanged(algorithmRespect.IsDiverging.Value);
                    confirmatioPanel.SetActive(false);
                    break;

                default:
                    OnIsDivergingValueChanged(false);
                    confirmatioPanel.SetActive(false);
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

        void OnReturnToDivergenceRequest()
        {
            if (controls.IsControlsEnabled.Value && controls.IsPlayerControlsEnabled.Value)
            {
                OnIsDivergingValueChanged(false);
            }            
        }
    }
}
