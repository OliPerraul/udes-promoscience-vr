using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Cirrus.Extensions;
using Cirrus;

namespace UdeS.Promoscience.Replays
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

        ToggleAlgorithm,
        SelectLabyrinth,

        ExitReplay,
        AddCourse,
        Reset
    }

    public delegate void OnAction(ReplayAction action, params object[] args);

    public delegate void OnSequenceToggled(Course course, bool enabled);

    public abstract class ScriptableController : ScriptableObject
    {
        public OnIntEvent valueChangeEvent;

        public IEnumerable<Course> Courses;

        public OnEvent OnLabyrinthReplayHandler;

        public OnAction OnActionHandler;

        public OnSequenceToggled OnSequenceToggledHandler;

        public OnCourseEvent OnSequenceSelectedHandler;

        public OnEvent OnSequenceFinishedHandler;

        public OnIntEvent OnMoveCountSetHandler;

        public OnIntEvent OnMoveIndexChanged;

        private int index = 0;

        public virtual void OnEnable()
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
                    valueChangeEvent.Invoke(index);
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

        public void SendAction(ReplayAction action, params object[] args)
        {
            if (OnActionHandler != null)
            {
                OnActionHandler.Invoke(action, args);
            }
        }

    }
}
