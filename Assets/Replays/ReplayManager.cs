using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Labyrinths;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.UI;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replays
{
    public class ReplayManager : Cirrus.BaseSingleton<ReplayManager>
    {
        [SerializeField]
        private ReplayManagerAsset asset;

        private BaseReplay CurrentReplay;

        public SplitReplay SplitReplay;

        public void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnGameStateValueChanged;
        }

        public void StartReplaySelect()
        {
            // TODO: Player should not refer to courseId anymore, maybe simply refer to course obj?               
            foreach (Player player in PlayerList.instance.list)
            {
                // Tell clients to pay attention
                if (player.ServerPlayerGameState == ClientGameState.WaitingReplay ||
                    player.ServerPlayerGameState == ClientGameState.ViewingLocalReplay ||
                    player.ServerPlayerGameState == ClientGameState.ViewingGlobalReplay ||
                    player.ServerPlayerGameState == ClientGameState.PlayingTutorial ||
                    player.ServerPlayerGameState == ClientGameState.ViewingLocalReplay ||
                    player.ServerPlayerGameState == ClientGameState.Playing)
                {
                    player.TargetSetGameState(player.connectionToClient, ClientGameState.ViewingGlobalReplay);
                }
            }

            List<Round> rounds = new List<Round>();

            // Get all rounds until now
            for (int i = 0; i < GameManager.Instance.CurrentGame.RoundNumber.Value + 1; i++)
            {
                rounds.Add(
                    new Round
                    {
                        Labyrinth = SQLiteUtilities.GetLabyrinthForGameRound(
                            GameManager.Instance.CurrentGame.Id,
                            i),

                        Courses = SQLiteUtilities.GetCoursesForGameRound(
                            GameManager.Instance.CurrentGame.Id,
                            i),
                    }
                    );
            }

            SplitReplay = new SplitReplay(
                asset,
                rounds);

            CurrentReplay = SplitReplay;

            CurrentReplay.Start();
        }


        public void OnGameStateValueChanged(ServerState state)
        {
            //switch (state)
            //{
            //    case ServerState.LabyrinthReplay:
            //        display.SetActive(true);
            //        //viewRawImage.texture = Server.Instance.CurrentLabyrinth.Camera.RenderTexture;
            //        break;

            //    default:
            //        display.SetActive(false);
            //        break;
            //}

        }

        public void StartInstantReplay()
        {
            // TODO: Player should not refer to courseId anymore, maybe simply refer to course obj?               
            foreach (Player player in PlayerList.instance.list)
            {
                // Tell clients to pay attention
                if (player.ServerPlayerGameState == ClientGameState.WaitingReplay ||
                    player.ServerPlayerGameState == ClientGameState.ViewingLocalReplay ||
                    player.ServerPlayerGameState == ClientGameState.ViewingGlobalReplay ||
                    player.ServerPlayerGameState == ClientGameState.PlayingTutorial ||
                    player.ServerPlayerGameState == ClientGameState.Playing)
                {
                    player.TargetSetGameState(
                        player.connectionToClient,
                        ClientGameState.ViewingGlobalReplay);
                }
            }

            CurrentReplay = new InstantReplay(
                asset,
                SQLiteUtilities.GetCoursesForGameRound(
                    GameManager.Instance.CurrentGame.Id, 
                    GameManager.Instance.CurrentGame.RoundNumber.Value),
                GameManager.Instance.CurrentGame.CurrentLabyrinth);

            CurrentReplay.Start();
        }




        public void Update()
        {

        }
    }
}