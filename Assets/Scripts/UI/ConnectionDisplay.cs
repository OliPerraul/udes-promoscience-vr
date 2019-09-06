using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{

    public class ConnectionDisplay : MonoBehaviour
    {
        [SerializeField]
        ScriptableBoolean isConnectedToPair;

        [SerializeField]
        ScriptableBoolean isConnectedToServer;

        [SerializeField]
        GameObject pairPanel;

        [SerializeField]
        GameObject serverPanel;

        private void Start()
        {
            isConnectedToPair.valueChangedEvent += OnIsConnectedToPairValueChanged;
            isConnectedToServer.valueChangedEvent += OnIsConnectedToServerValueChanged;
        }

        void OnIsConnectedToPairValueChanged()
        {
            if (isConnectedToPair.Value)
            {
                if (pairPanel.activeSelf)
                {
                    pairPanel.SetActive(false);
                }
            }
            else
            {
                if (isConnectedToServer.Value)
                {
                    pairPanel.SetActive(true);
                }
            }
        }


        void OnIsConnectedToServerValueChanged()
        {
            if (isConnectedToServer.Value)
            {
                if (!isConnectedToPair.Value)
                {
                    pairPanel.SetActive(true);
                }

                if (serverPanel.activeSelf)
                {
                    serverPanel.SetActive(false);
                }
            }
            else
            {
                if (pairPanel.activeSelf)
                {
                    pairPanel.SetActive(false);
                }

                serverPanel.SetActive(true);
            }
        }
    }
}
