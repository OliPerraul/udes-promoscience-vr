using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/GameState", order = 1)]
public class ScriptableGameState : ScriptableObject
{
    [SerializeField]
    GameState mValue;

    public Action valueChangedEvent;

    public GameState value
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

