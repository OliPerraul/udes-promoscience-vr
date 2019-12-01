using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Labyrinths;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.UI;

namespace UdeS.Promoscience.Replays
{
    public class ReplayManager : Cirrus.BaseSingleton<ReplayManager>
    {
        [SerializeField]
        private ReplayManagerAsset asset;

        private BaseReplay CurrentReplay;

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

            //State = ServerState.ReplaySelect;
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
                    GameManager.Instance.CurrentGame.Round.Value),
                GameManager.Instance.CurrentGame.CurrentLabyrinth);

            CurrentReplay.Start();
        }




        public void Update()
        {

        }
    }
}