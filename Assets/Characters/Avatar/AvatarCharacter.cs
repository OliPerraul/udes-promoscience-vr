using UnityEngine;
using System.Collections;

using UdeS.Promoscience.Controls;

namespace UdeS.Promoscience.Characters
{
    public class AvatarCharacter : MonoBehaviour, IAvatarAnimatorWrapper
    {
        [SerializeField]
        private ControlsAsset controller;

        [SerializeField]
        private TabletControlsAsset tabletControls;

        [SerializeField]
        private Animator animator;

        public Transform RootTransform => transform;

        [SerializeField]
        private Transform characterTransform;


        [SerializeField]
        private GameObject paintingColorDisplay;


        public Transform CharacterTransform => characterTransform;

        public float BaseLayerLayerWeight { set => ((IAvatarAnimatorWrapper)animatorWrapper).BaseLayerLayerWeight = value; }
        public float Speed { set => ((IAvatarAnimatorWrapper)animatorWrapper).Speed = value; }

        private AvatarAnimatorWrapper animatorWrapper;

        public void Awake()
        {
            animatorWrapper = new AvatarAnimatorWrapper(animator);
            controller.CameraRotation.OnValueChangedHandler += OnCameraRotationChanged;
            controller.IsThirdPersonEnabled.OnValueChangedHandler += OnThirdPersonEnabled;
            controller.Animation.OnValueChangedHandler += OnAnimationChanged;

            tabletControls.TabletCameraMode.OnValueChangedHandler += OnTabletCameraMode;
        }

        public void Start()
        {
            animatorWrapper.Play(AvatarAnimation.Idle);
        }

        public void OnTabletCameraMode(TabletCameraMode mode)
        {
            switch (mode)
            {
                case TabletCameraMode.ThirdPerson:
                    CharacterTransform.gameObject.SetActive(true);
                    paintingColorDisplay.SetActive(true);
                    break;

                default:
                    CharacterTransform.gameObject.SetActive(false);
                    paintingColorDisplay.SetActive(false);
                    break;
            }
        }


        public void OnThirdPersonEnabled(bool enabled)
        {
            CharacterTransform.gameObject.SetActive(enabled);
            paintingColorDisplay.SetActive(enabled);
        }

        public void OnCameraRotationChanged(Quaternion rotation)
        {
            CharacterTransform.rotation = rotation;
        }

        // TODO
        // Maybe do not listen on animation itself
        // maybe do listen on Float Speed
        public void OnAnimationChanged(AvatarAnimation animation)
        {
            switch (animation)
            {
                case AvatarAnimation.Idle:
                    Speed = 0f;
                    break;

                case AvatarAnimation.Jumping:
                    Speed = 1f;
                    break;
            }
        }

        public float GetStateSpeed(AvatarAnimation state)
        {
            return ((IAvatarAnimatorWrapper)animatorWrapper).GetStateSpeed(state);
        }

        public float GetClipLength(AvatarAnimation state)
        {
            return ((IAvatarAnimatorWrapper)animatorWrapper).GetClipLength(state);
        }

        public void Play(AvatarAnimation animation, float normalizedTime)
        {
            ((IAvatarAnimatorWrapper)animatorWrapper).Play(animation, normalizedTime);
        }

        public void Play(AvatarAnimation animation)
        {
            ((IAvatarAnimatorWrapper)animatorWrapper).Play(animation);
        }
    }
}