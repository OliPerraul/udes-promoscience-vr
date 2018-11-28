using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/TileArray", order = 1)]
public class ScriptableTileArray : ScriptableObject
{
    [SerializeField]
    Tile[] value;

    public Action valueChangedEvent;

    public Tile[] Value
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

