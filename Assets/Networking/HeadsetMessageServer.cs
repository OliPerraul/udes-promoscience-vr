using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.Controls;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Network
{
    public class HeadsetMessageServer : MonoBehaviour
    {
        [SerializeField]
        DirectiveManagerAsset directive;

        [SerializeField]
        GameRoundManagerAsset gameRound;

        [SerializeField]
        public AvatarControllerAsset controls;

        [SerializeField]
        private Algorithms.AlgorithmRespectAsset algorithmRespect;

        // TODO replace by network manager asset
        [SerializeField]
        ScriptableBoolean isConnectedToPair;

        // TODO replace by network manager asset
        [SerializeField]
        ScriptableString pairedIpAdress;

        // TODO replace by network manager asset
        [SerializeField]
        ScriptablePlayerInformation playerInformation;


        NetworkServerSimple server = null;

        NetworkConnection clientConnection = null;

        bool isRequestDelayed = false;

        int gameRoundRequest;

        public int serverPort = 9996;

        private void Start()
        {
            pairedIpAdress.valueChangedEvent += StartServer;
        }

        void Update()
        {
            if (server != null)
            {
                server.Update();
            }
        }

        void StartServer()
        {
            if (server == null)
            {
                server = new NetworkServerSimple();
                server.RegisterHandler(MsgType.Connect, OnConnect);
                server.RegisterHandler(MsgType.Disconnect, OnDisconnect);
                server.RegisterHandler(DirectiveMessage.GetCustomMsgType(), OnDirective);
                server.RegisterHandler(RequestForGameInformationMessage.GetCustomMsgType(), OnRequestForGameInformation);
                server.RegisterHandler(ReturnToDivergencePointRequestMessage.GetCustomMsgType(), OnReturnToDivergencePointRequest);

                server.Listen(serverPort);
            }
        }

        void StopServer()
        {
            if (server != null)
            {
                server.Stop();
                server = null;
            }
        }

        private void OnDestroy()
        {
            StopServer();
        }

        void OnConnect(NetworkMessage netMsg)
        {
            clientConnection = netMsg.conn;

            algorithmRespect.OnRespectChangedHandler += SendAlgorithmRespect;
            //algorithmRespect.ReturnToDivergencePointAnswer.OnValueChangedHandler += SendReturnToDivergencePointAnswer;

            controls.PlayerPaintTile.OnValueChangedHandler += SendPlayerPaintTile;
            controls.PlayerPosition.OnValueChangedHandler += SendPlayerPosition;
            controls.OnPlayerReachedTheEndHandler += SendEndReached;
            controls.PlayerRotation.OnValueChangedHandler += SendPlayerRotation;
            
            if (playerInformation.IsInitialize)
            {
                SendPlayerInformation();
            }
            else
            {
                playerInformation.playerInformationChangedEvent += SendPlayerInformation;
            }

            isConnectedToPair.Value = true;
        }

        void OnDisconnect(NetworkMessage netMsg)
        {
            isConnectedToPair.Value = false;

            clientConnection = null;

            algorithmRespect.OnRespectChangedHandler -= SendAlgorithmRespect;
            //algorithmRespect.ReturnToDivergencePointAnswer.OnValueChangedHandler -= SendReturnToDivergencePointAnswer;

            controls.PlayerPaintTile.OnValueChangedHandler -= SendPlayerPaintTile;
            controls.PlayerPosition.OnValueChangedHandler -= SendPlayerPosition;
            controls.OnPlayerReachedTheEndHandler -= SendEndReached;
            controls.PlayerRotation.OnValueChangedHandler -= SendPlayerRotation;
            
            playerInformation.playerInformationChangedEvent -= SendPlayerInformation;
        }

        void OnDirective(NetworkMessage netMsg)
        {
            DirectiveMessage msg = netMsg.ReadMessage<DirectiveMessage>();
            directive.CurrentDirective.Set(msg.directive, forceNotification:true);
        }

        void OnReturnToDivergencePointRequest(NetworkMessage netMsg)
        {
            algorithmRespect.OnReturnToDivergencePointRequestHandler?.Invoke();
        }

        void OnRequestForGameInformation(NetworkMessage netMsg)
        {
            RequestForGameInformationMessage msg = netMsg.ReadMessage<RequestForGameInformationMessage>();
            gameRoundRequest = msg.gameRound;

            if (Client.Instance.State == ClientGameState.Playing || 
                Client.Instance.State == ClientGameState.PlayingTutorial || 
                Client.Instance.State == ClientGameState.WaitingForNextRound)
            {
                if (gameRoundRequest == gameRound.Round.Value)
                {
                    if (gameRound.IsRoundCompleted.Value)
                    {
                        SendEndReached();
                    }
                    else
                    {
                        SendAlgorithm();
                        SendAlgorithmRespect(algorithmRespect.Respect);
                        SendPlayerPosition(controls.PlayerPosition.Value);
                        SendPlayerRotation(controls.PlayerRotation.Value);
                        SendPlayerTilesToPaint(controls.PlayerTilesToPaint.Value);
                    }

                    return;
                }
            }

            if (!isRequestDelayed)
            {
                Client.Instance.clientStateChangedEvent += DelayedSendGameInformation;
                isRequestDelayed = true;
            }
        }


        public void OnGameRoundChanged(int gameroudn)
        {
            DelayedSendGameInformation();
        }


        void DelayedSendGameInformation()
        {
            if (Client.Instance.State == ClientGameState.Playing || 
                Client.Instance.State == ClientGameState.PlayingTutorial || 
                Client.Instance.State == ClientGameState.WaitingForNextRound)
            {
                if (gameRoundRequest == gameRound.Round.Value)
                {
                    if (gameRound.IsRoundCompleted.Value)
                    {
                        SendEndReached();
                    }
                    else
                    {
                        SendAlgorithm();
                        SendAlgorithmRespect(algorithmRespect.Respect);
                        SendPlayerPosition(controls.PlayerPosition.Value);
                        SendPlayerRotation(controls.PlayerRotation.Value);
                        SendPlayerTilesToPaint(controls.PlayerTilesToPaint.Value);
                    }

                    gameRound.Round.OnValueChangedHandler -= OnGameRoundChanged;
                    isRequestDelayed = false;
                }
            }
        }

        void SendAlgorithm()
        {
            AlgorithmMessage msg = new AlgorithmMessage();
            //msg.algorithm = algorithm.Value;

            clientConnection.Send(msg.GetMsgType(), msg);
        }

        void SendAlgorithmRespect(float respect)
        {
            AlgorithmRespectMessage msg = new AlgorithmRespectMessage();
            msg.algorithmRespect = respect;
            clientConnection.Send(msg.GetMsgType(), msg);
        }

        void SendEndReached()
        {
            PlayerReachedTheEndMessage msg = new PlayerReachedTheEndMessage();

            clientConnection.Send(msg.GetMsgType(), msg);
        }

        void SendPlayerInformation()
        {
            PlayerInformationMessage msg = new PlayerInformationMessage();
            msg.teamId = playerInformation.PlayerTeamId;

            clientConnection.Send(msg.GetMsgType(), msg);
        }

        void SendPlayerPosition(Vector3 position)
        {
            PlayerPositionMessage msg = new PlayerPositionMessage();
            msg.position = controls.PlayerPosition.Value;
            clientConnection.Send(msg.GetMsgType(), msg);
        }

        void SendPlayerRotation(Quaternion rotation)
        {
            PlayerRotationMessage msg = new PlayerRotationMessage();
            msg.rotation = controls.PlayerRotation.Value;
            clientConnection.Send(msg.GetMsgType(), msg);
        }

        void SendPlayerPaintTile(Tile tile)
        {
            PlayerPaintTileMessage msg = new PlayerPaintTileMessage();
            msg.tilePositionX = controls.PlayerPaintTile.Value.Position.x;
            msg.tilePositionY = controls.PlayerPaintTile.Value.Position.y;
            msg.tileColor = controls.PlayerPaintTile.Value.Color;

            clientConnection.Send(msg.GetMsgType(), msg);
        }

        void SendPlayerTilesToPaint(Tile[] tiles)
        {
            PlayerTilesToPaintMessage msg = new PlayerTilesToPaintMessage();
            msg.tiles = Client.Instance.Labyrinth.GetTilesToPaint();

            clientConnection.Send(msg.GetMsgType(), msg);
        }

        //void SendReturnToDivergencePointAnswer(bool value)
        //{
        //    ReturnToDivergencePointAnswerMessage msg = new ReturnToDivergencePointAnswerMessage();
        //    msg.answer = algorithmRespect.ReturnToDivergencePointAnswer.Value;

        //    clientConnection.Send(msg.GetMsgType(), msg);
        //}
    }
}