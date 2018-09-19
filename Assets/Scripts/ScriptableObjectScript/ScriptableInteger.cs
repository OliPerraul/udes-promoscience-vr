using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Int", order = 1)]
public class ScriptableInteger : ScriptableObject
{
    [SerializeField]
    int mValue;

    public Action valueChangedEvent;

    public int value
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

