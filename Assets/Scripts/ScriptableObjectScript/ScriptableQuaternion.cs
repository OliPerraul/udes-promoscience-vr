using System;
using System.Collections;
using UnityEngine;

 [CreateAssetMenu(fileName = "Data", menuName = "Data/Quaternion", order = 1)]
 public class ScriptableQuaternion : ScriptableObject
 {
    Quaternion value;

    public Action valueChangedEvent;
   
    public Quaternion Value
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
    void OnValueChanged()
    {
        if (valueChangedEvent != null)
        {
            valueChangedEvent();
        }
    }
}

