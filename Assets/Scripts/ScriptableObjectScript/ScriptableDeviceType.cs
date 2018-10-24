using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Data", menuName = "Data/DeviceType", order = 1)]
public class ScriptableDeviceType : ScriptableObject
{
    [SerializeField]
    DeviceType value = DeviceType.NoType;

    public DeviceType Value
    {
        get
        {
            if(value == DeviceType.NoType)
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
    }
}

