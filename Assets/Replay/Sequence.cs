using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Replay;
using System.Threading;

public interface ISequence
{
    //void HandleAction(ReplayAction action, params object[] args);
}

namespace UdeS.Promoscience.Replay
{


    public abstract class Sequence : MonoBehaviour
    {
        [SerializeField]
        protected ScriptableReplayOptions replayOptions;

        protected Labyrinth labyrinth;

        protected bool isPlaying = false;

        public bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
        }

        [SerializeField]
        protected float speed = 0.6f;

        protected Mutex mutex;

        protected Vector2Int labyrinthPosition;

        protected Coroutine coroutineResult;

        public Coroutine ResumeCoroutineResult
        {
            get
            {
                return coroutineResult;
            }
        }

        protected bool HasNext
        {
            get
            {
                if (MoveCount == 0)
                    return false;

                return moveIndex < MoveCount;
            }
        }

        protected bool HasPrevious
        {
            get
            {
                if (MoveCount == 0)
                    return false;

                return moveIndex > 0;
            }
        }

        public abstract int MoveCount { get; }


        protected int moveIndex;


        protected abstract void Move(int target);

        protected abstract void Reverse();

        public void DecreaseIndex()
        {
            moveIndex--;

            if (replayOptions.OnProgressHandler != null)
                replayOptions.OnProgressHandler.Invoke(moveIndex);
        }

        public void IncreaseIndex()
        {
            moveIndex++;
            if (replayOptions.OnProgressHandler != null)
                replayOptions.OnProgressHandler.Invoke(moveIndex);
        }

        protected abstract void Perform();

        public abstract void Play();

        public abstract void Stop();

        public abstract void Pause();

        //public abstract void Resume();


        public virtual void Awake()
        {
            mutex = new Mutex();

            replayOptions.OnActionHandler += OnReplayAction;
        }


        // TODO: remove debug
        public virtual void OnValidate()
        {
            
        }

        public virtual void FixedUpdate()
        {

        }


        protected abstract IEnumerator PerformCoroutine();

        public virtual void Resume()
        {
            coroutineResult = StartCoroutine(ResumeCoroutine());
        }


        public IEnumerator ResumeCoroutine()
        {
            isPlaying = true;

            while (HasNext)
            {
                yield return StartCoroutine(PerformCoroutine());
                IncreaseIndex();
                yield return null;
            }

            isPlaying = false;

            if (replayOptions.OnSequenceFinishedHandler != null)
            {
                replayOptions.OnSequenceFinishedHandler.Invoke();
            }
        }

        public virtual void OnReplayAction(ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case ReplayAction.Previous:

                    mutex.WaitOne();

                    if (isPlaying)
                    {
                        Pause();
                    }

                    if (HasPrevious)
                    {
                        Reverse();
                        DecreaseIndex();
                    }

                    mutex.ReleaseMutex();

                    break;

                case ReplayAction.Next:

                    mutex.WaitOne();

                    if (isPlaying)
                    {
                        Pause();
                    }

                    if (HasNext)
                    {
                        Perform();
                    }

                    // Allow overflow to maintain synch with other sequ
                    IncreaseIndex();

                    mutex.ReleaseMutex();

                    break;

                case ReplayAction.Slide:

                    mutex.WaitOne();

                    int current = (int)args[0];
                    Move(current);

                    mutex.ReleaseMutex();

                    break;

                //case ReplayAction.Play:
                    
                //    Move(0);
                //    Resume();

                //    break;

                //case ReplayAction.Resume:
                //    Resume();

                //    break;

                //case ReplayAction.Pause:

                //    mutex.WaitOne();

                //    Pause();

                //    mutex.ReleaseMutex();

                //    break;

                case ReplayAction.Stop:

                    mutex.WaitOne();

                    Stop();

                    mutex.ReleaseMutex();

                    break;
            }
        }
    }
}

