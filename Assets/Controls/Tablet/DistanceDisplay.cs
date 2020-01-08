using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Controls.UI
{
    // TODO remove
    /// <summary>
    /// Shows the current distance from the wall or the to exit
    /// </summary>
    public class DistanceDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI north;

        [SerializeField]
        private TMPro.TextMeshProUGUI south;

        [SerializeField]
        private TMPro.TextMeshProUGUI east;

        [SerializeField]
        private TMPro.TextMeshProUGUI west;

        [SerializeField]
        private HeadsetControlsAsset controls;

        [SerializeField]
        private DistanceScanner distanceScanner;

        void Awake()
        {
            //controls.TabletCameraMode.OnValueChangedHandler += OnCompassEnabled;
        }

        private void OnCompassEnabled(TabletCameraMode value)
        {
            gameObject.SetActive(value == TabletCameraMode.ThirdPerson);
        }
    }
}