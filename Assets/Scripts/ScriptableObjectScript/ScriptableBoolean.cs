using System;
using System.Collections;
using UnityEngine;

namespace UdeS.Promoscience.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/Bool", order = 1)]
    public class ScriptableBoolean : ScriptableObject
    {
        [SerializeField]
        bool value;

        public Action valueChangedEvent;

        public bool Value
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
}