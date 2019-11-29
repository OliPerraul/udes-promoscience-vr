using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;
using System.Linq;

namespace UdeS.Promoscience.Replays
{
    public abstract class BaseReplay
    {
        //public Replay replay;
        protected ReplayManagerAsset controller;

        private bool isPlaying = false;

        // TODO remove
        public BaseReplay(
            ReplayManagerAsset controller)       
        {
            this.controller = controller;// replay;
            controller.OnActionHandler += OnReplayAction;
            controller.OnPlaybackSpeedHandler += OnPlaybackSpeedChanged;
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


        public virtual void OnReplayAction(ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case ReplayAction.ToggleOptions:                    
                    break;

                case ReplayAction.ExitReplay:
                    Clear();
                    break;

                // TODO: Handle play/ stop from replay object and not sequences
                // to prevent synch issues
                case ReplayAction.ToggleAlgorithm:
                    break;

                case ReplayAction.ToggleGreyboxLabyrinth:
                    //EnableGreybox(!isGreyboxToggled);
                    break;

                case ReplayAction.Play:
                    Resume();
                    break;

                case ReplayAction.Resume:
                    Resume();
                    break;

                case ReplayAction.Pause:

                    //mutex.WaitOne();

                    //Pause();

                    //mutex.ReleaseMutex();

                    break;

                case ReplayAction.Slide:



                    break;


                case ReplayAction.Next:


                    break;

                case ReplayAction.Previous:


                    break;

                case ReplayAction.Stop:

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



        public void OnCourseAdded(Course course)
        {
     
        }

        public void OnCourseRemoved(Course course)
        {

        }
    }
}