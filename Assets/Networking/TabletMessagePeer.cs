using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UdeS.Promoscience.Controls;

using UdeS.Promoscience.ScriptableObjects;
using System;
//using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.Network
{
    public class TabletMessagePeer : MonoBehaviour
    {
        //[SerializeField]
        //ScriptableFloat algorithmRespect;
        //[SerializeField]
        //private Resources resources;

        [SerializeField]
        GameAsset gameRound;

        [SerializeField]
        ScriptableBoolean isConnectedToPair;

        [SerializeField]
        ScriptableString pairedIpAdress;

        [SerializeField]
        ScriptablePlayerInformation playerInformation;


        [SerializeField]
        DirectiveManagerAsset directive;

        //[SerializeField]
        //ScriptableAction returnToDivergencePointRequest;
        [SerializeField]
        ControlsAsset controls;


        [SerializeField]
        private TabletToolManagerAsset tabletTools;


        [SerializeField]
        private Algorithms.AlgorithmRespectAsset algorithmRespect;



        NetworkClient client = null;

        public int serverPort = 9996;

        private void Start()
        {
            pairedIpAdress.valueChangedEvent += StartServer;
        }

        private void OnDestroy()
        {
            StopClient();
        }

        void StartServer()
        {
            if (client == null)
            {
                client = new NetworkClient();
                client.RegisterHandler(MsgType.Connect, OnConnect);
                client.RegisterHandler(MsgType.Disconnect, OnDisconnect);
                client.RegisterHandler(AlgorithmMessage.GetCustomMsgType(), OnAlgorithm);
                client.RegisterHandler(AlgorithmRespectMessage.GetCustomMsgType(), OnAlgorithmRespect);
                client.RegisterHandler(PlayerInformationMessage.GetCustomMsgType(), OnPlayerInformation);
                client.RegisterHandler(PlayerPaintTileMessage.GetCustomMsgType(), OnPlayerPaintTile);
                client.RegisterHandler(PaintingColorMessage.GetCustomMsgType(), OnPaintingColor);
                client.RegisterHandler(PlayerPositionMessage.GetCustomMsgType(), OnPlayerPosition);
                client.RegisterHandler(PlayerReachedTheEndMessage.GetCustomMsgType(), OnPlayerReachedTheEnd);
                client.RegisterHandler(PlayerRotationMessage.GetCustomMsgType(), OnPlayerRotation);
                client.RegisterHandler(PlayerTilesToPaintMessage.GetCustomMsgType(), OnPlayerTilesToPaint);


                client.RegisterHandler(ScannedDistanceMessage.GetCustomMsgType(), OnScannedDistance);
                client.RegisterHandler(CompassRotationMessage.GetCustomMsgType(), OnCompassRotation);

                //client.RegisterHandler(ReturnToDivergencePointAnswerMessage.GetCustomMsgType(), OnReturnToDivergencePointAnswer);

                client.Connect(pairedIpAdress.Value, serverPort);
            }
            else
            {
                Client.Instance.State.Value = ClientGameState.Ready;
            }
        }

        private void OnScannedDistance(NetworkMessage netMsg)
        {
            ScannedDistanceMessage msg = netMsg.ReadMessage<ScannedDistanceMessage>();
            tabletTools.ScannedDistance.Value = msg.distance;
        }

        private void OnCompassRotation(NetworkMessage netMsg)
        {
            CompassRotationMessage msg = netMsg.ReadMessage<CompassRotationMessage>();
            tabletTools.CompassRotation.Value = msg.rot;
        }

        private void OnPaintingColor(NetworkMessage netMsg)
        {
            PaintingColorMessage msg = netMsg.ReadMessage<PaintingColorMessage>();
            controls.PaintingColor.Value = msg.tileColor;
        }

        void StopClient()
        {
            if (client != null)
            {
                client.Disconnect();
                client = null;
            }
        }

        void OnGameStateChanged(ClientGameState state)
        {
            if (Client.Instance.State.Value == ClientGameState.Playing || 
                Client.Instance.State.Value == ClientGameState.PlayingTutorial)
            {
                SendRequestForGameInformation();
            }
        }

        void OnConnect(NetworkMessage netMsg)
        {
            directive.CurrentDirective.OnValueChangedHandler += SendDirective;
            algorithmRespect.OnReturnToDivergencePointRequestHandler += SendReturnToDivergencePointRequest;

            Client.Instance.State.OnValueChangedHandler += OnGameStateChanged;

            isConnectedToPair.Value = true;
        }

        void OnDisconnect(NetworkMessage netMsg)
        {
            isConnectedToPair.Value = false;

            Client.Instance.State.OnValueChangedHandler -= OnGameStateChanged;
            directive.CurrentDirective.OnValueChangedHandler -= SendDirective;
            algorithmRespect.OnReturnToDivergencePointRequestHandler -= SendReturnToDivergencePointRequest;

            client.Connect(pairedIpAdress.Value, serverPort);
        }

        void OnAlgorithm(NetworkMessage netMsg)
        {
            AlgorithmMessage msg = netMsg.ReadMessage<AlgorithmMessage>();
            //gameState.Algorithm = resources.CreateAlgorithm(msg.algorithm);
        }

        void OnAlgorithmRespect(NetworkMessage netMsg)
        {
            AlgorithmRespectMessage msg = netMsg.ReadMessage<AlgorithmRespectMessage>();
            algorithmRespect.Respect = msg.algorithmRespect;

            if (algorithmRespect.Respect >= 1 && algorithmRespect.IsDiverging.Value)
            {
                algorithmRespect.IsDiverging.Value = false;
            }
            else if (algorithmRespect.Respect < 1 && !algorithmRespect.IsDiverging.Value)
            {
                algorithmRespect.IsDiverging.Value = true;
            }
        }


        // IT BREAKS HERE FOR CONNECXION WITH TABLET
        void OnPlayerInformation(NetworkMessage netMsg)
        {
            PlayerInformationMessage msg = netMsg.ReadMessage<PlayerInformationMessage>();
            playerInformation.SetPlayerInformation(msg.teamId);

            //DEbUGTEXT.Instance.text.text = "Player Info received";


            Client.Instance.State.Value = ClientGameState.Ready;
        }

        void OnPlayerPaintTile(NetworkMessage netMsg)
        {
            PlayerPaintTileMessage msg = netMsg.ReadMessage<PlayerPaintTileMessage>();
            var previousTile = controls.PlayerPaintTile.Value;
            previousTile.Color = msg.tileColor;
            previousTile.x = msg.tilePositionX;
            previousTile.y = msg.tilePositionY;
            controls.PlayerPaintTile.Value = previousTile;
        }

        void OnPlayerPosition(NetworkMessage netMsg)
        {
            PlayerPositionMessage msg = netMsg.ReadMessage<PlayerPositionMessage>();
            controls.PlayerPosition.Value = msg.position;
        }

        void OnPlayerReachedTheEnd(NetworkMessage netMsg)
        {
            if(controls.OnPlayerReachedTheEndHandler != null)
            controls.OnPlayerReachedTheEndHandler.Invoke();
        }

        void OnPlayerRotation(NetworkMessage netMsg)
        {
            PlayerRotationMessage msg = netMsg.ReadMessage<PlayerRotationMessage>();
            controls.BroadcastPlayerRotation.Value = msg.rotation;
        }

        void OnPlayerTilesToPaint(NetworkMessage netMsg)
        {
            PlayerTilesToPaintMessage msg = netMsg.ReadMessage<PlayerTilesToPaintMessage>();
            controls.PlayerTilesToPaint.Value = msg.tiles;
        }

        //void OnReturnToDivergencePointAnswer(NetworkMessage netMsg)
        //{
        //    ReturnToDivergencePointAnswerMessage msg = netMsg.ReadMessage<ReturnToDivergencePointAnswerMessage>();
        //    algorithmRespect.ReturnToDivergencePointAnswer.Value = msg.answer;
        //}

        void SendDirective(Directive directive)
        {
            DirectiveMessage msg = new DirectiveMessage();
            msg.directive = directive;
            client.Send(msg.GetMsgType(), msg);
        }

        void SendRequestForGameInformation()
        {
            RequestForGameInformationMessage msg = new RequestForGameInformationMessage();
            msg.gameRound = gameRound.Level.Value;

            client.Send(msg.GetMsgType(), msg);
        }

        void SendReturnToDivergencePointRequest()
        {
            ReturnToDivergencePointRequestMessage msg = new ReturnToDivergencePointRequestMessage();
            client.Send(msg.GetMsgType(), msg);
        }
    }
}