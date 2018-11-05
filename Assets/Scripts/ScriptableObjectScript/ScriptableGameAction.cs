using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/GameAction", order = 1)]
public class ScriptableGameAction : ScriptableObject
{
    [SerializeField]
    GameAction value;

    public Action valueChangedEvent;

    public GameAction Value
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

