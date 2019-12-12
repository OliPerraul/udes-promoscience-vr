using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus;

namespace UdeS.Promoscience.Replays
{
    public interface IReplayWorker
    {
        Coroutine ResumeCoroutineResult { get; }

    }

    public abstract class BaseReplay : IReplayWorker
    {
        public abstract int MoveCount { get;}

        public Event<int> OnMoveIndexChangedHandler;

        public virtual Event<float> OnPlaybackSpeedChangedHandler { get; set; }

        public bool HasNext => MoveCount == 0 ? false : MoveIndex < MoveCount;

        public bool HasPrevious => MoveCount == 0 ? false : MoveIndex > 0;

        public Coroutine ResumeCoroutineResult => resumeCoroutineResult;

        protected Coroutine resumeCoroutineResult;

        protected abstract IEnumerable<IReplayWorker> Workers { get; }

        private bool isPlaying = false;

        public Cirrus.Event OnResumeHandler;

        public Cirrus.Event OnStopHandler;

        private float playbackSpeed;

        protected virtual float PlaybackSpeed
        {
            get => playbackSpeed;

            set {
                playbackSpeed = value;
                OnPlaybackSpeedChangedHandler?.Invoke(playbackSpeed);
            }
        }

        private int moveIndex = 0;

        public virtual int MoveIndex
        {
            get => moveIndex;

            protected set
            {
                moveIndex = value < MoveCount ? value : MoveCount;
                moveIndex = moveIndex < 0 ? 0 : moveIndex;
                OnMoveIndexChangedHandler?.Invoke(moveIndex);
            }
        }

        public void OnStop()
        {
            Resume();
        }

        public virtual void Stop()
        {
            // TODO distinguish stop and pause
            MoveIndex = 0;

            if (resumeCoroutineResult != null)
            {
                ReplayManager.Instance.StopCoroutine(resumeCoroutineResult);
                resumeCoroutineResult = null;
            }

            OnStopHandler?.Invoke();
        }


        public void OnResume()
        {
            Resume();
        }

        public virtual void Resume()
        {
            resumeCoroutineResult = ReplayManager.Instance.StartCoroutine(ResumeCoroutine());
        }

        public virtual IEnumerator ResumeCoroutine()
        {
            while (HasNext)
            {
                OnResumeHandler?.Invoke();

                // next in player sequence
                foreach (var worker in Workers)
                {
                    yield return worker.ResumeCoroutineResult;
                }

                yield return null;
            }
        }
    }
}