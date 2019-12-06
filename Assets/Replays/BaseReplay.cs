using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;
using System.Linq;
using Cirrus;

namespace UdeS.Promoscience.Replays
{
    public enum ReplayControlAction
    {
        Previous,
        Next,
        Play,
        Stop,
        Pause,
        Resume,
        Slide,

        Reset,
    }


    public abstract class BaseReplay
    {
        public Event<int> valueChangeEvent;

        public Cirrus.Event OnLabyrinthReplayHandler;

        public ArgsEvent<ReplayControlAction> OnActionHandler;

        private float playbackSpeed = 1;
  
        public Event<Course, bool> OnCourseAddedHandler;

        public Event<Course, bool> OnCourseToggledHandler;

        public ObservableValue<bool> IsToggleAlgorithm = new ObservableValue<bool>();

        public ObservableValue<bool> IsToggleGreyboxLabyrinth = new ObservableValue<bool>();

        public Event<float> OnPlaybackSpeedHandler;

        public Cirrus.Event OnSequenceFinishedHandler;

        public Event<int> OnMoveCountSetHandler;

        public Event<int> OnMoveIndexChangedHandler;

        public ObservableValue<Course> CurrentCourse = new ObservableValue<Course>();

        private int index = 0;


        public float PlaybackSpeed
        {
            get => playbackSpeed;

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
                if (moveCount != value)
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

        public void SendAction(ReplayControlAction action, params object[] args)
        {
            if (OnActionHandler != null)
            {
                OnActionHandler?.Invoke(action, args);
            }
        }

        private bool isPlaying = false;

        // TODO remove
        public BaseReplay()
        {
            OnActionHandler += OnReplayAction;
            OnPlaybackSpeedHandler += OnPlaybackSpeedChanged;
        }

        public virtual void Start()
        {

        }

        public virtual void OnPlaybackSpeedChanged(float speed)
        {

        }

        public virtual void Resume()
        {

        }

        public virtual void Next()
        {

        }

        public virtual void Previous()
        {

        }

        public virtual void Move(int target)
        {

        }

        public virtual void Pause()
        {

        }

        public virtual void Stop()
        {

        }


        public virtual void OnReplayAction(ReplayControlAction action, params object[] args)
        {
            switch (action)
            {
                //case ReplayAction.ToggleOptions:                    
                //    break;

                ////case ReplayAction.ExitReplay:
                ////    Clear();
                ////    break;

                //// TODO: Handle play/ stop from replay object and not sequences
                //// to prevent synch issues
                //case ReplayAction.ToggleAlgorithm:
                //    break;

                //case ReplayAction.ToggleGreyboxLabyrinth:
                //    //EnableGreybox(!isGreyboxToggled);
                //    break;

                case ReplayControlAction.Play:
                    Resume();
                    break;

                case ReplayControlAction.Resume:
                    Resume();
                    break;

                case ReplayControlAction.Pause:

                    //mutex.WaitOne();

                    //Pause();

                    //mutex.ReleaseMutex();

                    break;

                case ReplayControlAction.Slide:



                    break;


                case ReplayControlAction.Next:


                    break;

                case ReplayControlAction.Previous:


                    break;

                case ReplayControlAction.Stop:

                    //mutex.WaitOne();

                    //Stop();

                    //mutex.ReleaseMutex();

                    break;


            }
        }



        protected virtual void OnSequenceFinished()
        {

        }

        public virtual void Clear()
        {


            //labyrinth = null;         
        }



        public virtual void AddCourse(Course course)
        {
     
        }

        public virtual void RemoveCourse(Course course)
        {

        }
    }
}