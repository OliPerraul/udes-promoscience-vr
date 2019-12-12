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
    public class HeadsetCompassTool : HeadsetTool
    {
        public override ToolId Id => ToolId.Compass;

        [SerializeField]
        ControlsAsset controls;

        [SerializeField]
        private HeadsetToolManagerAsset headsetTools;

        [SerializeField]
        Transform indicator;

        int direction = 0;

        private Quaternion startRotation;

        readonly int[] xByDirection = { 0, 1, 0, -1 };
        readonly int[] yByDirection = { -1, 0, 1, 0 };

        void Awake()
        {
            Client.Instance.State.OnValueChangedHandler += OnClientStateChanged;

            startRotation = indicator.transform.rotation;

            OnClientStateChanged(Client.Instance.State.Value);
        }

        void OnClientStateChanged(ClientGameState state)
        {
            switch (Client.Instance.State.Value)
            {
                //case ClientGameState.Playing:
                //case ClientGameState.PlayingTutorial:
                //    if (Client.Instance.Algorithm.Value.Id == Algorithms.Id.Standard)
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
                var rotation = Quaternion.LookRotation(
                    indicator.position + 
                    new Vector3(100 * Labyrinths.Utils.TileSize * xByDirection[direction], 0, 100 * Labyrinths.Utils.TileSize * -yByDirection[direction]));

                indicator.rotation = rotation;

                headsetTools.CompassRotation.Value = rotation;

            }
        }

    }
}