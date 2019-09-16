using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/ClientGameState", order = 1)]
    public class ScriptableClientGameState : ScriptableObject
    {
        [SerializeField]
        ClientGameState value;

        public Action valueChangedEvent;

        public void Awake()
        {
            value = ClientGameState.Connecting;
        }

        public ClientGameState Value
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

