using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{
    // TODO: interface with abstract camera rig

    public interface ICameraRig
    {
        //Transform CharacterTransform { get; }

        Vector3 CameraForward { get; }

        Quaternion CameraRotation { get; }

        Transform CameraTransform { get; }

        TransitionCameraAnimatorWrapper TransitionCameraAnimator { get; }

        IInputScheme InputScheme { get; }
    }

    public class CameraRigWrapper : MonoBehaviour, IInputScheme, ICameraRig
    {
        private ICameraRig cameraRig;

        //[SerializeField]
        //public UnityEngine.EventSystems.StandaloneInputModule standaloneInputs;

        //[SerializeField]
        //public ControllerSelection.OVRInputModule ovrInputModule;

        [SerializeField]
        public StandardCameraRig desktopCameraRig;

        [SerializeField]
        public VRCameraRig vrCameraRig;   

        public bool IsPrimaryTouchPadDown => InputScheme.IsPrimaryTouchPadDown;

        public bool IsPrimaryTouchPadUp => InputScheme.IsPrimaryTouchPadUp;

        public bool IsPrimaryIndexTriggerDown => InputScheme.IsPrimaryIndexTriggerDown;

        public bool IsPrimaryIndexTriggerUp => InputScheme.IsPrimaryIndexTriggerUp;

        public bool IsLeftPressed => InputScheme.IsLeftPressed;

        public bool IsRightPressed => InputScheme.IsRightPressed;

        public IInputScheme InputScheme => cameraRig.InputScheme;

        public Vector3 CameraForward => cameraRig.CameraForward;

        public Quaternion CameraRotation => cameraRig.CameraRotation;

        public Transform CameraTransform => cameraRig.CameraTransform;

        public TransitionCameraAnimatorWrapper TransitionCameraAnimator => cameraRig.TransitionCameraAnimator;


        void Awake()
        {
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
            desktopCameraRig.gameObject.SetActive(true);
            vrCameraRig.gameObject.SetActive(false);
            cameraRig = desktopCameraRig;
            //desktopCameraRig.gameObject.SetActive(false);
            //vrCameraRig.gameObject.SetActive(true);
            //cameraRig = vrCameraRig;
            //inputScheme = new SimulatedInputScheme();
#elif UNITY_ANDROID
            desktopCameraRig.gameObject.SetActive(false);
            vrCameraRig.gameObject.SetActive(true);
            cameraRig = vrCameraRig;
#endif
        }

    }
}

