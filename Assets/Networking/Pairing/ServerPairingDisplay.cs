using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.Controls;
using UdeS.Promoscience.ScriptableObjects;
using Cirrus.Extensions;
using System;

namespace UdeS.Promoscience.Network.UI
{
    public class ServerPairingDisplay : MonoBehaviour
    {

        [SerializeField]
        private Text connectionStatusText;

        [SerializeField]
        private LocalizeInlineString waitingForBothString =
            new LocalizeInlineString(
                "Please connect your devices for pairing.",
                "S.V.P connecter vos appareils pour jumelage.");

        [SerializeField]
        private LocalizeInlineString waitingForHeadsetString =
            new LocalizeInlineString(
                "Please connect your headset for pairing.",
                "S.V.P connecter votre casque pour jumelage.");

        [SerializeField]
        private LocalizeInlineString waitingForTabletString = 
            new LocalizeInlineString(
                "Please connect your tablet for pairing.",
                "S.V.P connecter votre tablette pour jumelage.");

        [SerializeField]
        private LocalizeInlineString pairingResultSuccessString = 
            new LocalizeInlineString(
                "Devices successfully paired.", 
                "Appareils jumelés avec succès.");

        [SerializeField]
        private Image headsetImage;

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
            get => color;
            
            set
            {
                color = value;
                backgroundImage.color = color;
                //button.color = color;
            }
        }

        public void Awake()
        {
            serverImage.color = serverImage.color.SetA(1);
        }

        public void OnDestroy()
        {

        }

        public void Start()
        {            

        }

        public void UpdatePairingState(ServerPairingState state)
        {
            switch (state)
            {
                case ServerPairingState.Tablet:
                    headsetImage.color = headsetImage.color.SetA(disconnectedAlpha);
                    tabletImage.color = tabletImage.color.SetA(1);
                    connectionStatusText.text = waitingForHeadsetString.Value;
                    break;

                case ServerPairingState.Headset:
                    headsetImage.color = headsetImage.color.SetA(1);
                    tabletImage.color = tabletImage.color.SetA(disconnectedAlpha);
                    connectionStatusText.text = waitingForTabletString.Value;
                    break;


                case ServerPairingState.None:
                    headsetImage.color = headsetImage.color.SetA(disconnectedAlpha);
                    tabletImage.color = tabletImage.color.SetA(disconnectedAlpha);
                    connectionStatusText.text = waitingForBothString.Value;
                    break;

                case ServerPairingState.Both:
                    headsetImage.color = headsetImage.color.SetA(1);
                    tabletImage.color = tabletImage.color.SetA(1);
                    connectionStatusText.text = pairingResultSuccessString.Value;
                    break;
            }
        }


        public void Enable(bool enabled)
        {
            transform.gameObject.SetActive(enabled);
            //grabbedMouseFocus.Value = false;
        }

    }
}
