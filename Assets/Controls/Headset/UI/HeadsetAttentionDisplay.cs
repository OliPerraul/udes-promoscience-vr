using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Controls
{
    /// <summary>
    /// Display shows warning when user should be listening to the presentation
    /// </summary>
    public class HeadsetAttentionDisplay : MonoBehaviour
    {
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
                case ClientGameState.WaitingForNextLevel:
                case ClientGameState.ViewingGlobalReplay:
                    gameObject.SetActive(true);
                    break;

                default:
                    gameObject.SetActive(false);
                    break;
            }
        }

    }
}