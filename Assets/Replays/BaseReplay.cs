using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;
using System.Linq;
using Cirrus;

namespace UdeS.Promoscience.Replays
{
    public abstract class BaseReplay
    {
        protected ReplayControlsAsset controls;

        public Event<int> OnMoveCountSetHandler;

        public Event<int> OnMoveIndexChangedHandler;

        private int index = 0;

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


        private bool isPlaying = false;

        // TODO remove
        public BaseReplay(ReplayControlsAsset controls)
        {
            this.controls = controls;
            controls.OnControlActionHandler += OnReplayControlAction;
            controls.OnPlaybackSpeedHandler += OnPlaybackSpeedChanged;
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


        public virtual void OnReplayControlAction(ReplayControlAction action)
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
    }
}