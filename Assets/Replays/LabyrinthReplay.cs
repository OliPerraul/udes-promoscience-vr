using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Replays;
using System.Threading;

public interface ISequence
{
    //void HandleAction(ReplayAction action, params object[] args);
}

namespace UdeS.Promoscience.Replays
{
    // TODO do not derive mono behaviour
    public abstract class LabyrinthReplay : MonoBehaviour, IReplayWorker
    {
        ////[SerializeField]
        protected abstract BaseReplay Parent { get; }

        protected Labyrinths.LabyrinthObject labyrinth;

        protected bool isPlaying = false;

        public bool IsPlaying => isPlaying;

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

        protected float StepTime => stepTime / modifier;

        protected Mutex mutex;

        protected Vector2Int lposition;

        protected Vector2Int startlposition;

        protected Coroutine resumeCoroutineResult;

        public Coroutine ResumeCoroutineResult => resumeCoroutineResult;

        //public Cirrus.Event<int> OnMoveIndexChangedHandler;

        protected abstract bool HasPrevious { get; }

        protected abstract bool HasNext { get; }

        public abstract int MoveCount { get; }

        public abstract int MoveIndex { get; }

        protected abstract void DoPrevious();

        protected abstract void DoNext();

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


        protected virtual IEnumerator DoNextCoroutine() { yield return null; }


        public virtual Coroutine StartNextCoroutine()
        {
            return null;
            //coroutineResult = StartCoroutine(DoNextCoroutine());
            //return coroutineResult;
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
            //if (coroutineResult != null)
            //{
            //    StopCoroutine(coroutineResult);
            //    coroutineResult = null;
            //}
        }

        public virtual void AdjustOffset(float ofst)
        {

        }

        public void Move(int target)
        {
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
        }
    }
}

