using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/TileColor", order = 1)]
public class ScriptableTileColor : ScriptableObject
{
    [SerializeField]
    TileColor value;

    public Action valueChangedEvent;

    public TileColor Value
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

