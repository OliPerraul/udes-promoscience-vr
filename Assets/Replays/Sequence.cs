﻿using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Replays;
using System.Threading;

public interface ISequence
{
    //void HandleAction(ReplayAction action, params object[] args);
}

namespace UdeS.Promoscience.Replays
{

    public abstract class Sequence : MonoBehaviour
    {
        //[SerializeField]
        protected ReplayControllerAsset replay;

        protected Labyrinths.Labyrinth labyrinth;

        protected bool isPlaying = false;

        public bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
        }

        [SerializeField]
        protected float stepTime = 0.6f;

        protected float modifier = 1;

        public float PlaybackSpeed
        {
            set
            {
                modifier = Mathf.Clamp(value, Utils.MinPlaybackSpeed, Utils.MaxPlaybackSpeed);
            }
        }

        protected float StepTime
        {
            get
            {
                return stepTime / modifier;
            }
        }


        protected Mutex mutex;

        protected Vector2Int lposition;

        protected Vector2Int startlposition;

        protected Coroutine coroutineResult;

        public Coroutine NextCoroutineResult
        {
            get
            {
                return coroutineResult;
            }
        }

        protected abstract bool HasPrevious { get; }

        protected abstract bool HasNext { get; }

        public virtual bool WithinBounds
        {
            get
            {
                return replay.GlobalMoveIndex < LocalMoveCount;
            }
        }

        public abstract int LocalMoveCount { get; }

        public abstract int LocalMoveIndex { get; }

        //protected abstract void Move(int target);

        protected abstract void DoPrevious();

        protected abstract void DoNext();

        //public abstract void Resume();


        public virtual void Awake()
        {
            mutex = new Mutex();
        }


        // TODO: remove debug
        public virtual void OnValidate()
        {
            
        }

        public virtual void FixedUpdate()
        {

        }


        protected abstract IEnumerator DoNextCoroutine();


        public virtual void StartNextCoroutine()
        {
            coroutineResult = StartCoroutine(DoNextCoroutine());
        }

        public virtual void Next()
        {
            Stop();

            if (HasNext)
            {
                DoNext();
            }
        }

        public virtual void Previous()
        {
            if (HasPrevious)
            {
                DoPrevious();
            }
        }

        public virtual void Stop()
        {
            if (coroutineResult != null)
            {
                StopCoroutine(coroutineResult);
                coroutineResult = null;
            }
        }

        public virtual void AdjustOffset(float ofst)
        {

        }

        public void Move(int target)
        {
            if (target == LocalMoveIndex)
                return;

            if (Mathf.Sign(LocalMoveIndex - target) < 0)
            {
                while (HasNext)
                {
                    if (LocalMoveIndex == target)
                        return;

                    Next();
                }
            }
            else
            {
                while (HasPrevious)
                {
                    Previous();

                    if (LocalMoveIndex == target)
                        return;
                }
            }
        }
    }
}

