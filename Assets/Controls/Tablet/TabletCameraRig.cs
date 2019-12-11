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

    public enum TabletCameraMode
    {
        Topdown,
        FPS,
        ThirdPerson
    }

    public class TabletUtils
    {
        public const int NumCameraMode = 3;
    }


    public class TabletCameraRig : MonoBehaviour
    {
        [SerializeField]
        private ControlsAsset controls;

        [SerializeField]
        private GameObject ThirdPersonCamera;

        [SerializeField]
        private GameObject TopdownCamera;

        [SerializeField]
        private GameObject FirstPersonCamera;

        public void Awake()
        {
            controls.TabletCameraMode.OnValueChangedHandler += OnCompassEnabled;
            controls.CameraRotation.OnValueChangedHandler += OnCameraRotationChanged;
        }

        public void Start()
        {
            Screen.orientation = ScreenOrientation.AutoRotation;
        }

        public void OnCameraRotationChanged(Quaternion rotation)
        {
            FirstPersonCamera.transform.rotation = rotation;
        }

        public void OnCompassEnabled(TabletCameraMode enabled)
        {
            switch (enabled)
            {
                case TabletCameraMode.FPS:
                    //controls.IsThirdPersonEnabled.Value = false;
                    ThirdPersonCamera.SetActive(false);
                    TopdownCamera.SetActive(false);
                    FirstPersonCamera.SetActive(true);
                    break;

                case TabletCameraMode.ThirdPerson:
                    //controls.IsThirdPersonEnabled.Value = true;
                    ThirdPersonCamera.SetActive(true);
                    TopdownCamera.SetActive(false);
                    FirstPersonCamera.SetActive(false);
                    break;

                case TabletCameraMode.Topdown:
                    //controls.IsThirdPersonEnabled.Value = false;
                    ThirdPersonCamera.SetActive(false);
                    TopdownCamera.SetActive(true);
                    FirstPersonCamera.SetActive(false);
                    break;
            }

        }
    }
}