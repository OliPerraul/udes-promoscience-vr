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
    public class TabletCompassTool : BaseTool
    {
        public override ToolId Id => ToolId.Compass;

        [SerializeField]
        ControlsAsset controls;

        [SerializeField]
        private TabletControlsAsset tabletControls;

        [SerializeField]
        Transform indicator;

        //int direction = 0;

        //private Quaternion startRotation;

        //readonly int[] xByDirection = { 0, 1, 0, -1 };
        //readonly int[] yByDirection = { -1, 0, 1, 0 };

        void Awake()
        {
            //Client.Instance.State.OnValueChangedHandler += OnClientStateChanged;

            tabletControls.CompassRotation.OnValueChangedHandler += OnCompassRotation;

            //tabletControls.TabletCameraMode.OnValueChangedHandler += OnCompassENabled;

            //startRotation = indicator.transform.rotation;

            //OnClientStateChanged(Client.Instance.State.Value);
        }


        void OnCompassRotation(Quaternion rot)
        {
            indicator.rotation = rot;

            //if (indicator.gameObject.activeSelf)
            //{
            //    var rotation = Quaternion.LookRotation(indicator.position + new Vector3(100 * Labyrinths.Utils.TileSize * xByDirection[direction], 0, 100 * Labyrinths.Utils.TileSize * -yByDirection[direction]));
            //    indicator.localRotation = rotation;

            //}
        }

    }
}