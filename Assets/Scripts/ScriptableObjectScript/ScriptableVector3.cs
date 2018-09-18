using System;
using System.Collections;
using UnityEngine;

 [CreateAssetMenu(fileName = "Data", menuName = "Data/Vector3", order = 1)]
 public class ScriptableVector3 : ScriptableObject
 {
    Vector3 mValue;

    public Action valueChangedEvent;
   
    public Vector3 value
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
    void OnValueChanged()
    {
        if (valueChangedEvent != null)
        {
            valueChangedEvent();
        }
    }
}

