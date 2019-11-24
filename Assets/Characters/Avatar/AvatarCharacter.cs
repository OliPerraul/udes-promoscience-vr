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

        //[SerializeField]
        //private Transform rootTransform;

        public Transform Transform => transform;


        private AvatarAnimatorWrapper animatorWrapper;


        public void Awake()
        {
            animatorWrapper = new AvatarAnimatorWrapper(animator);

            controller.CameraRotation.OnValueChangedHandler += OnAvatarRotationChanged;
            controller.IsThirdPersonEnabled.OnValueChangedHandler += OnThirdPersonEnabled;
        }

        public void Start()
        {
            animatorWrapper.Play(AvatarAnimation.Idle);
        }

        public void OnThirdPersonEnabled(bool enabled)
        {
            gameObject.SetActive(enabled);
        }

        public void OnAvatarRotationChanged(Quaternion rotation)
        {
            Transform.rotation = rotation;
        }

    }
}