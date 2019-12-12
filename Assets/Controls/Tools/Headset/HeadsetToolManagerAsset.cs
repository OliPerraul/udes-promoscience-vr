using UnityEngine;
using System.Collections;
//using static UdeS.Promoscience.Controls;

namespace UdeS.Promoscience.Controls
{
    public class HeadsetToolManagerAsset : ScriptableObject
    {
        public Cirrus.ObservableValue<ToolId> CurrentTool = new Cirrus.ObservableValue<ToolId>();

        //public Cirrus.ObservableValue<float> WallDistance = new Cirrus.ObservableValue<float>();

        public Cirrus.ObservableValue<float> ScannedDistance = new Cirrus.ObservableValue<float>();

        public Cirrus.ObservableValue<Quaternion> CompassRotation = new Cirrus.ObservableValue<Quaternion>();

    }
}