using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Controls
{
    /// <summary>
    /// Shared state of the headset tool manager
    /// </summary>
    public class HeadsetToolManagerAsset : ScriptableObject
    {
        public Cirrus.ObservableValue<ToolId> CurrentTool = new Cirrus.ObservableValue<ToolId>();

        public Cirrus.ObservableValue<float> ScannedDistance = new Cirrus.ObservableValue<float>();

        public Cirrus.ObservableValue<Quaternion> CompassRotation = new Cirrus.ObservableValue<Quaternion>();

    }
}