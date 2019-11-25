using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine;
using UdeS.Promoscience.ScriptableObjects;
//using UnityStandardAssets.CrossPlatformInput;

// TODO remove (use OVRHeadsetEmulator)

namespace UdeS.Promoscience.Controls
{
    public class TabletCameraRig : MonoBehaviour
    {
        [SerializeField]
        private AvatarControllerAsset controls;

        [SerializeField]
        private GameObject ThirdPersonCamera;

        [SerializeField]
        private GameObject TopdownCamera;

        public void Awake()
        {
            controls.IsCompassEnabled.OnValueChangedHandler += OnCompassEnabled;
        }

        public void OnCompassEnabled(bool enabled)
        {
            Debug.Log(enabled);

            ThirdPersonCamera.SetActive(!enabled);
            TopdownCamera.SetActive(enabled);

        }
    }
}