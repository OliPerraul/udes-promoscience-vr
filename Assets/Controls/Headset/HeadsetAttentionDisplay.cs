using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Controls
{

    public class HeadsetAttentionDisplay : MonoBehaviour
    {
        // Start is called before the first frame update
        public void Awake()
        {
            Client.Instance.clientStateChangedEvent += OnClientStateChanged;
        }

        public void OnClientStateChanged()
        {
            switch (Client.Instance.State)
            {
                case ClientGameState.WaitingReplay:
                    gameObject.SetActive(true);
                    break;

                default:
                    gameObject.SetActive(false);
                    break;
            }
        }

    }
}