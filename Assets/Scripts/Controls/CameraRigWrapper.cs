using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{
    public class CameraRigWrapper : MonoBehaviour
    {
        [SerializeField]
        public SimulatedCameraRig desktopCameraRig;

        [SerializeField]
        public OVRCameraRig ovrCameraRig;

        public Transform Transform
        {
            get
            {
                if (ovrCameraRig)
                {
                    return ovrCameraRig.transform;
                }
                else if (desktopCameraRig)
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
            ovrCameraRig = null;
            Cursor.lockState = CursorLockMode.Locked;
#elif UNITY_ANDROID
            ovrCameraRig.SetActive(true);
            desktopCameraRig.SetActive(false);
            desktopCameraRig = null;
#endif
        }

        public bool IsPrimaryTouchPadDown
        {
            get
            {

                if (ovrCameraRig)
                {
                    return OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad);
                }
                else if (desktopCameraRig)
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

                if (ovrCameraRig)
                {
                    return OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad);
                }
                else if (desktopCameraRig)
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

                if (ovrCameraRig)
                {
                    return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
                }
                else if (desktopCameraRig)
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

                if (ovrCameraRig)
                {
                    return OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger);
                }
                else if (desktopCameraRig)
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

                if (ovrCameraRig)
                {
                    return OVRInput.GetUp(OVRInput.Button.Left);
                }
                else if (desktopCameraRig)
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

                if (ovrCameraRig)
                {
                    return OVRInput.GetUp(OVRInput.Button.Right);
                }
                else if (desktopCameraRig)
                {
                    return Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D);
                }
                else return false;
            }
        }


    }
}

