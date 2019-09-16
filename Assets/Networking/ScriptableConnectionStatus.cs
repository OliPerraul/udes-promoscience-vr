using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Network
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Connection Status", order = 1)]
    public class ScriptableConnectionStatus : ScriptableObject
    {
        public enum ConnectionStatus
        {
            Pairing,
            PairingSuccess,
            PairingFail,
            ConnectingToPair,
            ConnectingToServer,
            Connected,
            Ready,
        }

        private ConnectionStatus value = ConnectionStatus.Pairing;

        public void Awake()
        {
            value = ConnectionStatus.Pairing;        
        }

        public ConnectionStatus Value
        {
            get
            {
                return value;
            }
        }
    }
}

