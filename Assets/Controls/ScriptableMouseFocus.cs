using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Controls
{
    public class ScriptableMouseFocus : ScriptableObject
    {
        public Action valueChangedEvent;

        [System.Serializable]
        public enum MouseFocus
        {
            Menu,
            Hide
        }

        [SerializeField]
        private MouseFocus value = MouseFocus.Menu;

        public void Awake()
        {
            value = MouseFocus.Menu;
        }

        public MouseFocus Value
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