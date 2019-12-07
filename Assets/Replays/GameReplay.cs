using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays
{
    public class GameReplay : BaseReplay
    {
        public IList<Round> rounds;

        public IList<Round> Rounds => rounds;

        private List<PreviewReplay> previews = new List<PreviewReplay>();

        public int currentReplayindex = 0;

        public int CurrentIndexWrap {
            get {
                return currentReplayindex.Mod(rounds.Count);
            }

            set {
                currentReplayindex = value;
                currentReplayindex = currentReplayindex.Mod(rounds.Count);
            }
        }

        public GameReplay(
            ReplayControlsAsset controls,
            IList<Round> rounds) : base(controls)
        {
            this.rounds = rounds;
        }

        public override void Start()
        {
            base.Start();

            Resume();

            Server.Instance.State.Set(ServerState.GameReplay);
        }


        public override void Clear()
        {
            foreach (var preview in previews)
            {
                preview.Clear();
            }

            previews.Clear();
        }

        public PreviewReplay CreatePreviewReplay()
        {
            previews.Add(new PreviewReplay(
                controls,
                rounds[CurrentIndexWrap++]));

            previews[previews.Count - 1].OnRemovedHandler += OnPreviewRemoved;

            controls.OnPlaybackSpeedHandler += previews[previews.Count - 1].OnPlaybackSpeedChanged;

            controls.OnControlActionHandler += previews[previews.Count - 1].OnReplayControlAction;

            previews[previews.Count - 1].OnRoundReplayStartedHandler += OnRoundReplayStarted;

            previews[previews.Count - 1].Start();

            previews[previews.Count - 1].LabyrinthObject.transform.position = 
                Vector3.right * Labyrinths.Utils.SelectionOffset * (previews.Count - 1);

            return previews[previews.Count - 1];
        }


        public void OnRoundReplayStarted(PreviewReplay preview)
        {
            ReplayManager.Instance.StartRoundReplay();
        }

        public void OnPreviewRemoved(PreviewReplay preview)
        {
            previews.Remove(preview);

            controls.OnPlaybackSpeedHandler += preview.OnPlaybackSpeedChanged;

            controls.OnControlActionHandler += preview.OnReplayControlAction;

            for (int i = 0; i < previews.Count; i++)
            {
                previews[i].LabyrinthObject.transform.position = Vector3.right * Labyrinths.Utils.SelectionOffset * i;
            }

            currentReplayindex = previews.Count;
        }

    }
}
