using UnityEngine;
using System.Collections;
using Cirrus.Extensions;
using Cirrus;
using UnityEngine.EventSystems;

namespace UdeS.Promoscience.Labyrinths.UI
{
    /// <summary>
    /// Click on this button to select which level/labyrinth to play
    /// </summary>
    public class LevelButton : BaseButton
    {
        public override void Awake()
        {
            base.Awake();
        }

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
            //GameManager.Instance.CurrentGame.StartRound(
            //    Random.Range(1, Utils.NumLabyrinth + 1),
            //    (int)algorithmSelect.AlgorithmId);
        }
    }
}
