using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UdeS.Promoscience.Network
{
    public class ServerPairingManager : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Button pairingButton;

        [SerializeField]
        private GameObject pairingDisplay;

        private bool isPairing = false;

        void Awake()
        {
            pairingButton.onClick.AddListener(OnPairingClicked);

            pairingDisplay.SetActive(false);
        } 
        
        public void OnPairingClicked()
        {
            isPairing = !isPairing;

            pairingDisplay.SetActive(isPairing);

            if (isPairing) PairingServer.Instance.StartServer();
              
            else PairingServer.Instance.StopServer();              
        }
    }
}
