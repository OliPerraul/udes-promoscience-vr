using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;
using System.Linq;

namespace UdeS.Promoscience.Replays
{
    public class GameReplay : ControlReplay
    {
        public IList<Level> levels;

        public IList<Level> Levels => levels;

        private List<PreviewReplay> previews = new List<PreviewReplay>();

        public IList<PreviewReplay> Previews => previews;

        protected override IEnumerable<IReplayWorker> Workers => Previews;

        public GameReplay(
            ReplayControlsAsset controls,
            IList<Level> levels) : base(controls)
        {
            this.levels = levels;
        }

        public override void Initialize()
        {
            for (int i = 0; i < levels.Count; i++)
            {
                CreatePreviewReplay();
            }            
        }

        public override void Start()
        {
            Server.Instance.State.Set(ServerState.GameReplay);
        }

        public void OnLevelReplayStarted(PreviewReplay preview)
        {
            ReplayManager.Instance.StartLevelReplay(preview.Level);
        }

        public override void Clear()
        {
            foreach (var preview in previews)
            {
                preview.Clear();
            }

            previews.Clear();
        }

        public void SetMoveCount()
        {
            controls.ReplayMoveCount.Value = previews.Count == 0 ? 0 : previews.Max(x => x.MoveCount);
        }

        public PreviewReplay CreatePreviewReplay()
        {
            previews.Add(new PreviewReplay(
                levels[previews.Count.Mod(levels.Count)],
                this));

            previews[previews.Count - 1].Initialize();

            previews[previews.Count - 1].OnRemovedHandler += OnPreviewRemoved;

            previews[previews.Count - 1].OnLevelReplayStartedHandler += OnLevelReplayStarted;

            previews[previews.Count - 1].OnAlgorithmChangedHandler += () => SetMoveCount();

            previews[previews.Count - 1].LabyrinthObject.transform.position = 
                Vector3.right * Labyrinths.Utils.SelectionOffset * (previews.Count - 1);

            previews[previews.Count - 1].OnParentMoveIndexChanged(MoveIndex);

            // TODO: do not do this for every lab at when init
            SetMoveCount();

            return previews[previews.Count - 1];
        }

        public void OnPreviewRemoved(PreviewReplay preview)
        {
            previews.Remove(preview);

            //controls.OnPlaybackSpeedHandler -= preview.OnPlaybackSpeedChanged;

            //controls.OnControlActionHandler -= preview.OnReplayControlAction;

            for (int i = 0; i < previews.Count; i++)
            {
                previews[i].LabyrinthObject.transform.position = Vector3.right * Labyrinths.Utils.SelectionOffset * i;
            }

            SetMoveCount();
        }


        public override void Stop()
        {
            base.Stop();
        }


    }
}
