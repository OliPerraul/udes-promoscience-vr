using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Directive", order = 1)]
public class ScriptableDirective : ScriptableObject
{
    [SerializeField]
    Directive value;

    public Action valueChangedEvent;

    public Directive Value
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

