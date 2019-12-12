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
    public class HeadsetMessagePeer : MonoBehaviour
    {
        [SerializeField]
        DirectiveManagerAsset directive;

        [SerializeField]
        GameAsset gameManager;

        [SerializeField]
        public ControlsAsset controls;

        [SerializeField]
        public HeadsetToolManagerAsset headsetTools;

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
            controls.BroadcastPlayerRotation.OnValueChangedHandler += SendPlayerRotation;
            controls.PaintingColor.OnValueChangedHandler += SendPaintingColor;


            // For both type of scanner
            headsetTools.ScannedDistance.OnValueChangedHandler += SendToolScannedDistance;
            headsetTools.CompassRotation.OnValueChangedHandler += SendCompassRotation;


            gameManager.IsLevelCompleted.OnValueChangedHandler += OnRoundCompleted;
            gameManager.Level.OnValueChangedHandler += OnRoundChanged;


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


        public void OnRoundCompleted(bool completed)
        {
            if(completed)
            SendEndReached();
        }

        public void OnRoundChanged(int round)
        {
            DelayedSendGameInformation(Client.Instance.State.Value);
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
            controls.BroadcastPlayerRotation.OnValueChangedHandler -= SendPlayerRotation;
            
            playerInformation.playerInformationChangedEvent -= SendPlayerInformation;
        }

        void OnDirective(NetworkMessage netMsg)
        {
            DirectiveMessage msg = netMsg.ReadMessage<DirectiveMessage>();
            directive.CurrentDirective.Set(msg.directive, notify:true);
        }



        //void OnDirective(NetworkMessage netMsg)
        //{
        //    DirectiveMessage msg = netMsg.ReadMessage<DirectiveMessage>();
        //    directive.CurrentDirective.Set(msg.directive, notify: true);
        //}


        void OnReturnToDivergencePointRequest(NetworkMessage netMsg)
        {
            algorithmRespect.OnReturnToDivergencePointRequestHandler?.Invoke();
        }

        void OnRequestForGameInformation(NetworkMessage netMsg)
        {
            RequestForGameInformationMessage msg = netMsg.ReadMessage<RequestForGameInformationMessage>();
            gameRoundRequest = msg.gameRound;

            if (Client.Instance.State.Value == ClientGameState.Playing || 
                Client.Instance.State.Value == ClientGameState.PlayingTutorial || 
                Client.Instance.State.Value == ClientGameState.WaitingForNextLevel)
            {
                if (gameRoundRequest == gameManager.Level.Value)
                {
                    if (gameManager.IsLevelCompleted.Value)
                    {
                        SendEndReached();
                    }
                    else
                    {
                        SendAlgorithm();
                        SendAlgorithmRespect(algorithmRespect.Respect);
                        SendPlayerPosition(controls.PlayerPosition.Value);
                        SendPlayerRotation(controls.BroadcastPlayerRotation.Value);
                        SendPlayerTilesToPaint(controls.PlayerTilesToPaint.Value);
                    }

                    return;
                }
            }

            if (!isRequestDelayed)
            {
                Client.Instance.State.OnValueChangedHandler += DelayedSendGameInformation;
                isRequestDelayed = true;
            }
        }


        void DelayedSendGameInformation(ClientGameState state)
        {
            if (Client.Instance.State.Value == ClientGameState.Playing || 
                Client.Instance.State.Value == ClientGameState.PlayingTutorial || 
                Client.Instance.State.Value == ClientGameState.WaitingForNextLevel)
            {
                if (gameRoundRequest == gameManager.Level.Value)
                {
                    if (gameManager.IsLevelCompleted.Value)
                    {
                        SendEndReached();
                    }
                    else
                    {
                        SendAlgorithm();
                        SendAlgorithmRespect(algorithmRespect.Respect);
                        SendPlayerPosition(controls.PlayerPosition.Value);
                        SendPlayerRotation(controls.BroadcastPlayerRotation.Value);
                        SendPlayerTilesToPaint(controls.PlayerTilesToPaint.Value);
                    }

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



        void SendPaintingColor(TileColor paintingColor)
        {
            PaintingColorMessage msg = new PaintingColorMessage();
            msg.tileColor = paintingColor;
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
            msg.rotation = controls.BroadcastPlayerRotation.Value;
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
            msg.tiles = Client.Instance.Labyrinth.Value.GetTilesToPaint();

            clientConnection.Send(msg.GetMsgType(), msg);
        }


        // Tools
        // For both scanners
        public void SendToolScannedDistance(float scannedDistance)
        {
            ScannedDistanceMessage msg = new ScannedDistanceMessage();
            msg.distance = scannedDistance; // Client.Instance.Labyrinth.Value.GetTilesToPaint();

            clientConnection.Send(msg.GetMsgType(), msg);
        }


                // Tools
        // For both scanners
        public void SendCompassRotation(Quaternion rotation)
        {
            CompassRotationMessage msg = new CompassRotationMessage();
            msg.rot = rotation; // Client.Instance.Labyrinth.Value.GetTilesToPaint();

            clientConnection.Send(msg.GetMsgType(), msg);
        }

    }
}