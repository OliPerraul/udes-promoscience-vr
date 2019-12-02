using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Controls
{
    public interface IHeadsetCameraRig
    {
        Vector3 CameraForward { get; }

        Quaternion CameraRotation { get; }

        Transform CameraTransform { get; }

        TransitionCameraAnimatorWrapper TransitionCameraAnimator { get; }
    }

    public class HeadsetCameraRig : MonoBehaviour, IHeadsetCameraRig
    {
        [SerializeField]
        private OVRCameraRig firstPersonCamera;

        [SerializeField]
        private OVRCameraRig thirdPersonCamera;

        [SerializeField]
        private AvatarControllerAsset controls;

        [SerializeField]
        private Animator transitionCameraAnimator;

        [SerializeField]
        private OVRCameraRig transitionCamera;

        [SerializeField]
        private UnityEngine.UI.Image paintingColorOverlay;

        private TransitionCameraAnimatorWrapper transitionCameraAnimatorWrapper;

        public TransitionCameraAnimatorWrapper TransitionCameraAnimator => transitionCameraAnimatorWrapper;

        [UnityEngine.Serialization.FormerlySerializedAs("simulatedPointerTransform")]
        [SerializeField]
        private Transform pointerTransform;

        public Transform PointerTransform => pointerTransform;


        public Vector3 CameraForward => controls.IsThirdPersonEnabled.Value ?
            thirdPersonCamera.centerEyeAnchor.forward :
            firstPersonCamera.centerEyeAnchor.forward;

        public Quaternion CameraRotation =>
                controls.IsThirdPersonEnabled.Value ?
                     Quaternion.Euler(
                         0,
                         thirdPersonCamera.centerEyeAnchor.rotation.eulerAngles.y,
                         0) :
                     Quaternion.Euler(
                         0,
                         firstPersonCamera.centerEyeAnchor.rotation.eulerAngles.y,
                         0);

        public Transform CameraTransform => controls.IsThirdPersonEnabled.Value ?
                    thirdPersonCamera.transform :
                    firstPersonCamera.transform;

        public void Awake()
        {
            controls.IsThirdPersonEnabled.OnValueChangedHandler += OnThirdPersonEnabled;
            controls.IsTransitionCameraEnabled.OnValueChangedHandler += OnTransitionCameraEnabled;
            controls.PaintingColor.OnValueChangedHandler += OnPaitingColorChanged;

            transitionCameraAnimatorWrapper = transitionCameraAnimator == null ?
                null :
                new TransitionCameraAnimatorWrapper(transitionCameraAnimator);

        }

        private void OnPaitingColorChanged(TileColor value)
        {
            if (value == TileColor.Red)
            {
                paintingColorOverlay.gameObject.SetActive(true);
                paintingColorOverlay.color = Color.red;
            }
            else if(value == TileColor.Yellow)
            {
                paintingColorOverlay.gameObject.SetActive(true);
                paintingColorOverlay.color = Color.yellow;
            }
            else
            {
                paintingColorOverlay.gameObject.SetActive(false);
            }
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
            }
        }


        public void OnThirdPersonEnabled(bool enabled)
        {
            firstPersonCamera.gameObject.SetActive(!enabled);
            paintingColorOverlay.gameObject.SetActive(!enabled);
            thirdPersonCamera.gameObject.SetActive(enabled);
            
        }


        public void OnTransitionCameraEnabled(bool enabled)
        {
            transitionCamera.gameObject.SetActive(enabled);
        }
    }
}