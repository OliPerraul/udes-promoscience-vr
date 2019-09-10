using System;
using System.Collections;
using UdeS.Promoscience.Utils;
using UnityEngine;

namespace UdeS.Promoscience.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/Algorithm", order = 1)]
    public class ScriptableAlgorithm : ScriptableObject
    {
        [SerializeField]
        Utils.Algorithm value;

        public Action valueChangedEvent;

        public Utils.Algorithm Value
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
