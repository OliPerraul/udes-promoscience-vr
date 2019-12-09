using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Controls;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{

    public class DirectionDisplay : MonoBehaviour
    {
        [SerializeField]
        ControlsAsset controls;

        [SerializeField]
        GameObject directionDisplayer;

        void Awake()
        {
            Client.Instance.State.OnValueChangedHandler += OnClientStateChanged;

            OnClientStateChanged(Client.Instance.State.Value);
        }
        
        void OnClientStateChanged(ClientGameState state)
        {
            switch (Client.Instance.State.Value)
            {
                case ClientGameState.Playing:
                case ClientGameState.PlayingTutorial:
                    directionDisplayer.SetActive(true);
                    break;

                default:
                    directionDisplayer.SetActive(false);
                    break;

            }
        }
    }
}
