using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Algorithm", order = 1)]
public class ScriptableAlgorithm : ScriptableObject
{
    [SerializeField]
    Algorithm value;

    public Action valueChangedEvent;

    public Algorithm Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
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

