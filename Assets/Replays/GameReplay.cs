using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;
using System.Linq;

namespace UdeS.Promoscience.Replays
{
    public class GameReplay : ControlReplay
    {
        public IList<Round> rounds;

        public IList<Round> Rounds => rounds;

        private List<PreviewReplay> previews = new List<PreviewReplay>();

        public IList<PreviewReplay> Previews => previews;

        protected override IEnumerable<IReplayWorker> Workers => Previews;

        public GameReplay(
            ReplayControlsAsset controls,
            IList<Round> rounds) : base(controls)
        {
            this.rounds = rounds;
        }

        public override void Initialize()
        {
            for (int i = 0; i < rounds.Count; i++)
            {
                CreatePreviewReplay();
            }

            
        }

        public override void Start()
        {
            Server.Instance.State.Set(ServerState.GameReplay);
        }

        public void OnRoundReplayStarted(PreviewReplay preview)
        {
            ReplayManager.Instance.StartRoundReplay(preview.Round);
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
                rounds[previews.Count.Mod(rounds.Count)],
                this));

            previews[previews.Count - 1].Initialize();

            previews[previews.Count - 1].OnRemovedHandler += OnPreviewRemoved;

            previews[previews.Count - 1].OnRoundReplayStartedHandler += OnRoundReplayStarted;

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



    }
}
