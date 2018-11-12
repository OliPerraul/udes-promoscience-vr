using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/ClientGameState", order = 1)]
public class ScriptableClientGameState : ScriptableObject
{
    [SerializeField]
    ClientGameState value;

    public Action valueChangedEvent;

    public ClientGameState Value
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

