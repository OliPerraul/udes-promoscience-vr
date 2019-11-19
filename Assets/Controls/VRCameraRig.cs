﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Controls
{
    public class VRCameraRig : MonoBehaviour, ICameraRig
    {
        [UnityEngine.Serialization.FormerlySerializedAs("OVRCameraRig")]
        [SerializeField]
        public OVRCameraRig ovrCameraRig;

        [UnityEngine.Serialization.FormerlySerializedAs("AvatarTransform")]
        [SerializeField]
        public Transform avatarTransform;

        [UnityEngine.Serialization.FormerlySerializedAs("DirectionTransform")]
        [SerializeField]
        public Transform directionTransform;


        public Vector3 AvatarDirection => ovrCameraRig.centerEyeAnchor.transform.forward;

        public Transform AvatarTransform => avatarTransform;


        public Quaternion AvatarRotation => avatarTransform.rotation;
    
        public Transform Transform => transform;

        public Transform DirectionTransform => directionTransform;


        public void EnableThirdPerson(bool enable)
        {
           
        }
    }
}