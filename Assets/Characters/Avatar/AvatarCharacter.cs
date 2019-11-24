using UnityEngine;
using System.Collections;

using UdeS.Promoscience.Controls;

namespace UdeS.Promoscience.Characters
{
    public class AvatarCharacter : MonoBehaviour
    {
        [SerializeField]
        private AvatarControllerAsset controller;

        [SerializeField]
        private Animator animator;

        public Transform RootTransform => transform;

        [SerializeField]
        private Transform characterTransform;

        public Transform CharacterTransform => characterTransform;

        private AvatarAnimatorWrapper animatorWrapper;


        public void Awake()
        {
            animatorWrapper = new AvatarAnimatorWrapper(animator);

            controller.CameraRotation.OnValueChangedHandler += OnCameraRotationChanged;
            controller.IsThirdPersonEnabled.OnValueChangedHandler += OnThirdPersonEnabled;
        }

        public void Start()
        {
            animatorWrapper.Play(AvatarAnimation.Idle);
        }

        public void OnThirdPersonEnabled(bool enabled)
        {
            CharacterTransform.gameObject.SetActive(enabled);
        }

        public void OnCameraRotationChanged(Quaternion rotation)
        {
            CharacterTransform.rotation = rotation;
        }

    }
}