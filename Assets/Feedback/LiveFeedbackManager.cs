using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cirrus.Extensions;

namespace UdeS.Promoscience
{
    public class LiveFeedbackManager : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Text roundText;

        [SerializeField]
        private LocalizeInlineString quickPlayString = new LocalizeInlineString("Quickplay", "Partie rapide");

        [SerializeField]
        private LocalizeInlineString roundString = new LocalizeInlineString("Round ", "Niveau ");


        private Labyrinths.Labyrinth labyrinth;

        private Game game;


        public void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnServerStateChanged;
            GameManager.Instance.OnGameCreatedHandler += OnGameStarted;
            GameManager.Instance.OnGameEndedHandler += OnGameEnded;
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


        public void OnGameStarted(Game game)
        {
            this.game = game;

            game.OnRoundStartedHandler += OnRoundStarted;

        }

        public void OnGameEnded(Game game)
        {
            game.OnRoundStartedHandler -= OnRoundStarted;
            //game.OnRoundE

        }


        public void OnRoundStarted(Round round)
        {
            if (labyrinth != null)
            {
                labyrinth.gameObject.Destroy();
                labyrinth = null;
            }

            labyrinth = Labyrinths.Resources.Instance
                .GetLabyrinthTemplate(round.Labyrinth)
                .Create(round.Labyrinth);

            labyrinth.GenerateLabyrinthVisual();

            labyrinth.Init(enableCamera: true);

            labyrinth.Camera.OutputToTexture = false;

            roundText.gameObject.SetActive(true);

            roundText.text =
                game.RoundState == ServerState.Quickplay ?
                    quickPlayString.Value :
                    roundString.Value + (round.Number + 1).ToString();
        }



    }
}