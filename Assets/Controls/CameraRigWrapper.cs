//using UnityEngine;
//using System.Collections;

//namespace UdeS.Promoscience.Controls
//{
//    // TODO: interface with abstract camera rig

//    public interface ICameraRig
//    {
//        //Transform CharacterTransform { get; }

//        Vector3 CameraForward { get; }

//        Quaternion CameraRotation { get; }

//        Transform CameraTransform { get; }

//        TransitionCameraAnimatorWrapper TransitionCameraAnimator { get; }

//        IInputScheme InputScheme { get; }
//    }

//    public class CameraRigWrapper : MonoBehaviour, IInputScheme, ICameraRig
//    {
//        private ICameraRig cameraRig;

//        [SerializeField]
//        public TabletCameraRig desktopCameraRig;

//        [SerializeField]
//        public HeadsetCameraRig vrCameraRig;   

//        public bool IsPrimaryTouchPadDown => InputScheme.IsPrimaryTouchPadDown;

//        public bool IsPrimaryTouchPadUp => InputScheme.IsPrimaryTouchPadUp;

//        public bool IsPrimaryIndexTriggerDown => InputScheme.IsPrimaryIndexTriggerDown;

//        public bool IsPrimaryIndexTriggerUp => InputScheme.IsPrimaryIndexTriggerUp;

//        public bool IsLeftPressed => InputScheme.IsLeftPressed;

//        public bool IsRightPressed => InputScheme.IsRightPressed;

//        private IInputScheme inputScheme = null;

//        public IInputScheme InputScheme => inputScheme == null ? cameraRig.InputScheme : inputScheme;

//        public Vector3 CameraForward => cameraRig.CameraForward;

//        public Quaternion CameraRotation => cameraRig.CameraRotation;

//        public Transform CameraTransform => cameraRig.CameraTransform;

//        public TransitionCameraAnimatorWrapper TransitionCameraAnimator => cameraRig.TransitionCameraAnimator;


//    }
//}

