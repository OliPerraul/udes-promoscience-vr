using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Labyrinths.Editor
{

    public enum EditorState
    {
        Select,
        Editor
    }

    public class EditorController : Cirrus.BaseSingleton<EditorController>
    {
        //[SerializeField]
        //public Tilemap tilemap;

        [SerializeField]
        private Cirrus.SceneWrapperAsset menuScene;

        [SerializeField]
        private string labyrinthPath;

        [SerializeField]
        private UnityEngine.UI.Button exitButton;

        public Cirrus.ObservableValue<EditorState> State = new Cirrus.ObservableValue<EditorState>();

        public LabyrinthObject LabyrinthObject;

        public ILabyrinth Labyrinth;

        //[SerializeField]
        //public EditorDrawer Drawer;

        // Use this for initialization

        public void Awake()
        {
            State.OnValueChangedHandler += OnEditorState;

            EditorDrawer.Instance.OnTileDrawnHandler += OnTileDrawn;
            UI.EditorInterface.Instance.OnSaveHandler += OnSave;
            UI.EditorInterface.Instance.OnTrashHandler += OnTrash;


            EditorButtonManager.Instance.OnClickHandler += OnButtonClick;
        }

        public void Start()
        {
            State.Set(EditorState.Select);
        }


        public void OnButtonClick(EditorButtonFlag button)
        {
            switch (button)
            {
                case EditorButtonFlag.BottomExit:

                    if (State.Value == EditorState.Editor)
                    {
                        State.Value = EditorState.Select;
                    }
                    else if (State.Value == EditorState.Select)
                    {
                        menuScene.Load();
                    }

                    break;
            }
        }


        public void OnTileDrawn(Vector2Int pos, TileType tile)
        {
            LabyrinthObject.SetLabyrithValueAt(pos, tile);
        }

        public void OnEditorState(EditorState state)
        {
            switch (state)
            {
                case EditorState.Editor:
                    break;

                case EditorState.Select:

                    if (LabyrinthObject != null)
                    {
                        LabyrinthObject.gameObject.Destroy();
                        LabyrinthObject = null;
                    }

                    Labyrinth = null;
                    break;
            }
        }

        public void OnSave()
        {
            Debug.Log(Labyrinth.Name + " saved!");

            Labyrinth.PopulateStartAndEndPositions();

            // Save session
            //TODO improve
            var sessionLab = Server.Instance.GetLabyrinth(Labyrinth.Id);
            var idx = Server.Instance.Labyrinths.IndexOf(sessionLab);// Instance.GetLabyrinth(Labyrinth.Id);
            Server.Instance.Labyrinths[idx] = Labyrinth;

            // Save persistent
            SQLiteUtilities.UpdateLabyrinth(Labyrinth);
        }

        public void OnTrash()
        {

        }

    }
}
