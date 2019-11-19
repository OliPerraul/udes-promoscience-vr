using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{
    public interface IInputScheme
    {
        bool IsPrimaryTouchPadDown { get; }

        bool IsPrimaryTouchPadUp { get; }

        bool IsPrimaryIndexTriggerDown { get; }

        bool IsPrimaryIndexTriggerUp { get; }

        bool IsLeftPressed { get; }

        bool IsRightPressed { get; }

    }

    public class SimulatedInputScheme : IInputScheme
    {

        public bool IsPrimaryTouchPadDown => Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space);

        public bool IsPrimaryTouchPadUp => Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Space);

        public bool IsPrimaryIndexTriggerDown => Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Return);

        public bool IsPrimaryIndexTriggerUp => Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Return);

        public bool IsLeftPressed => Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A);

        public bool IsRightPressed => Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D);

    }

    public class VRInputScheme : IInputScheme
    {
        public bool IsPrimaryTouchPadDown => OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad);

        public bool IsPrimaryTouchPadUp => OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad);

        public bool IsPrimaryIndexTriggerDown => OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);

        public bool IsPrimaryIndexTriggerUp => OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger);

        public bool IsLeftPressed => OVRInput.GetUp(OVRInput.Button.Left);

        public bool IsRightPressed => OVRInput.GetUp(OVRInput.Button.Right);
    }

}