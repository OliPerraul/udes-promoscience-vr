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
        [SerializeField]
        private AvatarControllerAsset controls;

        [SerializeField]
        private FirstPersonCamera firstPersonCamera;

        [SerializeField]
        private ThirdPersonCamera thirdPersonCamera;

        [SerializeField]
        private Animator transitionCameraAnimator;

        private IInputScheme inputScheme = new SimulatedInputScheme();

        public IInputScheme InputScheme => inputScheme;


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
                         firstPersonCamera.transform;
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
            // Assign active camera rotation to innactive one
            if (controls.IsThirdPersonEnabled.Value)
            {
                firstPersonCamera.transform.rotation = Quaternion.Euler(
                    firstPersonCamera.transform.rotation.eulerAngles.x,
                    thirdPersonCamera.transform.rotation.eulerAngles.y,
                    firstPersonCamera.transform.rotation.eulerAngles.z
                    );

                // TODO fix
            }
            else
            {
                thirdPersonCamera.transform.rotation = Quaternion.Euler(
                    thirdPersonCamera.transform.rotation.eulerAngles.x,
                    firstPersonCamera.transform.rotation.eulerAngles.y,
                    thirdPersonCamera.transform.rotation.eulerAngles.z
                    );

                // This is probably a bad idea
                thirdPersonCamera.m_LookAngle = firstPersonCamera.transform.rotation.eulerAngles.y;
            }
        }



        public void OnTransitionCameraEnabled(bool enabled)
        {
            transitionCameraAnimator?.gameObject?.SetActive(enabled);
        }
        
    }
}