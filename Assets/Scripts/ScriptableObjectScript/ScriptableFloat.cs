using System;
using System.Collections;
using UnityEngine;

 [CreateAssetMenu(fileName = "Data", menuName = "Data/Float", order = 1)]
 public class ScriptableFloat : ScriptableObject
 {
    [SerializeField]
    float value;

    public Action valueChangedEvent;
   
    public float Value
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

