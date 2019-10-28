using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Network
{
    public class HeadsetMessageServer : MonoBehaviour
    {
        //[SerializeField]
        //ScriptableAlgorithm algorithm;

        //[SerializeField]
        //ScriptableFloat algorithmRespect;

        [SerializeField]
        ScriptableDirective directive;

        [SerializeField]
        ScriptableInteger gameRound;

        [SerializeField]
        ScriptableBoolean isConnectedToPair;

        [SerializeField]
        ScriptableBoolean isRoundCompleted;

        [SerializeField]
        ScriptableString pairedIpAdress;

        [SerializeField]
        ScriptablePlayerInformation playerInformation;

        [SerializeField]
        Labyrinths.ScriptableTile playerPaintTile;

        [SerializeField]
        ScriptableVector3 playerPosition;

        [SerializeField]
        ScriptableAction playerReachedTheEnd;

        [SerializeField]
        ScriptableQuaternion playerRotation;

        [SerializeField]
        ScriptableBoolean returnToDivergencePointAnswer;

        [SerializeField]
        ScriptableAction returnToDivergencePointRequest;

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

            Client.Instance.OnRespectChangedHandler += SendAlgorithmRespect;
            playerPaintTile.valueChangedEvent += SendPlayerPaintTile;
            playerPosition.valueChangedEvent += SendPlayerPosition;
            playerReachedTheEnd.action += SendEndReached;
            playerRotation.valueChangedEvent += SendPlayerRotation;
            returnToDivergencePointAnswer.valueChangedEvent += SendReturnToDivergencePointAnswer;

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

            Client.Instance.OnRespectChangedHandler -= SendAlgorithmRespect;
            playerPaintTile.valueChangedEvent -= SendPlayerPaintTile;
            playerPosition.valueChangedEvent -= SendPlayerPosition;
            playerReachedTheEnd.action -= SendEndReached;
            playerRotation.valueChangedEvent -= SendPlayerRotation;
            returnToDivergencePointAnswer.valueChangedEvent -= SendReturnToDivergencePointAnswer;

            playerInformation.playerInformationChangedEvent -= SendPlayerInformation;
        }

        void OnDirective(NetworkMessage netMsg)
        {
            DirectiveMessage msg = netMsg.ReadMessage<DirectiveMessage>();
            directive.Value = msg.directive;
        }

        void OnReturnToDivergencePointRequest(NetworkMessage netMsg)
        {
            returnToDivergencePointRequest.FireAction();
        }

        void OnRequestForGameInformation(NetworkMessage netMsg)
        {
            RequestForGameInformationMessage msg = netMsg.ReadMessage<RequestForGameInformationMessage>();
            gameRoundRequest = msg.gameRound;

            if (Client.Instance.State == ClientGameState.Playing || 
                Client.Instance.State == ClientGameState.PlayingTutorial || 
                Client.Instance.State == ClientGameState.WaitingForNextRound)
            {
                if (gameRoundRequest == gameRound.Value)
                {
                    if (isRoundCompleted.Value)
                    {
                        SendEndReached();
                    }
                    else
                    {
                        SendAlgorithm();
                        SendAlgorithmRespect(Client.Instance.Respect);
                        SendPlayerPosition();
                        SendPlayerRotation();
                        SendPlayerTilesToPaint();
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

        void DelayedSendGameInformation()
        {
            if (Client.Instance.State == ClientGameState.Playing || 
                Client.Instance.State == ClientGameState.PlayingTutorial || 
                Client.Instance.State == ClientGameState.WaitingForNextRound)
            {
                if (gameRoundRequest == gameRound.Value)
                {
                    if (isRoundCompleted.Value)
                    {
                        SendEndReached();
                    }
                    else
                    {
                        SendAlgorithm();
                        SendAlgorithmRespect(Client.Instance.Respect);
                        SendPlayerPosition();
                        SendPlayerRotation();
                        SendPlayerTilesToPaint();
                    }

                    gameRound.valueChangedEvent -= DelayedSendGameInformation;
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

        void SendPlayerPosition()
        {
            PlayerPositionMessage msg = new PlayerPositionMessage();
            msg.position = playerPosition.Value;

            clientConnection.Send(msg.GetMsgType(), msg);
        }

        void SendPlayerRotation()
        {
            PlayerRotationMessage msg = new PlayerRotationMessage();
            msg.rotation = playerRotation.Value;

            clientConnection.Send(msg.GetMsgType(), msg);
        }

        void SendPlayerPaintTile()
        {
            PlayerPaintTileMessage msg = new PlayerPaintTileMessage();
            msg.tilePositionX = playerPaintTile.TilePosition.x;
            msg.tilePositionY = playerPaintTile.TilePosition.y;
            msg.tileColor = playerPaintTile.TileColor;

            clientConnection.Send(msg.GetMsgType(), msg);
        }

        void SendPlayerTilesToPaint()
        {
            PlayerTilesToPaintMessage msg = new PlayerTilesToPaintMessage();
            msg.tiles = Client.Instance.Labyrinth.GetTilesToPaint();

            clientConnection.Send(msg.GetMsgType(), msg);
        }

        void SendReturnToDivergencePointAnswer()
        {
            ReturnToDivergencePointAnswerMessage msg = new ReturnToDivergencePointAnswerMessage();
            msg.answer = returnToDivergencePointAnswer.Value;

            clientConnection.Send(msg.GetMsgType(), msg);
        }
    }
}