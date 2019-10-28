using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
//using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.UI
{
    public class AlgorithmDisplay : MonoBehaviour
    {
        [SerializeField]
        ScriptableControler controls;

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

        private bool init = false;

        void OnEnable()
        {
            if (init) return;

            init = true;

            Client.Instance.OnAlgorithmChangedHandler += OnAlgorithmValueChanged;
            Client.Instance.clientStateChangedEvent += OnClientStateChanged;

            if (Client.Instance.DeviceType == DeviceType.Tablet)
            {
                algorithmButton.onClick.AddListener(OnButtonClicked);
                closeButton.onClick.AddListener(OnCloseButtonClicked);
            }

            OnClientStateChanged();
        }


        void OnClientStateChanged()
        {
            switch (Client.Instance.State)
            {
                case ClientGameState.Playing:
                case ClientGameState.PlayingTutorial:
                    display.gameObject.SetActive(true);

                    if(Client.Instance.DeviceType == DeviceType.Tablet)
                        descriptionDisplay.SetActive(false);

                    OnAlgorithmValueChanged();
                    break;

                default:
                    display.gameObject.SetActive(false);
                    descriptionDisplay.SetActive(false);
                    break;

            }
        }


        public void OnButtonClicked()
        {
            if (Client.Instance.DeviceType == DeviceType.Tablet)
                descriptionDisplay.gameObject.SetActive(!descriptionDisplay.gameObject.activeSelf);
        }

        public void OnCloseButtonClicked()
        {
            if (Client.Instance.DeviceType == DeviceType.Tablet)
            {
                descriptionDisplay.gameObject.SetActive(false);//.gameObject.activeSelf);
            }
        }


        void OnAlgorithmValueChanged()
        {

            if (Client.Instance.DeviceType == DeviceType.Tablet)
            {
                text.text = Client.Instance.Algorithm.Name + " (?)";
                descriptionText.text = Client.Instance.Algorithm.Description;
                descriptionName.text = Client.Instance.Algorithm.Name;
            }
            else
            {
                text.text = Client.Instance.Algorithm.Name;
            }
        }


    }
}
