using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience
{

    public class VRCameraRig : MonoBehaviour
    {
        [SerializeField]
        public OVRCameraRig OVRCameraRig;

        [SerializeField]
        public Transform AvatarTransform;

        [SerializeField]
        public Transform DirectionTransform;   
    }
}