using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Controls
{
    public class VRCameraRig : MonoBehaviour, ICameraRig
    {
        //[UnityEngine.Serialization.FormerlySerializedAs("OVRCameraRig")]
        //[SerializeField]
        //public OVRCameraRig ovrCameraRig;

        [UnityEngine.Serialization.FormerlySerializedAs("AvatarTransform")]
        [SerializeField]
        public Transform avatarTransform;

        public Transform CharacterTransform => avatarTransform;

        [UnityEngine.Serialization.FormerlySerializedAs("DirectionTransform")]
        [SerializeField]
        public Transform directionTransform;

        [SerializeField]
        private Animator transitionCameraAnimator;

        private TransitionCameraAnimatorWrapper transitionCameraAnimatorWrapper;

        public TransitionCameraAnimatorWrapper TransitionCameraAnimator => transitionCameraAnimatorWrapper;


        public Vector3 CameraDirection => controls.IsThirdPersonEnabled.Value ?
            thirdPersonCamera.centerEyeAnchor.forward :
            firstPersonCamera.centerEyeAnchor.forward;

        public Quaternion CameraRotation
        {
            get
            {
                return controls.IsThirdPersonEnabled.Value ?
                     Quaternion.Euler(
                         0,
                         thirdPersonCamera.centerEyeAnchor.rotation.eulerAngles.y,
                         0) :
                     Quaternion.Euler(
                         0,
                         firstPersonCamera.centerEyeAnchor.rotation.eulerAngles.y,
                         0);

            }
        }


        public Transform Transform => transform;

        public Transform DirectionArrowTransform => directionTransform;

        [SerializeField]
        private OVRCameraRig firstPersonCamera;

        [SerializeField]
        private OVRCameraRig thirdPersonCamera;

        [SerializeField]
        private AvatarControllerAsset controls;

        public void Awake()
        {
            controls.IsThirdPersonEnabled.OnValueChangedHandler += OnThirdPersonEnabled;
            controls.IsTransitionCameraEnabled.OnValueChangedHandler += OnTransitionCameraEnabled;

            transitionCameraAnimatorWrapper = new TransitionCameraAnimatorWrapper(transitionCameraAnimator);

        }

        public void OnThirdPersonEnabled(bool enabled)
        {
            firstPersonCamera.gameObject.SetActive(!enabled);
            thirdPersonCamera.gameObject.SetActive(enabled);
        }


        public void OnTransitionCameraEnabled(bool enabled)
        {
            transitionCameraAnimator.gameObject.SetActive(enabled);
        }
    }
}