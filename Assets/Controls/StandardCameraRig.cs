using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine;
using UdeS.Promoscience.ScriptableObjects;
//using UnityStandardAssets.CrossPlatformInput;

namespace UdeS.Promoscience.Controls
{
    public class StandardCameraRig : MonoBehaviour, ICameraRig
    {
        public Transform Transform => transform;

        [SerializeField]
        private AvatarControllerAsset controls;

        [SerializeField]
        private FirstPersonCamera firstPersonCamera;

        [SerializeField]
        private UnityStandardAssets.Cameras.FreeLookCam thirdPersonCamera;

        [SerializeField]
        private Animator transitionCameraAnimator;


        [SerializeField]
        private Transform directionTransform;

        public Transform DirectionArrowTransform => directionTransform;


        private TransitionCameraAnimatorWrapper transitionCameraAnimatorWrapper;

        public TransitionCameraAnimatorWrapper TransitionCameraAnimator => transitionCameraAnimatorWrapper;


        public Vector3 CameraForward => CameraTransform.forward;

        public Quaternion CameraRotation
        {
            get
            {
                return 
                     Quaternion.Euler(
                         0,
                         CameraTransform.rotation.eulerAngles.y,
                         0);

            }
        }

        public Transform CameraTransform
        {
            get
            {
                return controls.IsThirdPersonEnabled.Value ?     
                         thirdPersonCamera.transform :
                         firstPersonCamera.PivotTransform;
            }
        }


        public void Awake()
        {
            controls.IsThirdPersonEnabled.OnValueChangedHandler += OnThirdPersonEnabled;
            controls.IsTransitionCameraEnabled.OnValueChangedHandler += OnTransitionCameraEnabled;
                        
            transitionCameraAnimatorWrapper = transitionCameraAnimator == null ?
                null :
                new TransitionCameraAnimatorWrapper(transitionCameraAnimator);
        }

        public void OnThirdPersonEnabled(bool enabled)
        {
            firstPersonCamera.gameObject.SetActive(!enabled);
            thirdPersonCamera.gameObject.SetActive(enabled);
        }


        public void FixedUpdate()
        {
            if (controls.IsThirdPersonEnabled.Value)
            {
                firstPersonCamera.transform.rotation = Quaternion.Euler(
                    firstPersonCamera.transform.rotation.eulerAngles.x,
                    thirdPersonCamera.transform.rotation.eulerAngles.y,
                    firstPersonCamera.transform.rotation.eulerAngles.z
                    );
            }
            else
            {
                thirdPersonCamera.transform.rotation = Quaternion.Euler(
                    thirdPersonCamera.transform.rotation.eulerAngles.x,
                    firstPersonCamera.transform.rotation.eulerAngles.y,
                    thirdPersonCamera.transform.rotation.eulerAngles.z
                    );
            }
        }



        public void OnTransitionCameraEnabled(bool enabled)
        {
            transitionCameraAnimator.gameObject.SetActive(enabled);
        }
        
    }
}