﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

namespace UdeS.Promoscience.Controls
{
    public class SimulatedCameraRig : MonoBehaviour
    {

        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;


        [SerializeField]
        private GameObject character;

        public Transform CharacterTransform
        {
            get
            {
                return character.transform;
            }
        }


        [SerializeField]
        private UnityEngine.Camera camera;

        public void FixedUpdate()//Transform character, Transform camera)
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            character.transform.localRotation *= Quaternion.Euler(0f, yRot, 0f);
            camera.transform.localRotation *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                camera.transform.localRotation = ClampRotationAroundXAxis(camera.transform.localRotation);

            if (smooth)
            {
                character.transform.localRotation = Quaternion.Slerp(character.transform.localRotation, character.transform.localRotation,
                    smoothTime * Time.deltaTime);
                camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, camera.transform.localRotation,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.transform.localRotation = character.transform.localRotation;
                camera.transform.localRotation = camera.transform.localRotation;
            }
        }

        public void NewLookRotation(Transform character, Transform camera)
        {
            float yRot = (Input.GetAxis("Mouse X") * XSensitivity + 1) * Time.deltaTime * 50;
            float xRot = (Input.GetAxis("Mouse Y") * YSensitivity + 1) * Time.deltaTime * 50;

            character.transform.localRotation *= Quaternion.Euler(0f, yRot, 0f);
            camera.transform.localRotation *= Quaternion.Euler(-xRot, 0f, 0f);

            //			if(clampVerticalRotation)
            //				camera.transform.localRotation = ClampRotationAroundXAxis (camera.transform.localRotation);

            if (smooth)
            {
                character.transform.localRotation = Quaternion.Slerp(character.transform.localRotation, character.transform.localRotation,
                                                            smoothTime * Time.deltaTime);
                camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, camera.transform.localRotation,
                                                         smoothTime * Time.deltaTime);
            }
            else
            {
                character.transform.localRotation = character.transform.localRotation;
                camera.transform.localRotation = camera.transform.localRotation;
            }
        }

        public void newLookRotationX(Transform character, Transform camera)
        {
            float yRot = (Input.GetAxis("Mouse X") * XSensitivity + 1) * Time.deltaTime * 50;
            //			float xRot = (CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity+1)*Time.deltaTime*50;
            character.transform.localRotation *= Quaternion.Euler(0f, yRot, 0f);
            //			camera.transform.localRotation *= Quaternion.Euler (-xRot, 0f, 0f);

            //			if(clampVerticalRotation)
            //				camera.transform.localRotation = ClampRotationAroundXAxis (camera.transform.localRotation);

            if (smooth)
            {
                character.transform.localRotation = Quaternion.Slerp(character.transform.localRotation, character.transform.localRotation,
                                                            smoothTime * Time.deltaTime);
                camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, camera.transform.localRotation,
                                                         smoothTime * Time.deltaTime);
            }
            else
            {
                character.transform.localRotation = character.transform.localRotation;
                camera.transform.localRotation = camera.transform.localRotation;
            }
        }


        public void newLookRotationNX(Transform character, Transform camera)
        {
            float yRot = -(Input.GetAxis("Mouse X") * XSensitivity + 1) * Time.deltaTime * 50;
            //			float xRot = (CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity+1)*Time.deltaTime*50;
            character.transform.localRotation *= Quaternion.Euler(0f, yRot, 0f);
            //			camera.transform.localRotation *= Quaternion.Euler (-xRot, 0f, 0f);

            //			if(clampVerticalRotation)
            //				camera.transform.localRotation = ClampRotationAroundXAxis (camera.transform.localRotation);

            if (smooth)
            {
                character.transform.localRotation = Quaternion.Slerp(character.transform.localRotation, character.transform.localRotation,
                                                            smoothTime * Time.deltaTime);
                camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, camera.transform.localRotation,
                                                         smoothTime * Time.deltaTime);
            }
            else
            {
                character.transform.localRotation = character.transform.localRotation;
                camera.transform.localRotation = camera.transform.localRotation;
            }
        }

        public void newLookRotationY(Transform character, Transform camera)
        {
            //			float yRot = (CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity+1)*Time.deltaTime*50;
            float xRot = (Input.GetAxis("Mouse Y") * YSensitivity + 1) * Time.deltaTime * 50;
            //			character.transform.localRotation *= Quaternion.Euler (0f, yRot, 0f);
            camera.transform.localRotation *= Quaternion.Euler(-xRot, 0f, 0f);

            //			if(clampVerticalRotation)
            //				camera.transform.localRotation = ClampRotationAroundXAxis (camera.transform.localRotation);

            if (smooth)
            {
                character.transform.localRotation = Quaternion.Slerp(character.transform.localRotation, character.transform.localRotation,
                                                            smoothTime * Time.deltaTime);
                camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, camera.transform.localRotation,
                                                         smoothTime * Time.deltaTime);
            }
            else
            {
                character.transform.localRotation = character.transform.localRotation;
                camera.transform.localRotation = camera.transform.localRotation;
            }
        }

        public void newLookRotationNY(Transform character, Transform camera)
        {
            //			float yRot = (CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity+1)*Time.deltaTime*50;
            float xRot = -(Input.GetAxis("Mouse Y") * YSensitivity + 1) * Time.deltaTime * 50;
            //			character.transform.localRotation *= Quaternion.Euler (0f, yRot, 0f);
            camera.transform.localRotation *= Quaternion.Euler(-xRot, 0f, 0f);

            //			if(clampVerticalRotation)
            //				camera.transform.localRotation = ClampRotationAroundXAxis (camera.transform.localRotation);

            if (smooth)
            {
                character.transform.localRotation = Quaternion.Slerp(character.transform.localRotation, character.transform.localRotation,
                                                            smoothTime * Time.deltaTime);
                camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, camera.transform.localRotation,
                                                         smoothTime * Time.deltaTime);
            }
            else
            {
                character.transform.localRotation = character.transform.localRotation;
                camera.transform.localRotation = camera.transform.localRotation;
            }
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

        bool CheckTrigger()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
#else
            return Input.GetMouseButton(0);
#endif
        }

    }
}