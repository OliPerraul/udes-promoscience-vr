using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Controls;

namespace UdeS.Promoscience.Controls
{

    public class TabletControlsAsset : ScriptableObject
    {
        // TODO split tablet vs headset avatar controller asset
        // Tablet should not depend on ControlsAsset (aka HeadsetControlsAsset)


        public Cirrus.ObservableValue<TabletCameraMode> TabletCameraMode = new Cirrus.ObservableValue<TabletCameraMode>(Controls.TabletCameraMode.ThirdPerson);

        public Cirrus.ObservableValue<Quaternion> TabletFirstPersonCameraRoation = new Cirrus.ObservableValue<Quaternion>(Quaternion.identity);

        public Cirrus.ObservableValue<Quaternion> CompassRotation = new Cirrus.ObservableValue<Quaternion>();




    }

}