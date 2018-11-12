using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/ServerGameState", order = 1)]
public class ScriptableServerGameState : ScriptableObject
{
    [SerializeField]
    ServerGameState value;

    public Action valueChangedEvent;

    public ServerGameState Value
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

