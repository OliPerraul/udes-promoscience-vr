using UnityEngine;
using System.Collections;
using System;
using Cirrus.Extensions;
using Cirrus;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replays
{
    // Preview the replay for a labyrinth via the selection interface (Algorithm only)
    public class PreviewReplay : BaseReplay
    {
        private Level level;

        public Level Level => level;

        public int LevelNumber => level.Number;

        private Labyrinths.LabyrinthObject labyrinthObject;

        public Labyrinths.LabyrinthObject LabyrinthObject => labyrinthObject;

        private Vector2Int lposition;

        private Vector3 wposition;

        private AlgorithmReplay algorithm;

        public Algorithms.Id Algorithm {
            set => algorithm.Algorithm = value;
        }

        public Cirrus.Event OnAlgorithmChangedHandler;

        public Event<PreviewReplay> OnRemovedHandler;

        public Event<PreviewReplay> OnLevelReplayStartedHandler;

        protected override IEnumerable<IReplayWorker> Workers => new List<IReplayWorker>{algorithm};

        private System.Threading.Mutex mutex = new System.Threading.Mutex();

        public override int MoveCount => algorithm.MoveCount;

        private GameReplay parentReplay;

        public void OnParentMoveIndexChanged(int index) => MoveIndex = index;

        public void OnParentPlaybackSpeedChanged(float speed) => PlaybackSpeed = speed;

        public void OnAlgorithmChanged()
        {
            MoveIndex = MoveIndex < MoveCount ? MoveIndex : MoveCount - 1;
            OnAlgorithmChangedHandler?.Invoke();
        }

        //public
        public PreviewReplay(
            Level round,
            GameReplay parentReplay)
        {
            this.level = round;

            this.parentReplay = parentReplay;

            parentReplay.OnMoveIndexChangedHandler += OnParentMoveIndexChanged;
            parentReplay.OnPlaybackSpeedChangedHandler += OnParentPlaybackSpeedChanged;
            parentReplay.OnResumeHandler += OnResume;
            parentReplay.OnStopHandler += OnStop;
        }

        public void Initialize()
        {
            //base.Initialize();

            labyrinthObject = Labyrinths.Resources.Instance
                .GetLabyrinthObject(level.Labyrinth)
                .Create(level.Labyrinth);

            labyrinthObject.GenerateLabyrinthVisual();

            labyrinthObject.Init(enableCamera: true);

            labyrinthObject.Camera.OutputToTexture = true;

            UI.ReplayDisplay.Instance.ViewRawImage.texture = labyrinthObject.Camera.RenderTexture;

            lposition = labyrinthObject.GetLabyrithStartPosition();

            wposition = labyrinthObject.GetLabyrinthPositionInWorldPosition(lposition);

            algorithm =
                Resources.Instance.AlgorithmSequence.Create(
                this,
                labyrinthObject,
                level.Algorithm,
                lposition
                );

            algorithm.OnChangedHandler += OnAlgorithmChanged;
        }


        // TODO combine
        public void Remove()
        {
            parentReplay.OnMoveIndexChangedHandler -= OnParentMoveIndexChanged;
            parentReplay.OnPlaybackSpeedChangedHandler -= OnParentPlaybackSpeedChanged;
            parentReplay.OnResumeHandler -= OnResume;

            if (algorithm != null)
                algorithm.gameObject.Destroy();

            labyrinthObject?.gameObject?.Destroy();
            OnRemovedHandler?.Invoke(this);
        }

        // TODO combine
        public void Clear()
        {
            parentReplay.OnMoveIndexChangedHandler -= OnParentMoveIndexChanged;
            parentReplay.OnPlaybackSpeedChangedHandler -= OnParentPlaybackSpeedChanged;
            parentReplay.OnResumeHandler -= OnResume;
            parentReplay.OnResumeHandler -= OnStop;

            if (algorithm != null)
                algorithm.gameObject.Destroy();

            labyrinthObject?.gameObject?.Destroy();
        }
    }
}
