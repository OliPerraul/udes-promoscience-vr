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
        //Topdown,
        FPS,
        ThirdPerson
    }

    public class TabletUtils
    {
        public const int NumCameraMode = 2;
    }


    public class TabletCameraRig : MonoBehaviour
    {
        [SerializeField]
        // TODO remove
        private ControlsAsset controls;

        [SerializeField]
        private TabletControlsAsset tabletControls;


        [SerializeField]
        private GameObject ThirdPersonCamera;

        //[SerializeField]
        //private GameObject TopdownCamera;

        [SerializeField]
        private GameObject FirstPersonCamera;

        public void Awake()
        {
            tabletControls.TabletCameraMode.OnValueChangedHandler += OnTabletCameraModeChanged;
            tabletControls.TabletFirstPersonCameraRoation.OnValueChangedHandler += OnCameraRotationChanged;
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

        public void OnTabletCameraModeChanged(TabletCameraMode enabled)
        {
            switch (enabled)
            {
                case TabletCameraMode.FPS:
                    ThirdPersonCamera.SetActive(false);
                    FirstPersonCamera.SetActive(true);
                    break;

                case TabletCameraMode.ThirdPerson:
                    //controls.IsThirdPersonEnabled.Value = true;
                    ThirdPersonCamera.SetActive(true);
                    FirstPersonCamera.SetActive(false);
                    //TopdownCamera.SetActive(false);

                    break;

                //case TabletCameraMode.Topdown:
                //    //controls.IsThirdPersonEnabled.Value = false;
                //    ThirdPersonCamera.SetActive(false);
                //    TopdownCamera.SetActive(true);
                //    FirstPersonCamera.SetActive(false);
                //    break;
            }

        }
    }
}