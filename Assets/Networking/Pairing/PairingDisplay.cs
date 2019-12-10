using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.Controls;
using UdeS.Promoscience.ScriptableObjects;
using Cirrus.Extensions;
using System;

namespace UdeS.Promoscience.UI
{
    public class PairingDisplay : MonoBehaviour
    {
        //[SerializeField]
        //private DeviceTypeManagerAsset deviceType;

        [SerializeField]
        private Network.ScriptablePairingStatus pairingStatus;

        [SerializeField]
        private Text connectionStatusText;

        [SerializeField]
        private LocalizeInlineString connectingToPairString;

        [SerializeField]
        private LocalizeInlineString connectingToPairingServerString;

        [SerializeField]
        private LocalizeInlineString pairingRequestSentString;

        [SerializeField]
        private LocalizeInlineString pairingResultSuccessString;

        [SerializeField]
        private LocalizeInlineString pairingResultFailString;

        //[SerializeField]
        //private ScriptableClientGameState gameState;

        [SerializeField]
        private Image headsetImage;

        private Image pairedDeviceImage;

        [SerializeField]
        private Image tabletImage;

        [SerializeField]
        private Image serverImage;

        [SerializeField]
        private float disconnectedAlpha = 0.4f;

        [SerializeField]
        private Image backgroundImage;

        private UnityEngine.Color color;

        public UnityEngine.Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
                backgroundImage.color = color;
                //button.color = color;
            }
        }



        void Start()
        {
            pairingStatus.valueChangedEvent += OnPairingStatusChanged;
            //gameState.valueChangedEvent += OmGameStateChanged;

            serverImage.color = serverImage.color.SetA(disconnectedAlpha);            
            
            switch (Promoscience.Utils.CurrentDeviceType)
            {
                case DeviceType.Headset:
                    headsetImage.color = headsetImage.color.SetA(1);
                    pairedDeviceImage = tabletImage;
                    pairedDeviceImage.color = pairedDeviceImage.color.SetA(disconnectedAlpha);
                    break;

                case DeviceType.Tablet:
                    tabletImage.color = tabletImage.color.SetA(1);
                    pairedDeviceImage = headsetImage;
                    pairedDeviceImage.color = pairedDeviceImage.color.SetA(disconnectedAlpha);
                    break;                    
            }

            Enable();
        }

        public void Enable()
        {
            transform.gameObject.SetActive(true);
            //grabbedMouseFocus.Value = false;
            
        }

        public void Disable()
        {
            transform.gameObject.SetActive(false);
            //grabbedMouseFocus.Value = false;
        }


        void OnPairingStatusChanged()
        {
            //Disable();
            switch (pairingStatus.Value)
            {
                case Network.ScriptablePairingStatus.ConnectionStatus.Connecting:
                    serverImage.color = pairedDeviceImage.color.SetA(disconnectedAlpha);
                    connectionStatusText.text = connectingToPairingServerString.Value;
                    break;

                case Network.ScriptablePairingStatus.ConnectionStatus.Pairing:
                    serverImage.color = pairedDeviceImage.color.SetA(1);
                    connectionStatusText.text = pairingRequestSentString.Value;
                    break;

                case Network.ScriptablePairingStatus.ConnectionStatus.PairingFail:
                    pairedDeviceImage.color = pairedDeviceImage.color.SetA(disconnectedAlpha);
                    connectionStatusText.text = pairingResultFailString.Value;
                    break;

                case Network.ScriptablePairingStatus.ConnectionStatus.PairingSuccess:
                    pairedDeviceImage.color = pairedDeviceImage.color.SetA(1);
                    connectionStatusText.text = pairingResultSuccessString.Value;
                    //StartCoroutine(ShowConnection());
                    break;
            }
        }
    }
}
