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
        private UnityEngine.UI.Button returnToDivergenceButton;

        [SerializeField]
        private UnityEngine.UI.Button confirmButton;

        [SerializeField]
        private UnityEngine.UI.Button cancelButton;


        void Awake()
        {
            Client.Instance.State.OnValueChangedHandler += OnClientStateChanged;

            algorithmRespect.IsDiverging.OnValueChangedHandler += OnIsDivergingValueChanged;
            algorithmRespect.OnReturnToDivergencePointRequestHandler += OnReturnToDivergenceRequest;

            controls.OnPlayerReachedTheEndHandler += OnPlayerReachedTheEnd;
            controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;

            returnToDivergenceButton.onClick.AddListener(OnReturnToDivergenceClicked);
            cancelButton.onClick.AddListener(OnCancelClicked);
            confirmButton.onClick.AddListener(OnConfirmClicked);

            OnClientStateChanged(Client.Instance.State.Value);
        }

        public void OnReturnToDivergenceClicked()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public void OnCancelClicked()
        {
            gameObject.SetActive(false);
        }

        public void OnConfirmClicked()
        {
            gameObject.SetActive(false);
            algorithmRespect.InvokeReturnToDivergent();
        }


        void OnClientStateChanged(ClientGameState state)
        {
            switch (Client.Instance.State.Value)
            {
                case ClientGameState.Playing:
                case ClientGameState.PlayingTutorial:
                    OnIsDivergingValueChanged(algorithmRespect.IsDiverging.Value);
                    confirmatioPanel.SetActive(false);
                    break;

                default:
                    OnIsDivergingValueChanged(false);
                    confirmatioPanel.SetActive(false);
                    break;
            }
        }


        void OnControlsEnableValueChanged()
        {
            if (!controls.IsControlsEnabled.Value)
            {
                confirmatioPanel.SetActive(false);
                returnToDivergenceButton.gameObject.SetActive(false);
            }
        }


        void OnIsDivergingValueChanged(bool isdiverg)
        {
            if (isdiverg)
            {
                returnToDivergenceButton.gameObject.SetActive(true);
            }
            else
            {
                confirmatioPanel.SetActive(false);
                returnToDivergenceButton.gameObject.SetActive(false);
            }
        }

        void OnPlayerReachedTheEnd()
        {
            confirmatioPanel.SetActive(false);
            returnToDivergenceButton.gameObject.SetActive(false);
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
