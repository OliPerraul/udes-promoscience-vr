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
            Client.Instance.State.OnValueChangedHandler += OnClientStateChanged;
        }

        public void Start()
        {
            gameObject.SetActive(false);
        }

        public void OnClientStateChanged(ClientGameState state)
        {
            switch (state)
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