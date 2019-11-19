using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine;
using UdeS.Promoscience.ScriptableObjects;
//using UnityStandardAssets.CrossPlatformInput;

namespace UdeS.Promoscience.Controls
{
    public class SimulatedCameraRig : MonoBehaviour, ICameraRig
    {
        [SerializeField]
        private AvatarControllerAsset controls;

        [SerializeField]
        private FirstPersonCamera firstPersonCamera;

        [SerializeField]
        private UnityStandardAssets.Cameras.FreeLookCam thirdPersonCamera;

        public Transform Transform => transform;

        [SerializeField]
        private Transform avatarTransform;

        public Transform AvatarTransform =>
            controls.IsThirdPersonEnabled.Value ?
            avatarTransform :
            firstPersonCamera.PivotTransform;

        public Vector3 AvatarDirection => AvatarTransform.forward;

        [SerializeField]
        private GameObject mesh;

        [SerializeField]
        private Transform directionTransform;

        public Transform DirectionTransform => directionTransform;

        public Quaternion AvatarRotation
        {
            get
            {
                if (controls.IsThirdPersonEnabled.Value)
                {
                    return Quaternion.Euler(
                        AvatarTransform.rotation.eulerAngles.x, 
                        0, 
                        AvatarTransform.rotation.eulerAngles.z);
                }
                else
                {
                    return avatarTransform.rotation;
                }
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