using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/String", order = 1)]
public class ScriptableString : ScriptableObject
{
    [SerializeField]
    string mValue;

    public Action valueChangedEvent;

    public string value
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

