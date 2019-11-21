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



    [Serializable]
    public class ObservableValue<T>
    {
        public Event<T> OnValueChangedHandler;

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

        public void Set(T value, bool forceNotification=true)
        {
            _value = value;

            if(forceNotification)
                if (OnValueChangedHandler != null)
                    OnValueChangedHandler.Invoke(value);
        }
    }
}