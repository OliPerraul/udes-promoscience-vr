using UnityEngine;
using System.Collections;
using System;
using Cirrus.Extensions;
using Cirrus;

namespace UdeS.Promoscience.Replays
{
    // Preview the replay for a labyrinth via the selection interface (Algorithm only)
    public class PreviewReplay : BaseReplay
    {
        private Round round;

        public int RoundNumber => round.Number;

        private Labyrinths.LabyrinthObject labyrinthObject;

        public Labyrinths.LabyrinthObject LabyrinthObject => labyrinthObject;

        private Vector2Int lposition;

        private Vector3 wposition;

        private AlgorithmReplay sequence;

        public Event<PreviewReplay> OnRemovedHandler;

        public Event<PreviewReplay> OnRoundReplayStartedHandler;

        private bool isPlaying = false;

        private System.Threading.Mutex mutex = new System.Threading.Mutex();

        //public
        public PreviewReplay(
            ReplayControlsAsset controls,
            Round round) : 
            base(controls)
        {
            this.round = round;
        }

        public void CreateLabyrinth()
        {
            labyrinthObject = Labyrinths.Resources.Instance
                .GetLabyrinthObject(round.Labyrinth)
                .Create(round.Labyrinth);

            labyrinthObject.GenerateLabyrinthVisual();

            labyrinthObject.Init(enableCamera: true);

            labyrinthObject.Camera.OutputToTexture = true;

            UI.ReplayDisplay.Instance.ViewRawImage.texture = labyrinthObject.Camera.RenderTexture;

            lposition = labyrinthObject.GetLabyrithStartPosition();

            wposition = labyrinthObject.GetLabyrinthPositionInWorldPosition(lposition);
        }


        public override void Start()
        {
            CreateLabyrinth();

            sequence =
                Resources.Instance.AlgorithmSequence.Create(
                this,
                labyrinthObject,
                round.Algorithm,
                lposition
                );

            controls.PlaybackSpeed = 2f;
        }


        // TODO combine
        public void Remove()
        {
            labyrinthObject?.gameObject?.Destroy();
            OnRemovedHandler?.Invoke(this);
        }

        // TODO combine
        public override void Clear()
        {
            labyrinthObject?.gameObject?.Destroy();
        }


        public override void OnPlaybackSpeedChanged(float speed)
        {
            sequence.PlaybackSpeed = speed;
        }

        public override void Resume()
        {
            isPlaying = true;
            Server.Instance.StartCoroutine(ResumeCoroutine());
        }

        public IEnumerator ResumeCoroutine()
        {
            isPlaying = true;

            while (HasNext)
            {
                //next in algorithm
                if (sequence.WithinBounds)
                {
                    yield return sequence.StartNextCoroutine();
                    GlobalMoveIndex++;
                }

                yield return null;
            }

            isPlaying = false;
        }

        public override void Next()
        {
            if (isPlaying)
            {
                Pause();
            }

            if (sequence.WithinBounds) sequence.Next();

            GlobalMoveIndex++;
        }

        public override void Previous()
        {
            if (isPlaying)
            {
                Pause();
            }

            GlobalMoveIndex--;

            if (sequence.WithinBounds) sequence.Previous();            
        }

        public override void Move(int target)
        {
            GlobalMoveIndex = target;

            sequence.Move(target);
        }

        public override void Pause()
        {
            if (!isPlaying)
                return;

            sequence.Stop();

            Move(GlobalMoveIndex);

            isPlaying = false;
        }

        public override void Stop()
        {
            sequence.Stop();

            Move(0);

            isPlaying = false;
        }


        public override void OnReplayControlAction(ReplayControlAction action)
        {
            switch (action)
            {
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

                    //mutex.WaitOne();

                    //Stop();

                    //mutex.ReleaseMutex();

                    break;
            }
        }
    }
}
