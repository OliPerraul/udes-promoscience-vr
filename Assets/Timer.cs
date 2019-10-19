﻿using UnityEngine;
using System.Collections;
using Cirrus;

// TODO: use in cooldown


namespace UdeS.Promoscience
{
    // A timer cannot be created in Start(), or Wake() because it needs the Clock, instead to create duiring init use OnEnable
    public class Timer
    {
        bool _repeat = false;
        float _limit = -1;
        float _time = 0f;

        bool active = false;
        public bool IsActive
        {
            get
            {
                return active;
            }
        }

        public OnEvent OnTimeLimitHandler;

        public Timer(float limit, bool start = true, bool repeat = false)
        {
            _time = 0;
            _limit = limit;
            _repeat = repeat;

            if (start)
            {
                Start();
            }
        }

        public float Time
        {
            get
            {
                return _time;
            }
        }

        public void Reset(float limit=-1)
        {
            if (limit > 0)
            {
                _limit = limit;
            }

            _time = 0;
        }

        public void Start()
        {
            Reset();

            if (!active)
            {
                Clock.Instance.OnTickedHandler += OnTicked;
            }

            active = true;
        }

        public void Resume()
        {
            if (!active)
            {
                Clock.Instance.OnTickedHandler += OnTicked;
            }

            active = true;
        }


        public void Stop()
        {
            if (active)
            {
                Clock.Instance.OnTickedHandler -= OnTicked;
            }

            active = false;
        }

        private void OnTicked()
        {
            _time += UnityEngine.Time.deltaTime;
            if (_time >= _limit)
            {
                if(OnTimeLimitHandler != null)
                OnTimeLimitHandler.Invoke();

                if (_repeat)
                {
                    Reset();
                }
                else
                {
                    Stop();
                }         
            }
        }

        ~Timer()
        {
            Stop();
        }
    }
}
