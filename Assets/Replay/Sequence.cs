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

        protected bool IsWithinRange
        {
            get
            {
                if (LocalMoveCount == 0)
                    return false;

                return replayOptions.GlobalMoveIndex >= 0 && replayOptions.GlobalMoveIndex < LocalMoveCount;
            }
        }

        public abstract int LocalMoveCount { get; }


        //protected abstract void Move(int target);

        protected abstract void Reverse();

        protected abstract void Perform();

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


        protected abstract IEnumerator PerformCoroutine();

        public virtual void Resume()
        {
            coroutineResult = StartCoroutine(ResumeCoroutine());
        }


        public IEnumerator ResumeCoroutine()
        {
            isPlaying = true;

            while (IsWithinRange)
            {
                yield return StartCoroutine(PerformCoroutine());
                yield return null;
            }

            isPlaying = false;

            if (replayOptions.OnSequenceFinishedHandler != null)
            {
                replayOptions.OnSequenceFinishedHandler.Invoke();
            }
        }

        public virtual void Next()
        {
            //if (isPlaying)
            //{
            //    Pause();
            //}

            if (IsWithinRange)
            {
                Perform();
            }
        }



        public virtual void Previous()
        {
            //if (isPlaying)
            //{
            //    Pause();
            //}

            if (IsWithinRange)
            {
                Reverse();
            }
        }
    }
}

