using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/DeviceType", order = 1)]
    public class ScriptableDeviceType : ScriptableObject
    {        
        private Utils.DeviceType value = Utils.DeviceType.NoType;

        public void Awake()
        {
            value = Utils.DeviceType.NoType;
            //InitializeValue();
        }

        public Utils.DeviceType Value
        {
            get
            {
                if (value == Utils.DeviceType.NoType)
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
                value = Utils.DeviceType.Tablet;
            }
            else if (sceneName[0] == 'H')
            {
                value = Utils.DeviceType.Headset;
            }
            //else if (sceneName[0] == 'P')// Presentation Support
            //{
            //    value = Utils.DeviceType.SupportDevice;
            //}
        }

        public void OnValidate()
        {
            value = Utils.DeviceType.NoType;
        }
    }
}

