using UnityEngine;
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

    public delegate void OnAction(ReplayAction action, params object[] args);

    public delegate void OnSequenceToggled(Course course, bool enabled);

    public class ScriptableReplayOptions : ScriptableObject
    {
        public enum ReplayOption
        {
            Player,
            Algorithm
        }

        public Action valueChangeEvent;

        public OnAction OnActionHandler;

        public OnSequenceToggled OnSequenceToggledHandler;

        public OnCourseEvent OnSequenceSelectedHandler;

        public OnEvent OnSequenceFinishedHandler;

        public OnIntEvent OnMoveCountSetHandler;

        public OnIntEvent OnMoveIndexChanged;

        private int index = 0;

        public void OnEnable()
        {
            moveIndex = 0;
            moveCount = int.MinValue;
        }

        public int CourseIndex
        {
            get
            {
                return index;
            }

            set
            {
                index = value;

                if (valueChangeEvent != null)
                    valueChangeEvent();
            }
        }

        [SerializeField]
        private int moveCount = int.MinValue;

        public int GlobalMoveCount
        {
            get
            {
                return moveCount;
            }

            set
            {
                if (moveCount != value )
                { 
                    moveCount = value;
                    if (OnMoveCountSetHandler != null) OnMoveCountSetHandler(value);
                }

            }
        }
              
        [SerializeField]
        private int moveIndex = 0;

        public int GlobalMoveIndex 
        {
            get
            {
                return moveIndex;
            }

            set
            {
                if (value >= 0 && value <= GlobalMoveCount)
                {
                    moveIndex = value;
                    if (OnMoveIndexChanged != null) OnMoveIndexChanged.Invoke(moveIndex);
                }
            }
        }

        public bool HasNext
        {
            get
            {
                if (GlobalMoveCount == 0)
                    return false;

                return GlobalMoveIndex < GlobalMoveCount;
            }
        }

        public bool HasPrevious
        {
            get
            {
                if (GlobalMoveCount == 0)
                    return false;

                return GlobalMoveIndex > 0;
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


        public void SendAction(ReplayAction action, params object[] args)
        {
            if (OnActionHandler != null)
            {
                OnActionHandler.Invoke(action, args);
            }
        }

    }
}
