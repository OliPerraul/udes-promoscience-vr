using UnityEngine;
using System.Collections;
//using static UdeS.Promoscience.Controls;

namespace UdeS.Promoscience.Controls
{
    public class TabletToolManagerAsset : ScriptableObject
    {
        public Cirrus.ObservableValue<float> ScannedDistance = new Cirrus.ObservableValue<float>();

        public Cirrus.ObservableValue<Quaternion> CompassRotation = new Cirrus.ObservableValue<Quaternion>();

        public Cirrus.ObservableValue<ToolId> CurrentTool = new Cirrus.ObservableValue<ToolId>();

        public Cirrus.Event OnLeftPressedHandler;

        public Cirrus.Event OnRightPressedHandler;

    }
}