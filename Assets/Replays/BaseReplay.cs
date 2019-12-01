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



        public virtual void AddCourse(Course course, bool added)
        {
     
        }

        public virtual void OnCourseRemoved(Course course)
        {

        }
    }
}