using System;
using UdeS.Promoscience.Controls;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UdeS.Promoscience.Controls
{
    [Serializable]
    public class FirstPersonCamera : MonoBehaviour
    {
        public float XSensitivity = 2f;

        public float YSensitivity = 2f;

        public bool clampVerticalRotation = true;

        public float MinimumX = -90F;

        public float MaximumX = 90F;

        public bool smooth;

        public float smoothTime = 5f;

        [SerializeField]
        private Camera camera;

        public Camera Camera => camera;

        [SerializeField]
        private AvatarControllerAsset controls;

        public Transform PivotTransform => transform;

        public void FixedUpdate()
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            PivotTransform.localRotation *= Quaternion.Euler(0f, yRot, 0f);
            camera.transform.localRotation *= Quaternion.Euler(-xRot, 0f, 0f);
            camera.transform.localRotation = ClampRotationAroundXAxis(camera.transform.localRotation);            
        }


        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

    }
}
