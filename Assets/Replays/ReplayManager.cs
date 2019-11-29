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
        private GameObject sidebar;

        [SerializeField]
        private GameObject display;

        [SerializeField]
        private UnityEngine.UI.RawImage viewRawImage;

        public UnityEngine.UI.RawImage ViewRawImage => viewRawImage;

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
            switch (state)
            {
                case ServerState.LabyrinthReplay:
                    display.SetActive(true);
                    //viewRawImage.texture = Server.Instance.CurrentLabyrinth.Camera.RenderTexture;
                    break;

                default:
                    display.SetActive(false);
                    break;
            }

        }

        public void StartAdvancedReplay(Labyrinth labyrinth)
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

            //State = ServerState.AdvancedReplay;

            //Labyrinths.CurrentData = labyrinth.Data;

            //CurrentReplay = new Replays.SingleReplay(
                //replayController,
                //SQLiteUtilities.GetSessionCoursesForLabyrinth(labyrinth.Id),
                //labyrinth);

            //CurrentReplay.Start();
        }



        public void StartAdvancedReplay(Replays.BaseReplay replay)
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

            //State = ServerState.AdvancedReplay;

            //Courses = SQLiteUtilities.GetSessionCoursesForLabyrinth(replay.LabyrinthData.Id);

            //Labyrinths.CurrentData = replay.LabyrinthData;

            //CurrentReplay = replay;

            //CurrentReplay.Start();
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

            //State = ServerState.InstantReplay;

            CurrentReplay = new InstantReplay(
                asset,
                SQLiteUtilities.GetSessionCoursesForLabyrinth(
                    GameManager.Instance.CurrentGame.CurrentLabyrinth.Id),
                GameManager.Instance.CurrentGame.CurrentLabyrinth);

            CurrentReplay.Start();
        }




        public void Update()
        {

        }
    }
}