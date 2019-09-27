using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace UdeS.Promoscience.Playbacks
{
    public enum PlaybackAction
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

    public delegate void OnAction(PlaybackAction action, params object[] args);

    public class ScriptablePlaybackOptions : ScriptableObject
    {
        public enum PlaybackOption
        {
            Player,
            Algorithm
        }

        public Action valueChangeEvent;

        public OnAction OnActionHandler;

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

        private PlaybackOption option;

        public PlaybackOption Option
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

        public void SendAction(PlaybackAction action, params object[] args)
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
