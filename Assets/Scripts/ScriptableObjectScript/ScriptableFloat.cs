using System;
using System.Collections;
using UnityEngine;

 [CreateAssetMenu(fileName = "Data", menuName = "Data/Float", order = 1)]
 public class ScriptableFloat : ScriptableObject
 {
    [SerializeField]
    float mValue;

    public Action valueChangedEvent;
   
    public float value
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

