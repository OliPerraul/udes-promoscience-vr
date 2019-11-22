using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{

    public class AvatarCharacter : MonoBehaviour
    {
        [SerializeField]
        private AvatarControllerAsset controller;

        [SerializeField]
        private Transform character;

        public void Awake()
        {
            controller.AvatarRotation.OnValueChangedHandler += OnAvatarRotationChanged;
        }

        public void OnAvatarRotationChanged(Quaternion rotation)
        {
            character.transform.rotation = rotation;
        }

    }
}