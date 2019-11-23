using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Controls
{
    public class VRCameraRig : MonoBehaviour, ICameraRig
    {
        [UnityEngine.Serialization.FormerlySerializedAs("OVRCameraRig")]
        [SerializeField]
        public OVRCameraRig ovrCameraRig;

        [UnityEngine.Serialization.FormerlySerializedAs("AvatarTransform")]
        [SerializeField]
        public Transform avatarTransform;

        public Transform CharacterTransform => avatarTransform;

        [UnityEngine.Serialization.FormerlySerializedAs("DirectionTransform")]
        [SerializeField]
        public Transform directionTransform;

        public Vector3 CameraDirection => ovrCameraRig.centerEyeAnchor.transform.forward;

        public Quaternion CameraRotation => avatarTransform.rotation;
    
        public Transform Transform => transform;

        public Transform DirectionArrowTransform => directionTransform;

        [SerializeField]
        private OVRCameraRig firstPersonCamera;

        [SerializeField]
        private OVRCameraRig thirdPersonCamera;

        [SerializeField]
        private GameObject mesh;

        [SerializeField]
        private AvatarControllerAsset controls;

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