﻿using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{
    public class CameraRigWrapper : MonoBehaviour
    {
        [SerializeField]
        public SimulatedCameraRig desktopCameraRig;

        [SerializeField]
        public OVRCameraRig ovrCameraRig;

        [SerializeField]
        public UnityEngine.EventSystems.StandaloneInputModule standaloneInputs;

        [SerializeField]
        public ControllerSelection.OVRInputModule ovrInputModule;


        public bool ovrCameraRigEnabled;

        public Transform Transform
        {
            get
            {
                if (ovrCameraRigEnabled)
                {
                    return ovrCameraRig.transform;
                }
                else if (!ovrCameraRigEnabled)
                {
                    return desktopCameraRig.CharacterTransform;
                }
                else return null;
            }
        }



        void Awake()
        {
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
            desktopCameraRig.gameObject.SetActive(true);
            ovrCameraRig.gameObject.SetActive(false);
            ovrInputModule.enabled = false;
            standaloneInputs.enabled = true;
            ovrCameraRigEnabled = false;
            Cursor.lockState = CursorLockMode.Locked;
#elif UNITY_ANDROID
            desktopCameraRig.gameObject.SetActive(true);
            ovrCameraRig.gameObject.SetActive(true);
            ovrInputModule.enabled = true;
            standaloneInputs.enabled = false;
            ovrCameraRigEnabled = true;
#endif
        }

        public bool IsPrimaryTouchPadDown
        {
            get
            {

                if (ovrCameraRigEnabled)
                {
                    return OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad);
                }
                else if (!ovrCameraRigEnabled)
                {
                    return Input.GetKeyDown(KeyCode.Mouse0);
                }
                else return false;
            }
        }

        public bool IsPrimaryTouchPadUp
        {
            get
            {

                if (ovrCameraRigEnabled)
                {
                    return OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad);
                }
                else if (!ovrCameraRigEnabled)
                {
                    return Input.GetKeyUp(KeyCode.Mouse0);
                }
                else return false;
            }
        }

        public bool IsPrimaryIndexTriggerDown
        {
            get
            {

                if (ovrCameraRigEnabled)
                {
                    return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
                }
                else if (!ovrCameraRigEnabled)
                {
                    return Input.GetKeyDown(KeyCode.Mouse1);
                }
                else return false;
            }
        }

        public bool PrimaryIndexTriggerUp
        {
            get
            {

                if (ovrCameraRigEnabled)
                {
                    return OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger);
                }
                else if (!ovrCameraRigEnabled)
                {
                    return Input.GetKeyUp(KeyCode.Mouse1);
                }
                else return false;
            }
        }


        public bool IsLeft
        {
            get
            {

                if (ovrCameraRigEnabled)
                {
                    return OVRInput.GetUp(OVRInput.Button.Left);
                }
                else if (!ovrCameraRigEnabled)
                {
                    return Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A);
                }
                else return false;
            }
        }

        public bool IsRight
        {
            get
            {

                if (ovrCameraRigEnabled)
                {
                    return OVRInput.GetUp(OVRInput.Button.Right);
                }
                else if (!ovrCameraRigEnabled)
                {
                    return Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D);
                }
                else return false;
            }
        }


    }
}
