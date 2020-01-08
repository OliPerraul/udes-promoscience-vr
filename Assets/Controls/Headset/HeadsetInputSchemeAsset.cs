using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{
    /// <summary>
    /// Contient le code définissant le schéma pour les entrées de l’utilisateur via la manette ou autre périphériques. 
    /// </summary>
    public interface IHeadsetInputScheme
    {
        bool IsPrimaryTouchPadDown { get; }

        bool IsPrimaryTouchPadUp { get; }

        bool IsPrimaryIndexTriggerHeld { get; }

        bool IsPrimaryIndexTriggerDown { get; }

        bool IsPrimaryIndexTriggerUp { get; }

        bool IsLeftPressed { get; }

        bool IsRightPressed { get; }

        bool IsUpPressed { get; }

        bool IsDownPressed { get; }

        bool IsAnyPressed { get; }

        bool IsCameraKeyPressed { get; }
    }

    /// <summary>
    /// Desktop implementation of headset input used for debugging
    /// </summary>
    public class SimulatedHeadsetInputScheme : IHeadsetInputScheme
    {
        public bool IsPrimaryTouchPadDown => Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space);

        public bool IsPrimaryTouchPadUp => Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Space);

        public bool IsPrimaryIndexTriggerHeld => Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Return);

        public bool IsPrimaryIndexTriggerDown => Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Return);

        public bool IsPrimaryIndexTriggerUp => Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Return);

        public bool IsLeftPressed => Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A);

        public bool IsRightPressed => Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D);

        public bool IsUpPressed => Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W);

        public bool IsDownPressed => Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S);

        public bool IsAnyPressed => Input.anyKeyDown;

        public bool IsCameraKeyPressed => Input.GetKeyDown(KeyCode.K);
    }

    public class OVRInputScheme : IHeadsetInputScheme
    {
        public bool IsPrimaryTouchPadDown => OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad);

        public bool IsPrimaryTouchPadUp => OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad);

        public bool IsPrimaryIndexTriggerHeld => OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);

        public bool IsPrimaryIndexTriggerDown => OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);

        public bool IsPrimaryIndexTriggerUp => OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger);

        public bool IsLeftPressed => OVRInput.GetUp(OVRInput.Button.Left);

        public bool IsRightPressed => OVRInput.GetUp(OVRInput.Button.Right);

        // Swipe up
        public bool IsUpPressed => OVRInput.GetUp(OVRInput.Button.Up);

        // Swipe down
        public bool IsDownPressed => OVRInput.GetUp(OVRInput.Button.Down);

        public bool IsAnyPressed => OVRInput.GetUp(OVRInput.Button.Any);

        public bool IsCameraKeyPressed => OVRInput.GetUp(OVRInput.Button.Back);
    }

    public class HeadsetInputSchemeAsset : ScriptableObject, IHeadsetInputScheme
    {
        private IHeadsetInputScheme inputScheme;

        public bool IsPrimaryTouchPadDown => inputScheme.IsPrimaryTouchPadDown;

        public bool IsPrimaryTouchPadUp => inputScheme.IsPrimaryTouchPadUp;

        public bool IsPrimaryIndexTriggerHeld => inputScheme.IsPrimaryIndexTriggerHeld;

        public bool IsPrimaryIndexTriggerDown => inputScheme.IsPrimaryIndexTriggerDown;

        public bool IsPrimaryIndexTriggerUp => inputScheme.IsPrimaryIndexTriggerUp;

        public bool IsLeftPressed => inputScheme.IsLeftPressed;

        public bool IsRightPressed => inputScheme.IsRightPressed;

        public bool IsUpPressed => inputScheme.IsUpPressed;

        public bool IsDownPressed => inputScheme.IsDownPressed;

        public bool IsAnyPressed => inputScheme.IsAnyPressed;

        public bool IsCameraKeyPressed => inputScheme.IsCameraKeyPressed;

        public void OnEnable()
        {
#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR
            if(inputScheme == null)
                inputScheme = new SimulatedHeadsetInputScheme();
#elif UNITY_ANDROID
        if(inputScheme == null)
            inputScheme = new OVRInputScheme();
#endif
        }
    }


}