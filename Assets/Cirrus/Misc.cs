using UnityEngine;
using System.Collections;
using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Cirrus
{
    public delegate void Event();

    public delegate void Event<T>(T value);

    public delegate void Event<A, B>(A a, B b);

    public delegate void Event<A,B,C>(A a, B b, C c);



    public delegate void FloatEvent(float value);

    public delegate void IntEvent(int value);

    public delegate void StringEvent(string value);

    public delegate void BoolEvent(bool value);

    [System.Serializable]
    public class ObservableInt : ObservableValue<int> { public ObservableInt(int value) : base(value) { } }

    [System.Serializable]
    public class ObservableBool : ObservableValue<bool> { public ObservableBool(bool value) : base(value) { } }

    [System.Serializable]
    public class ObservableFloat : ObservableValue<float> { public ObservableFloat(float value) : base(value) { } }

    [System.Serializable]
    public class ObservableString : ObservableValue<string> { public ObservableString(string value) : base(value) { } }


    [Serializable]
    public class ObservableValue<T>
    {
        public Event<T> OnValueChangedHandler;

        [SerializeField]
        private T _value;

        public ObservableValue() { }
        
        public ObservableValue(T value)
        {
            _value = value;
        }

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

        public void Set(T value, bool notify=true)
        {
            _value = value;

            if (notify)
            {
                OnValueChangedHandler?.Invoke(value);    
            }
        }
    }
}