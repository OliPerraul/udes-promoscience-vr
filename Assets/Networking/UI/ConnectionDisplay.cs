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
    public class ConnectionDisplay : MonoBehaviour
    {
        [SerializeField]
        private AvatarControllerAsset controls;

        [SerializeField]
        private DeviceTypeManagerAsset deviceType;

        [SerializeField]
        private Network.ScriptablePairingStatus pairingStatus;

        [SerializeField]
        private Text connectionStatusText;

        [SerializeField]
        private LocalizeInlineString connectingToServerString;

        [SerializeField]
        private LocalizeInlineString connectingToPairString;

        // "Svp retirer l'equipement et porter attenttion."
        [SerializeField]
        private LocalizeInlineString pleaseStandbyString;


        [SerializeField]
        private LocalizeInlineString readyString;

        [SerializeField]
        private LocalizeInlineString waitingForServer;

        //[SerializeField]
        //private ScriptableBoolean grabbedMouseFocus;

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

        [SerializeField]
        private List<Teams.TeamResource> teams;

        //[SerializeField]
        //private Button button;

        [SerializeField]
        ScriptableBoolean isConnectedToPair;

        [SerializeField]
        ScriptableBoolean isConnectedToServer;


        //[SerializeField]
        //private float showConnectionDelay = 2f;

        [SerializeField]
        private float readyCloseDelay = 2f;

        void Awake()
        {
            controls.IsThirdPersonEnabled.OnValueChangedHandler += OnThirdPersonEnabled;
            // TODO replace by networkcontrollerasset
            isConnectedToPair.valueChangedEvent += OnIsConnectedToPairValueChanged;
            isConnectedToServer.valueChangedEvent += OnIsConnectedToServerValueChanged;

            Client.Instance.clientStateChangedEvent += OnClientStateChanged;

            serverImage.color = serverImage.color.SetA(disconnectedAlpha);            
            
            switch (deviceType.Value)
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
        }

        public void OnThirdPersonEnabled(bool enabled)
        {
            if (isConnectedToPair.Value && isConnectedToServer.Value)
            {
                Disable();
            }
            else
            {
                OnIsConnectedToServerValueChanged();
            }
        }

        public void OnClientStateChanged()
        {
            switch (Client.Instance.State)
            {
                case ClientGameState.ViewingGlobalReplay:
                    Enable();
                    connectionStatusText.text = pleaseStandbyString.Value;
                    break;

                default:
                    Disable();
                    break;
            }
        }




        public void Enable()
        {
            transform.gameObject.SetActive(true);
            //controls.IsMouseFocusGrabbed.Value = false;
            
        }

        public void Disable()
        {
            transform.gameObject.SetActive(false);
            //controls.IsMouseFocusGrabbed.Value = false;
        }

        void OnIsConnectedToPairValueChanged()
        {
            Enable();

            if (isConnectedToPair.Value)
            {
                if (isConnectedToServer.Value)
                {
                    pairedDeviceImage.color = pairedDeviceImage.color.SetA(1);
                    serverImage.color = serverImage.color.SetA(1);
                    connectionStatusText.text = readyString.Value;
                    if(gameObject.activeSelf)
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

        void OnIsConnectedToServerValueChanged()
        {
            Enable();

            if (isConnectedToServer.Value)
            {
                if (isConnectedToPair.Value)
                {                        
                    serverImage.color = serverImage.color.SetA(1);
                    pairedDeviceImage.color = pairedDeviceImage.color.SetA(1);
                    connectionStatusText.text = readyString.Value;
                    if (gameObject.activeSelf)
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


        IEnumerator ReadyClose()
        {
            yield return new WaitForSeconds(readyCloseDelay);
            Disable();           
        }

    }
}
