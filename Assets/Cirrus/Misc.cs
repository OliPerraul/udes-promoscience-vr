using UnityEngine;
using System.Collections;
using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Cirrus
{
    public delegate void Event();

    public delegate void Event<T>(T value);

    public delegate void FloatEvent(float value);

    public delegate void IntEvent(int value);

    public delegate void StringEvent(string value);

    public delegate void BoolEvent(bool value);



    [Serializable]
    public class NotifyChangeValue<T>
    {
        public Event<T> OnValueChangedHandler;

        private T _value;

        public T Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (!EqualityComparer<T>.Default.Equals(_value, value))
                {
                    _value = value;
                    if (OnValueChangedHandler != null)
                        OnValueChangedHandler.Invoke(_value);
                }
            }
        }
    }

    [Serializable]
    public class NotifyChangeInt : NotifyChangeValue<int> { }

    [Serializable]
    public class NotifyChangeBool : NotifyChangeValue<bool> { }

    [Serializable]
    public class NotifyChangeString : NotifyChangeValue<string> { }
}