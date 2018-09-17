using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Bool", order = 1)]
public class ScriptableBoolean : ScriptableObject
{
    bool mValue;

    public Action valueChangedEvent;

    public bool value
    {
        get
        {
            return mValue;
        }
        set
        {
            mValue = value;
            OnValueChanged();
        }
    }
    void OnValueChanged()
    {
        if (valueChangedEvent != null)
        {
            valueChangedEvent();
        }
    }
}

