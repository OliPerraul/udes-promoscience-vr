using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{
    // TODO: interface with abstract camera rig

    public class CameraRigWrapper : MonoBehaviour
    {
        [SerializeField]
        public SimulatedCameraRig desktopCameraRig;

        [SerializeField]
        public VRCameraRig vrCameraRig;

        [SerializeField]
        public UnityEngine.EventSystems.StandaloneInputModule standaloneInputs;

        [SerializeField]
        public ControllerSelection.OVRInputModule ovrInputModule;


        public bool ovrCameraRigEnabled;

        public Vector3 AvatarDirection
        {
            get
            {
                if (ovrCameraRigEnabled)
                {
                    return vrCameraRig.OVRCameraRig.centerEyeAnchor.transform.forward;
                }
                else
                {
                    return desktopCameraRig.AvatarTransform.forward;
                }
            }
        }

        public Transform AvatarTransform
        {
            get
            {
                if (ovrCameraRigEnabled)
                {
                    return vrCameraRig.AvatarTransform;
                }
                else if (!ovrCameraRigEnabled)
                {
                    return desktopCameraRig.AvatarTransform;
                }
                else return null;
            }
        }



        public Transform Transform
        {
            get
            {
                if (ovrCameraRigEnabled)
                {
                    return vrCameraRig.transform;
                }
                else if (!ovrCameraRigEnabled)
                {
                    return desktopCameraRig.Transform;
                }
                else return null;
            }
        }


        public Transform DirectionTransform
        {
            get
            {
                if (ovrCameraRigEnabled)
                {
                    return vrCameraRig.DirectionTransform;
                }
                else if (!ovrCameraRigEnabled)
                {
                    return desktopCameraRig.DirectionTransform;
                }
                else return null;
            }
        }


        void Awake()
        {
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
            desktopCameraRig.gameObject.SetActive(true);

            if (vrCameraRig != null)
            {
                vrCameraRig.gameObject.SetActive(false);
            }
            ovrInputModule.enabled = false;
            standaloneInputs.enabled = true;
            ovrCameraRigEnabled = false;
            Cursor.lockState = CursorLockMode.Locked;
#elif UNITY_ANDROID

            if(desktopCameraRig != null)
            {
                desktopCameraRig.gameObject.SetActive(false);
            }

            vrCameraRig.gameObject.SetActive(true);
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
                    return Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space);
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
                    return Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Space);
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
                    return Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Return);
                }
                else return false;
            }
        }

        public bool IsPrimaryIndexTriggerUp
        {
            get
            {
                if (ovrCameraRigEnabled)
                {
                    return OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger);
                }
                else if (!ovrCameraRigEnabled)
                {
                    return Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Return);
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

