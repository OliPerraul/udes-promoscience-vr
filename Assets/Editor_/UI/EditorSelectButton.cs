using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Labyrinths.UI;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Labyrinths.Editor.UI
{
    public class EditorSelectButton : BaseButton
    {
        //[SerializeField]
        //private ControllerAsset controller;

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
            EditorController.Instance.State.Set(EditorState.Editor);

            // TODO do not mix business logic here
            // TODO copy a smarter way
            EditorController.Instance.Labyrinth = JsonUtility.FromJson<Labyrinth>(labyrinth.Data.Json);
            
            EditorController.Instance.LabyrinthObject = Labyrinths.Resources.Instance
                .GetLabyrinthObject(EditorController.Instance.Labyrinth)
                .Create(EditorController.Instance.Labyrinth);

            EditorController.Instance.LabyrinthObject.GenerateLabyrinthVisual();

            EditorController.Instance.LabyrinthObject.gameObject.SetActive(true);

            EditorController.Instance.LabyrinthObject.Init(enableCamera: true);

            EditorController.Instance.LabyrinthObject.Camera.OutputToTexture = false;
        }
    }
}
