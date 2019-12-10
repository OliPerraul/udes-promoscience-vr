using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Controls;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{
    public class AlgorithmDisplay : MonoBehaviour
    {
        [SerializeField]
        ControlsAsset controls;

        [SerializeField]
        GameObject display;

        [SerializeField]
        Text text;

        [SerializeField]
        Text descriptionText;

        [SerializeField]
        Text descriptionName;

        [SerializeField]
        private Button algorithmButton;

        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private GameObject descriptionDisplay;

        void Awake()
        {
            Client.Instance.Algorithm.OnValueChangedHandler += OnAlgorithmValueChanged;
            Client.Instance.State.OnValueChangedHandler += OnClientStateChanged;

            if (Promoscience.Utils.CurrentDeviceType == DeviceType.Tablet)
            {
                algorithmButton.onClick.AddListener(OnButtonClicked);
                closeButton.onClick.AddListener(OnCloseButtonClicked);
            }

            OnClientStateChanged(Client.Instance.State.Value);
        }


        void OnClientStateChanged(ClientGameState state)
        {
            switch (Client.Instance.State.Value)
            {
                case ClientGameState.Playing:
                case ClientGameState.PlayingTutorial:
                    display.gameObject.SetActive(true);

                    if(Promoscience.Utils.CurrentDeviceType == DeviceType.Tablet)
                        descriptionDisplay.SetActive(false);

                    OnAlgorithmValueChanged(Client.Instance.Algorithm.Value);
                    break;

                default:
                    display.gameObject.SetActive(false);

                    if (Promoscience.Utils.CurrentDeviceType == DeviceType.Tablet)
                        descriptionDisplay.SetActive(false);
                    break;

            }
        }


        public void OnButtonClicked()
        {
            if (Promoscience.Utils.CurrentDeviceType == DeviceType.Tablet)
                descriptionDisplay.gameObject.SetActive(!descriptionDisplay.gameObject.activeSelf);
        }

        public void OnCloseButtonClicked()
        {
            if (Promoscience.Utils.CurrentDeviceType == DeviceType.Tablet)
            {
                descriptionDisplay.gameObject.SetActive(false);//.gameObject.activeSelf);
            }
        }


        public void OnAlgorithmValueChanged(Algorithms.Algorithm algorithm)
        {
            if (Promoscience.Utils.CurrentDeviceType == DeviceType.Tablet)
            {
                text.text = Client.Instance.Algorithm.Value.Name + " (?)";
                descriptionText.text = Client.Instance.Algorithm.Value.Description;
                descriptionName.text = Client.Instance.Algorithm.Value.Name;
            }
            else
            {
                text.text = Client.Instance.Algorithm.Value.Name;
            }
        }


    }
}
