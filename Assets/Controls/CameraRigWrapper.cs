using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{
    // TODO: interface with abstract camera rig

    public interface ICameraRig
    {
        //Transform CharacterTransform { get; }

        Vector3 CameraDirection { get; }

        Quaternion CameraRotation { get; }

        Transform Transform { get; }

        Transform DirectionArrowTransform { get; }

        TransitionCameraAnimatorWrapper TransitionCameraAnimator { get; }
    }

    public class CameraRigWrapper : MonoBehaviour, IInputScheme, ICameraRig
    {
        private IInputScheme inputScheme;

        private ICameraRig cameraRig;

        [SerializeField]
        public UnityEngine.EventSystems.StandaloneInputModule standaloneInputs;

        [SerializeField]
        public ControllerSelection.OVRInputModule ovrInputModule;

        [SerializeField]
        public PCCameraRig desktopCameraRig;

        [SerializeField]
        public VRCameraRig vrCameraRig;   

        public bool IsPrimaryTouchPadDown => inputScheme.IsPrimaryTouchPadDown;

        public bool IsPrimaryTouchPadUp => inputScheme.IsPrimaryTouchPadUp;

        public bool IsPrimaryIndexTriggerDown => inputScheme.IsPrimaryIndexTriggerDown;

        public bool IsPrimaryIndexTriggerUp => inputScheme.IsPrimaryIndexTriggerUp;

        public bool IsLeftPressed => inputScheme.IsLeftPressed;

        public bool IsRightPressed => inputScheme.IsRightPressed;

        public Vector3 CameraDirection => cameraRig.CameraDirection;

        public Quaternion CameraRotation => cameraRig.CameraRotation;

        public Transform Transform => transform;

        public Transform DirectionArrowTransform => cameraRig.DirectionArrowTransform;

        public TransitionCameraAnimatorWrapper TransitionCameraAnimator => cameraRig.TransitionCameraAnimator;

        void Awake()
        {
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
            desktopCameraRig.gameObject.SetActive(true);
            vrCameraRig.gameObject.SetActive(false);
            cameraRig = desktopCameraRig;
            inputScheme = new SimulatedInputScheme();
#elif UNITY_ANDROID
            desktopCameraRig.gameObject.SetActive(false);
            vrCameraRig.gameObject.SetActive(true);
            cameraRig = vrCameraRig;
            inputScheme = new VRInputScheme();
#endif
        }

    }
}

