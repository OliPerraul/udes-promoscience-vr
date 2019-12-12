using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;
using System.Linq;
using Cirrus;
using Event = Cirrus.Event;

namespace UdeS.Promoscience.Replays
{
    public abstract class ControlReplay : BaseReplay
    {
        protected ReplayControlsAsset controls;

        private System.Threading.Mutex mutex = new System.Threading.Mutex();
        
        public override int MoveCount => controls.ReplayMoveCount.Value;

        protected override float PlaybackSpeed => controls.PlaybackSpeed;

        public override Event<float> OnPlaybackSpeedChangedHandler
        {
            get => controls.OnPlaybackSpeedHandler;
            set { }
        }

        // TODO remove
        public ControlReplay(ReplayControlsAsset controls)
        {
            this.controls = controls;

            controls.OnControlActionHandler += OnReplayControlAction;
            controls.SlideValue.OnValueChangedHandler += OnSlideValueChanged;

            OnMoveIndexChangedHandler += (x) => controls?.ReplayMoveIndexChangedHandler(x);

        }

        public virtual void Clear()
        {
            controls.OnControlActionHandler -= OnReplayControlAction;
            controls.SlideValue.OnValueChangedHandler -= OnSlideValueChanged;
        }


        public virtual void Initialize()
        {

        }


        public virtual void Start()
        {

        }


        public void OnSlideValueChanged(int target)
        {
            mutex.WaitOne();

            if (target == MoveIndex)
                return;

            if (Mathf.Sign(MoveIndex - target) < 0)
            {
                while (HasNext)
                {
                    if (MoveIndex == target)
                        return;

                    Next();
                }
            }
            else
            {
                while (HasPrevious)
                {
                    Previous();

                    if (MoveIndex == target)
                        return;
                }
            }

            mutex.ReleaseMutex();
        }

        private void OnReplayControlAction(ReplayControlAction action)
        {
            switch (action)
            {
                case ReplayControlAction.Play:
                    mutex.WaitOne();
                    Resume();
                    mutex.ReleaseMutex();
                    break;

                case ReplayControlAction.Resume:
                    mutex.WaitOne();
                    Resume();
                    mutex.ReleaseMutex();
                    break;

                case ReplayControlAction.Pause:

                    mutex.WaitOne();

                    Pause();

                    mutex.ReleaseMutex();

                    break;

                case ReplayControlAction.Next:

                    mutex.WaitOne();

                    Next();

                    mutex.ReleaseMutex();

                    break;

                case ReplayControlAction.Previous:

                    mutex.WaitOne();

                    Previous();

                    mutex.ReleaseMutex();

                    break;

                case ReplayControlAction.Stop:

                    mutex.WaitOne();

                    Stop();

                    mutex.ReleaseMutex();

                    break;
            }
        }


        public void Next()
        {
            MoveIndex++;
        }

        public void Previous()
        {
            MoveIndex--;
        }

        public void Pause()
        {
            // TODO
            Stop();
        }

        //public virtual void Stop()
        //{

        //}
    }
}