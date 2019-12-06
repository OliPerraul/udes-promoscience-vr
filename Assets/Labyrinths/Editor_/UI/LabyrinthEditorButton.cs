using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Labyrinths.UI;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Labyrinths.Editor.UI
{
    public class LabyrinthEditorButton : BaseButton
    {
        [SerializeField]
        private ControllerAsset controller;

        public override BaseButton Create(
            Transform parent,
            LabyrinthObject labyrinth)
        {
            var l = this.Create(parent);
            l.labyrinth = labyrinth;
            l.rawImage.texture = labyrinth.Camera.RenderTexture;
            return l;
        }

        public override void OnClick()
        {
            controller.State.Set(State.Editor);
        }
    }
}
