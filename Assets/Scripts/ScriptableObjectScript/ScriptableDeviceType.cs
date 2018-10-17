using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Data", menuName = "Data/DeviceType", order = 1)]
public class ScriptableDeviceType : ScriptableObject
{
    [SerializeField]
    DeviceType mValue = DeviceType.NoType;

    public DeviceType value
    {
        get
        {
            if(mValue == DeviceType.NoType)
            {
                InitializeValue();
            }
            return mValue;
        }
    }

    void InitializeValue()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName[0] == 'T')
        {
            mValue = DeviceType.Tablet;
        }
        else if (sceneName[0] == 'H')
        {
            mValue = DeviceType.Headset;
        }
    }
}

