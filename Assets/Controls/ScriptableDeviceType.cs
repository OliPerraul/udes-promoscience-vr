using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/DeviceType", order = 1)]
    public class ScriptableDeviceType : ScriptableObject
    {        
        private Promoscience.DeviceType value = Promoscience.DeviceType.NoType;

        public void OnEnable()
        {
            value = Promoscience.DeviceType.NoType;
            //InitializeValue();
        }

        public Promoscience.DeviceType Value
        {
            get
            {
                if (value == Promoscience.DeviceType.NoType)
                {
                    InitializeValue();
                }
                return value;
            }
        }

        void InitializeValue()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName[0] == 'T')
            {
                value = Promoscience.DeviceType.Tablet;
            }
            else if (sceneName[0] == 'H')
            {
                value = Promoscience.DeviceType.Headset;
            }
            //else if (sceneName[0] == 'P')// Presentation Support
            //{
            //    value = Promoscience.DeviceType.SupportDevice;
            //}
        }

        public void OnValidate()
        {
            value = Promoscience.DeviceType.NoType;
        }
    }
}

