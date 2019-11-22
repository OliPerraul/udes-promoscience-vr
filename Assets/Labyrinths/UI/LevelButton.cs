using UnityEngine;
using System.Collections;
using Cirrus.Extensions;
using Cirrus;
using UnityEngine.EventSystems;

namespace UdeS.Promoscience.Labyrinths.UI
{
    public class LevelButton : BaseButton
    {
        public override void Awake()
        {
            base.Awake();
        }

        public override BaseButton Create(
            Transform parent,
            Labyrinth labyrinth)
        {
            var l = this.Create(parent);
            l.labyrinth = labyrinth;
            l.rawImage.texture = labyrinth.Camera.RenderTexture;
            return l;
        }

        public override void OnClick()
        {
            Server.Instance.StartGameWithLabyrinth(labyrinth.Id);
        }
    }
}
