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



        public void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnServerStateChanged;
        }

        private Labyrinths.Labyrinth labyrinth;


        public void OnServerStateChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.Quickplay:
                case ServerState.Round:
                    labyrinth = Labyrinths.Resources.Instance
                        .GetLabyrinthTemplate(GameManager.Instance.CurrentGame.CurrentLabyrinth)
                        .Create(GameManager.Instance.CurrentGame.CurrentLabyrinth);

                    labyrinth.GenerateLabyrinthVisual();

                    labyrinth.Init(enableCamera: true);

                    labyrinth.Camera.OutputToTexture = false;

                    roundText.text = 
                        Server.Instance.State.Value == ServerState.Quickplay ? 
                            quickPlayString.Value : 
                            roundString.Value + GameManager.Instance.CurrentGame.Round.Value.ToString();
                    break;


                default:
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