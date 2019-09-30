﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replay
{
    public enum ReplayAction
    {
        Previous,
        Next,
        Play,
        Stop,
        Pause,
        Resume,
        Slide,
    }

    public delegate void OnProgress(float progress);
    public delegate void OnAction(ReplayAction action, params object[] args);

    public class ScriptableReplayOptions : ScriptableObject
    {
        public enum ReplayOption
        {
            Player,
            Algorithm
        }

        public Action valueChangeEvent;

        public OnAction OnActionHandler;

        public Event OnSequenceFinishedHsndler;

        public OnProgress OnProgressHandler;       

        public List<CourseData> Courses;

        private int index = 0;

        public int CourseIndex
        {
            get
            {
                return index;
            }

            set
            {
                index = value;

                if(valueChangeEvent != null)
                    valueChangeEvent();
            }
        }

        private ReplayOption option;

        public ReplayOption Option
        {
            get
            {
                return option;
            }

            set
            {
                option = value;

                if (valueChangeEvent != null)
                    valueChangeEvent();
            }
        }

        public void OnEnable()
        {
            Courses = new List<CourseData>();            
        }

        public void SendAction(ReplayAction action, params object[] args)
        {
            if (OnActionHandler != null)
            {
                OnActionHandler.Invoke(action, args);
            }
        }

        public void SendProgress(float progress)
        {
            if (OnProgressHandler != null)
            {
                OnProgressHandler.Invoke(progress);
            }
        }

        //public void On


    }
}