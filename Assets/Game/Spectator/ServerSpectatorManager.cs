using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cirrus.Extensions;

namespace UdeS.Promoscience
{
    /// <summary>
    /// Server side spectator view
    /// CUrrently just displays the currently played labyrinth
    /// It would be cool to display the player movement in real time
    /// </summary>
    public class ServerSpectatorManager : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Text roundText;

        [SerializeField]
        private LocalizeInlineString quickPlayString = new LocalizeInlineString("Quickplay", "Partie rapide");

        [SerializeField]
        private LocalizeInlineString roundString = new LocalizeInlineString("Level ", "Niveau ");

        private Labyrinths.LabyrinthObject labyrinth;

        public void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnServerStateChanged;
        }

        public void OnDestroy()
        {
            if (Server.Instance != null) Server.Instance.State.OnValueChangedHandler -= OnServerStateChanged;
        }

        public void OnServerStateChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.Quickplay:
                case ServerState.Level:

                    if (labyrinth != null)
                    {
                        labyrinth.gameObject.Destroy();
                        labyrinth = null;
                    }

                    labyrinth = Labyrinths.Resources.Instance
                        .GetLabyrinthObject(GameManager.Instance.CurrentGame.CurrentLevel.Labyrinth)
                        .Create(GameManager.Instance.CurrentGame.CurrentLevel.Labyrinth);

                    labyrinth.GenerateLabyrinthVisual();

                    labyrinth.Init(enableCamera: true);

                    labyrinth.Camera.OutputToTexture = false;

                    roundText.gameObject.SetActive(true);

                    roundText.text =
                        GameManager.Instance.CurrentGame.LevelState == ServerState.Quickplay ?
                            quickPlayString.Value :
                            roundString.Value + (GameManager.Instance.CurrentGame.CurrentLevel.Number + 1).ToString();

                    break;

                case ServerState.Menu:
                    break;

                default:
                    roundText.gameObject.SetActive(false);

                    if (labyrinth != null)
                    {
                        labyrinth.gameObject.Destroy();
                        labyrinth = null;
                    }



                    break;
            }
        }





    }
}