using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/GameState", order = 1)]
public class ScriptableGameState : ScriptableObject
{
    [SerializeField]
    GameState value;

    public Action valueChangedEvent;

    public GameState Value
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

