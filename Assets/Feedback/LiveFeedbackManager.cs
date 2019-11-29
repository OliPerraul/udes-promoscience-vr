using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience
{
    public class LiveFeedbackManager : MonoBehaviour
    {
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
                    break;
            }
        }
    }
}