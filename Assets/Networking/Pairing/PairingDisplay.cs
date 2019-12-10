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
        private LocalizeInlineString connectingToPairString2 =
            new LocalizeInlineString(
                "Connecting to paired device.",
                "Connexion a l'appareil jumelé.");

        [SerializeField]
        private LocalizeInlineString connectingToPairingServerString2 = 
            new LocalizeInlineString(
                "Connecting to pairing server.", 
                "Connexion au serveur de jumelage.");

        // jumelés
        [SerializeField]
        private LocalizeInlineString pairingRequestSentString2 = 
            new LocalizeInlineString(
                "Pairing request sent.", 
                "Demande de jumelage envoyée.");

        [SerializeField]
        private LocalizeInlineString pairingResultSuccessString2 =
            new LocalizeInlineString(
                "Devices successfully paired.",
                "Appareils jumelés avec succès.");


        [SerializeField]
        private LocalizeInlineString pairingResultFailString2 =
            new LocalizeInlineString(
                "Pairing error occured. Please try again.",
                "Erreur de jumelage. S.V.P. réessayer.");

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

        //public void Disable()
        //{
        //    transform.gameObject.SetActive(false);
        //    //grabbedMouseFocus.Value = false;
        //}


        void OnPairingStatusChanged()
        {
            //Disable();
            switch (pairingStatus.Value)
            {
                case Network.ScriptablePairingStatus.ConnectionStatus.Connecting:
                    serverImage.color = pairedDeviceImage.color.SetA(disconnectedAlpha);
                    connectionStatusText.text = connectingToPairingServerString2.Value;
                    break;

                case Network.ScriptablePairingStatus.ConnectionStatus.Pairing:
                    serverImage.color = pairedDeviceImage.color.SetA(1);
                    connectionStatusText.text = pairingRequestSentString2.Value;
                    break;

                case Network.ScriptablePairingStatus.ConnectionStatus.PairingFail:
                    pairedDeviceImage.color = pairedDeviceImage.color.SetA(disconnectedAlpha);
                    connectionStatusText.text = pairingResultFailString2.Value;
                    break;

                case Network.ScriptablePairingStatus.ConnectionStatus.PairingSuccess:
                    pairedDeviceImage.color = pairedDeviceImage.color.SetA(1);
                    connectionStatusText.text = pairingResultSuccessString2.Value;
                    //StartCoroutine(ShowConnection());
                    break;
            }
        }
    }
}
