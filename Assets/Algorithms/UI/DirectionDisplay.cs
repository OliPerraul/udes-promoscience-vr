using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{

    public class DirectionDisplay : MonoBehaviour
    {
        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        GameObject directionDisplayer;

        private bool init = false;

        void OnEnable()
        {
            if (init) return;

            init = true;

            Client.Instance.clientStateChangedEvent += OnClientStateChanged;

            OnClientStateChanged();
        }
        
        void OnClientStateChanged()
        {
            switch (Client.Instance.State)
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
