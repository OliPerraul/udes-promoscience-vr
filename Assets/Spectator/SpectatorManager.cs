using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cirrus.Extensions;

namespace UdeS.Promoscience
{
    public class SpectatorManager : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Text roundText;

        [SerializeField]
        private LocalizeInlineString quickPlayString = new LocalizeInlineString("Quickplay", "Partie rapide");

        [SerializeField]
        private LocalizeInlineString roundString = new LocalizeInlineString("Round ", "Niveau ");

        private Labyrinths.LabyrinthObject labyrinth;

        public void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnServerStateChanged;
            //GameManager.Instance.OnGameCreatedHandler += OnGameStarted;
            //GameManager.Instance.OnGameEndedHandler += OnGameEnded;
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
                case ServerState.Round:

                    if (labyrinth != null)
                    {
                        labyrinth.gameObject.Destroy();
                        labyrinth = null;
                    }

                    labyrinth = Labyrinths.Resources.Instance
                        .GetLabyrinthObject(GameManager.Instance.CurrentGame.CurrentRound.Labyrinth)
                        .Create(GameManager.Instance.CurrentGame.CurrentRound.Labyrinth);

                    labyrinth.GenerateLabyrinthVisual();

                    labyrinth.Init(enableCamera: true);

                    labyrinth.Camera.OutputToTexture = false;

                    roundText.gameObject.SetActive(true);

                    roundText.text =
                        GameManager.Instance.CurrentGame.RoundState == ServerState.Quickplay ?
                            quickPlayString.Value :
                            roundString.Value + (GameManager.Instance.CurrentGame.CurrentRound.Number + 1).ToString();

                    break;

                case ServerState.Menu:
                    break;

                default:
                    //roundText.gameObject.SetActive(false);

                    //if (labyrinth != null)
                    //{
                    //    labyrinth.gameObject.Destroy();
                    //    labyrinth = null;
                    //}



                    break;
            }
        }





    }
}