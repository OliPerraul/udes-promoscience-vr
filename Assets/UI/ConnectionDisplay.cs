﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using UdeS.Promoscience.ScriptableObjects;
using Cirrus.Extensions;

namespace UdeS.Promoscience.UI
{
    public class ConnectionDisplay : MonoBehaviour
    {
        [SerializeField]
        private ScriptableDeviceType deviceType;

        [SerializeField]
        private Network.ScriptablePairingStatus pairingStatus;

        [SerializeField]
        private Text connectionStatusText;

        [SerializeField]
        private LocalizeString connectingToServerString;

        [SerializeField]
        private LocalizeString connectingToPairString;

        [SerializeField]
        private LocalizeString connectingToPairingServerString;

        [SerializeField]
        private LocalizeString pairingRequestSentString;

        [SerializeField]
        private LocalizeString pairingResultSuccessString;

        [SerializeField]
        private LocalizeString pairingResultFailString;

        [SerializeField]
        private LocalizeString readyString;


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

        [SerializeField]
        private List<ScriptableTeam> teams;

        [SerializeField]
        private Button button;

        [SerializeField]
        ScriptableBoolean isConnectedToPair;

        [SerializeField]
        ScriptableBoolean isConnectedToServer;


        [SerializeField]
        private float showConnectionDelay = 2f;

        void Start()
        {
            isConnectedToPair.valueChangedEvent += OnIsConnectedToPairValueChanged;
            isConnectedToServer.valueChangedEvent += OnIsConnectedToServerValueChanged;
            pairingStatus.valueChangedEvent += OnPairingStatusChanged;
            serverImage.color = serverImage.color.SetA(disconnectedAlpha);

            EnableButton(false);
            
            switch (deviceType.Value)
            {
                case Utils.DeviceType.Headset:
                    headsetImage.color = headsetImage.color.SetA(1);
                    pairedDeviceImage = tabletImage;
                    pairedDeviceImage.color = pairedDeviceImage.color.SetA(disconnectedAlpha);
                    break;

                case Utils.DeviceType.Tablet:
                    tabletImage.color = tabletImage.color.SetA(1);
                    pairedDeviceImage = headsetImage;
                    pairedDeviceImage.color = pairedDeviceImage.color.SetA(disconnectedAlpha);
                    break;
            }
        }

        void OnIsConnectedToPairValueChanged()
        {
            if (pairingStatus.Value == Network.ScriptablePairingStatus.ConnectionStatus.PairingSuccess)
            {

                if (isConnectedToPair.Value)
                {
                    if (isConnectedToServer.Value)
                    {
                        pairedDeviceImage.color = pairedDeviceImage.color.SetA(1);
                        serverImage.color = serverImage.color.SetA(1);
                        connectionStatusText.text = readyString.Value;

                        EnableButton(true);
                    }
                    else
                    {
                        serverImage.color = serverImage.color.SetA(disconnectedAlpha);
                        connectionStatusText.text = connectingToServerString.Value;

                        EnableButton(false);
                    }
                }
                else
                {
                    pairedDeviceImage.color = pairedDeviceImage.color.SetA(disconnectedAlpha);
                    connectionStatusText.text = connectingToPairString.Value;

                    EnableButton(false);
                }
            }
        }

        void OnIsConnectedToServerValueChanged()
        {
            if (pairingStatus.Value == Network.ScriptablePairingStatus.ConnectionStatus.PairingSuccess)
            {
                if (isConnectedToServer.Value)
                {
                    if (isConnectedToPair.Value)
                    {
                        serverImage.color = serverImage.color.SetA(1);
                        pairedDeviceImage.color = pairedDeviceImage.color.SetA(1);
                        connectionStatusText.text = readyString.Value;

                        EnableButton();
                    }
                    else
                    {
                        pairedDeviceImage.color = pairedDeviceImage.color.SetA(disconnectedAlpha);
                        connectionStatusText.text = connectingToPairString.Value;

                        EnableButton(false);
                    }
                }
                else
                {
                    serverImage.color = serverImage.color.SetA(disconnectedAlpha);
                    connectionStatusText.text = connectingToServerString.Value;

                    EnableButton(false);
                }
            }
        }

        IEnumerator ShowConnection()
        {
            yield return new WaitForSeconds(showConnectionDelay);

            OnIsConnectedToPairValueChanged();
            OnIsConnectedToServerValueChanged();
        }

        public void EnableButton(bool enable = true)
        {
            if (enable)
            {
                button.image.color = button.image.color.SetA(1);
                button.interactable = true;
            }
            else
            {
                button.image.color = button.image.color.SetA(disconnectedAlpha);
                button.interactable = false;
            }
        }


        void OnPairingStatusChanged()
        {
            switch (pairingStatus.Value)
            {
                case Network.ScriptablePairingStatus.ConnectionStatus.Pairing:
                    connectionStatusText.text = pairingRequestSentString.Value;

                    EnableButton(false);

                    break;

                case Network.ScriptablePairingStatus.ConnectionStatus.PairingFail:
                    connectionStatusText.text = pairingResultFailString.Value;

                    EnableButton(false);

                    break;

                case Network.ScriptablePairingStatus.ConnectionStatus.PairingSuccess:
                    connectionStatusText.text = pairingResultSuccessString.Value;
                    StartCoroutine(ShowConnection());

                    EnableButton(false);
                    break;
            }
        }
    }
}
