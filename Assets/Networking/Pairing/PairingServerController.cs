using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UdeS.Promoscience.Network
{
    public class PairingServerController : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Button pairingButton;

        [SerializeField]
        private UnityEngine.UI.Button closeButton;

        [SerializeField]
        private UI.ServerPairingDisplay pairingDisplay;

        private bool isPairing = false;

        void Awake()
        {
            pairingButton.onClick.AddListener(OnPairingClicked);

            closeButton.onClick.AddListener(OnCloseClick);

            pairingDisplay.Enable(false);

            PairingServer.Instance.PairingState.OnValueChangedHandler += OnPairingStateChanged;

        }

        public void OnPairingStateChanged(ServerPairingState state)
        {
            if (!isPairing) return;

            pairingDisplay.UpdatePairingState(state);
        }

        public void OnPairingClicked()
        {
            DoPair(!isPairing);       
        }

        public void OnCloseClick()
        {
            DoPair(false);
        }

        public void DoPair(bool pairing)
        {
            isPairing = pairing;

            pairingDisplay.Enable(isPairing);

            PairingServer.Instance.PairingState.Set(ServerPairingState.None);

            if (isPairing) PairingServer.Instance.StartServer();

            else PairingServer.Instance.StopServer();
        }
    }
}
