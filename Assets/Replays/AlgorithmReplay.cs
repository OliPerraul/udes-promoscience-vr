using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Replays
{
    public class AlgorithmReplay : BaseReplay
    {
        private Round round;

        private Labyrinths.LabyrinthObject labyrinthObject;

        public Labyrinths.LabyrinthObject LabyrinthObject => labyrinthObject;

        private Vector2Int lposition;

        private Vector3 wposition;

        private AlgorithmSequence sequence;

        //public
        public AlgorithmReplay(
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
    }
}
