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
        [UnityEngine.Serialization.FormerlySerializedAs("avatarTransform")]
        private Transform characterTransform;

        public Transform CharacterTransform => characterTransform;

        [SerializeField]
        private GameObject mesh;

        [SerializeField]
        private Transform directionTransform;

        public Transform DirectionArrowTransform => directionTransform;

        public Vector3 CameraDirection => controls.IsThirdPersonEnabled.Value ?
            thirdPersonCamera.transform.forward :
            firstPersonCamera.PivotTransform.forward;

        public Quaternion CameraRotation
        {
            get
            {
                return controls.IsThirdPersonEnabled.Value ?
                     Quaternion.Euler(
                         0,
                         thirdPersonCamera.transform.rotation.eulerAngles.y,
                         0) :
                     Quaternion.Euler(
                         0,
                         firstPersonCamera.PivotTransform.rotation.eulerAngles.y,
                         0);

            }
        }

        public void Awake()
        {
            controls.IsThirdPersonEnabled.OnValueChangedHandler += OnThirdPersonEnabled;
            controls.IsThirdPersonEnabled.Value = false;
        }

        public void OnThirdPersonEnabled(bool enabled)
        {
            firstPersonCamera.gameObject.SetActive(!enabled);
            thirdPersonCamera.gameObject.SetActive(enabled);
            mesh.SetActive(enabled);
        }
    }
}