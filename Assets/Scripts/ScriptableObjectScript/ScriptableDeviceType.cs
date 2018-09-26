using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Data", menuName = "Data/DeviceType", order = 1)]
public class ScriptableDeviceType : ScriptableObject
{
    [SerializeField]
    int mValue = -1;

    public int value
    {
        get
        {
            if(mValue == -1)
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
            mValue = Constants.DEVICE_TABLET;
        }
        else if (sceneName[0] == 'H')
        {
            mValue = Constants.DEVICE_HEADSET;
        }
    }
}

