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

        void Awake()
        {
            controls.IsCompassEnabled.OnValueChangedHandler += OnCompassEnabled;
        }

        private void OnCompassEnabled(bool value)
        {
            gameObject.SetActive(value);
        }

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}