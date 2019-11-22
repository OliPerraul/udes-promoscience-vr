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

        public Transform AvatarTransform => avatarTransform;

        public Vector3 AvatarDirection => controls.IsThirdPersonEnabled.Value ? 
            AvatarTransform.forward : 
            firstPersonCamera.PivotTransform.forward;

        [SerializeField]
        private GameObject mesh;

        [SerializeField]
        private Transform directionTransform;

        public Transform DirectionArrowTransform => directionTransform;

        public Quaternion LookRotation
        {
            get
            {
                if (controls.IsThirdPersonEnabled.Value)
                {
                    return Quaternion.Euler(
                        firstPersonCamera.PivotTransform.rotation.eulerAngles.x, 
                        0,
                        firstPersonCamera.PivotTransform.rotation.eulerAngles.z);
                }
                else
                {
                    return Quaternion.Euler(
                         thirdPersonCamera.transform.rotation.eulerAngles.x,
                         0,
                         thirdPersonCamera.transform.rotation.eulerAngles.z);
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