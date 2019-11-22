using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{

    public class AvatarCharacter : MonoBehaviour
    {
        [SerializeField]
        [Cirrus.Editor.FindAssetOfType(typeof(AvatarControllerAsset))]
        private AvatarControllerAsset controller;

        [SerializeField]
        private Transform character;

        public void FixedUpdate()
        {
            character.transform.rotation = controller.AvatarRotation.Value;
        }

    }
}