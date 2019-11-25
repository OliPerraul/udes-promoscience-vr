using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Controls;
using UdeS.Promoscience;
using System;
//using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.UI
{
    public class Compass : MonoBehaviour
    {
        [SerializeField]
        AvatarControllerAsset controls;

        [SerializeField]
        Transform indicator;


        int direction = 0;

        private Quaternion startRotation;

        readonly int[] xByDirection = { 0, 1, 0, -1 };
        readonly int[] yByDirection = { -1, 0, 1, 0 };

        void Awake()
        {
            Client.Instance.clientStateChangedEvent += OnClientStateChanged;

            controls.IsCompassEnabled.OnValueChangedHandler += OnCompassENabled;

            startRotation = indicator.transform.rotation;

            OnClientStateChanged();
        }

        private void OnCompassENabled(bool value)
        {
            indicator.gameObject.SetActive(value);
        }

        void OnClientStateChanged()
        {
            switch (Client.Instance.State)
            {
                //case ClientGameState.Playing:
                //case ClientGameState.PlayingTutorial:
                //    if (Client.Instance.Algorithm.Id == Algorithms.Id.Standard)
                //    {
                //        indicator.gameObject.SetActive(true);
                //    }
                //    else
                //    {
                //        indicator.gameObject.SetActive(false);
                //    }
                //    break;

                //default:
                //    indicator.gameObject.SetActive(false);
                //    break;

            }
        }

        void Update()
        {
            indicator.rotation = startRotation;

            if (indicator.gameObject.activeSelf)
            {
                var rotation = Quaternion.LookRotation(indicator.position + new Vector3(100 * Labyrinths.Utils.TILE_SIZE * xByDirection[direction], 0, 100 * Labyrinths.Utils.TILE_SIZE * -yByDirection[direction]));
                indicator.rotation = rotation;

            }
        }

    }
}