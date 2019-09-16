using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Network
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Connection Status", order = 1)]
    public class ScriptablePairingStatus : ScriptableObject
    {
        public Action valueChangedEvent;

        [System.Serializable]
        public enum ConnectionStatus
        {
            Undefined,
            Pairing,
            PairingSuccess,
            PairingFail,
        }

        [SerializeField]
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

            set
            {
                this.value = value;
                OnValueChanged();
            }
        }

        public void OnValueChanged()
        {
            if (valueChangedEvent != null)
            {
                valueChangedEvent();
            }
        }
    }
}

