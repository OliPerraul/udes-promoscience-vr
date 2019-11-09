using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/DeviceType", order = 1)]
    public class DeviceTypeManagerAsset : ScriptableObject
    {        
        private DeviceType value = DeviceType.NoType;

        public void OnEnable()
        {
            value = DeviceType.NoType;
            //InitializeValue();
        }

        public DeviceType Value
        {
            get
            {
                if (value == DeviceType.NoType)
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
                value = DeviceType.Tablet;
            }
            else if (sceneName[0] == 'H')
            {
                value = DeviceType.Headset;
            }
            //else if (sceneName[0] == 'P')// Presentation Support
            //{
            //    value = Promoscience.DeviceType.SupportDevice;
            //}
        }

        public void OnValidate()
        {
            value = DeviceType.NoType;
        }
    }
}

