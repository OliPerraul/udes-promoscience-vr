﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using UdeS.Promoscience.ScriptableObjects;
using Cirrus.Extensions;
using System;

namespace UdeS.Promoscience.UI
{
    public class ConnectionMenu : MonoBehaviour
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
        private LocalizeString waitingForServer;

        [SerializeField]
        private ScriptableBoolean grabbedMouseFocus;

        [SerializeField]
        private ScriptableClientGameState gameState;

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

        //[SerializeField]
        //private Button button;

        [SerializeField]
        ScriptableBoolean isConnectedToPair;

        [SerializeField]
        ScriptableBoolean isConnectedToServer;


        [SerializeField]
        private float showConnectionDelay = 2f;

        [SerializeField]
        private float readyCloseDelay = 2f;

        void Start()
        {
            isConnectedToPair.valueChangedEvent += OnIsConnectedToPairValueChanged;
            isConnectedToServer.valueChangedEvent += OnIsConnectedToServerValueChanged;
            pairingStatus.valueChangedEvent += OnPairingStatusChanged;
            gameState.valueChangedEvent += OmGameStateChanged;

            serverImage.color = serverImage.color.SetA(disconnectedAlpha);            
            
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

        private void OmGameStateChanged()
        {
            OnIsConnectedToServerValueChanged();
        }

        public void Enable()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            grabbedMouseFocus.Value = false;

            
        }

        public void Disable()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            grabbedMouseFocus.Value = false;
        }

        void OnIsConnectedToPairValueChanged()
        {
            Enable();
            if (pairingStatus.Value == Network.ScriptablePairingStatus.ConnectionStatus.PairingSuccess)
            {

                if (isConnectedToPair.Value)
                {
                    if (isConnectedToServer.Value)
                    {
                        pairedDeviceImage.color = pairedDeviceImage.color.SetA(1);
                        serverImage.color = serverImage.color.SetA(1);
                        connectionStatusText.text = readyString.Value;
                        StartCoroutine(ReadyClose());                        
                    }
                    else
                    {                        
                        serverImage.color = serverImage.color.SetA(disconnectedAlpha);
                        connectionStatusText.text = connectingToServerString.Value;                        
                    }
                }
                else
                {                    
                    pairedDeviceImage.color = pairedDeviceImage.color.SetA(disconnectedAlpha);
                    connectionStatusText.text = connectingToPairString.Value;
                }
            }
        }

        void OnIsConnectedToServerValueChanged()
        {
            Enable();
            if (pairingStatus.Value == Network.ScriptablePairingStatus.ConnectionStatus.PairingSuccess)
            {
                if (isConnectedToServer.Value)
                {
                    if (isConnectedToPair.Value)
                    {                        
                        serverImage.color = serverImage.color.SetA(1);
                        pairedDeviceImage.color = pairedDeviceImage.color.SetA(1);
                        connectionStatusText.text = readyString.Value;
                        StartCoroutine(ReadyClose());
                        
                    }
                    else
                    {    
                        pairedDeviceImage.color = pairedDeviceImage.color.SetA(disconnectedAlpha);
                        connectionStatusText.text = connectingToPairString.Value;                        
                    }
                }
                else
                {                    
                    serverImage.color = serverImage.color.SetA(disconnectedAlpha);
                    connectionStatusText.text = connectingToServerString.Value;
                }
            }
        }

        IEnumerator ShowConnection()
        {
            yield return new WaitForSeconds(showConnectionDelay);

            OnIsConnectedToPairValueChanged();
            OnIsConnectedToServerValueChanged();
     

            yield return null;
        }

        IEnumerator ReadyClose()
        {
            if (gameState.Value == Utils.ClientGameState.Playing ||
                gameState.Value == Utils.ClientGameState.PlayingTutorial)
            {

                yield return new WaitForSeconds(readyCloseDelay);

                Disable();
            }
        }

        void OnPairingStatusChanged()
        {
            Disable();
            switch (pairingStatus.Value)
            {
                case Network.ScriptablePairingStatus.ConnectionStatus.Pairing:
                    connectionStatusText.text = pairingRequestSentString.Value;
                    break;

                case Network.ScriptablePairingStatus.ConnectionStatus.PairingFail:
                    connectionStatusText.text = pairingResultFailString.Value;
                    break;

                case Network.ScriptablePairingStatus.ConnectionStatus.PairingSuccess:
                    connectionStatusText.text = pairingResultSuccessString.Value;
                    StartCoroutine(ShowConnection());
                    break;
            }
        }
    }
}
