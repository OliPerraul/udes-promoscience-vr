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

        ToggleOptions,
        ToggleAlgorithm,
        ToggleGreyboxLabyrinth,

        SelectLabyrinth,
        SelectReplay,
        ExitReplay,       

        AddCourse,
        CourseToggled,

        Reset,
    }

    public delegate void OnAction(ReplayAction action, params object[] args);

    public class ReplayControllerAsset : ScriptableObject
    {
        public IntEvent valueChangeEvent;

        public Cirrus.Event OnLabyrinthReplayHandler;

        public OnAction OnActionHandler;

        private float playbackSpeed = 1;

        public float PlaybackSpeed
        {
            get
            {
                return playbackSpeed;
            }

            set
            {
                playbackSpeed = value;
                if (playbackSpeed > Utils.MaxPlaybackSpeed) playbackSpeed = Utils.MinPlaybackSpeed;
                if (playbackSpeed < Utils.MinPlaybackSpeed) playbackSpeed = Utils.MaxPlaybackSpeed;
                if (OnPlaybackSpeedHandler != null)
                {
                    OnPlaybackSpeedHandler.Invoke(playbackSpeed);
                }
            }
        }

        public FloatEvent OnPlaybackSpeedHandler;

        public Cirrus.Event OnSequenceFinishedHandler;

        public IntEvent OnMoveCountSetHandler;

        public IntEvent OnMoveIndexChangedHandler;

        private Course course;

        public OnCourseEvent OnCourseSelectedHandler;

        public Course CurrentCourse
        {
            set
            {
                course = value;
                if (OnCourseSelectedHandler != null)
                    OnCourseSelectedHandler.Invoke(course);
            }

            get
            {
                return course;
            }
        }
                

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
                    if (OnMoveIndexChangedHandler != null) OnMoveIndexChangedHandler.Invoke(moveIndex);
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
