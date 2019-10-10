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

        protected Vector2Int lposition;

        protected Vector2Int startlposition;

        protected Coroutine coroutineResult;

        public Coroutine ResumeCoroutineResult
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
                return replayOptions.GlobalMoveIndex < LocalMoveCount;
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

        public virtual void Resume()
        {
            coroutineResult = StartCoroutine(ResumeCoroutine());
        }


        public IEnumerator ResumeCoroutine()
        {
            isPlaying = true;

            while (HasPrevious)
            {
                yield return StartCoroutine(DoNextCoroutine());
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
                    if (LocalMoveIndex == target)
                        return;

                    Previous();       
                }
            }
        }
    }
}

