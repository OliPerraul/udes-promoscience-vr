using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine;
using UdeS.Promoscience.ScriptableObjects;
//using UnityStandardAssets.CrossPlatformInput;

namespace UdeS.Promoscience.Controls
{
    public class SimulatedCameraRig : MonoBehaviour
    {
        [SerializeField]
        private AvatarControllerAsset controls;

        [SerializeField]
        private FirstPersonCamera firstPersonCamera;

        [SerializeField]
        private UnityStandardAssets.Cameras.FreeLookCam thirdPersonCamera;

        public Transform Transform
        {
            get
            {
                return transform;
            }
        }

        [SerializeField]
        private Transform avatarTransform;

        public Transform AvatarTransform
        {
            get
            {
                return avatarTransform;
            }
        }

        [SerializeField]
        private GameObject mesh;

        [SerializeField]
        private Transform directionTransform;

        public Transform DirectionTransform
        {
            get
            {
                return directionTransform;
            }
        }

        public void Awake()
        {
            controls.IsThirdPersonEnabled.OnValueChangedHandler += OnThirdPersonEnabled;
            controls.IsThirdPersonEnabled.Value = false;
        }

        public void OnThirdPersonEnabled(bool enable)
        {    
            firstPersonCamera.gameObject.SetActive(!enable);
            thirdPersonCamera.gameObject.SetActive(enable);
            mesh.SetActive(enable);
        }
    }
}