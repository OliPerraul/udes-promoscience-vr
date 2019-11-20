using UnityEngine;
using System.Collections;
using Cirrus.Extensions;
using Cirrus;
using UnityEngine.EventSystems;

namespace UdeS.Promoscience.Labyrinths.UI
{
    public class LabyrinthButton : BaseLabyrinthButton
    {
        public virtual LabyrinthButton Create(
            Transform parent,
            Labyrinth labyrinth)
        {
            var l = this.Create(parent);
            l.labyrinth = labyrinth;
            l.rawImage.texture = labyrinth.Camera.RenderTexture;
            return l;
        }
    }
}
