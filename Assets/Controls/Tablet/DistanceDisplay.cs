using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Controls.UI
{

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
        private AvatarControllerAsset controls;

        [SerializeField]
        private DistanceScanner distanceScanner;

        void Awake()
        {
            controls.IsCompassEnabled.OnValueChangedHandler += OnCompassEnabled;
        }

        private void OnCompassEnabled(bool value)
        {
            gameObject.SetActive(value);
        }

        // TODO only on movement
        public void Update()
        {
            int dist = -1;
            north.text = (dist = (int)distanceScanner.ExecuteDistanceScan(Direction.Up)) < 0 ? "?": dist.ToString();
            south.text = (dist = (int)distanceScanner.ExecuteDistanceScan(Direction.Down)) < 0 ? "?" : dist.ToString();
            west.text = (dist = (int)distanceScanner.ExecuteDistanceScan(Direction.Left)) < 0 ? "?" : dist.ToString();
            east.text = (dist = (int)distanceScanner.ExecuteDistanceScan(Direction.Right)) < 0 ? "?" : dist.ToString();
        }
    }
}