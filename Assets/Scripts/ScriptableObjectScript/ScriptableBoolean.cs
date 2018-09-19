using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Bool", order = 1)]
public class ScriptableBoolean : ScriptableObject
{
    [SerializeField]
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

    public void OnValueChanged()
    {
        if (valueChangedEvent != null)
        {
            valueChangedEvent();
        }
    }
}

