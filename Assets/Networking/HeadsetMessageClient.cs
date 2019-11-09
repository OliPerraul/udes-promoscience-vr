using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.Network
{
    public class HeadsetMessageClient : MonoBehaviour
    {
        //[SerializeField]
        //ScriptableFloat algorithmRespect;
        //[SerializeField]
        //private Resources resources;

        [SerializeField]
        GameRoundManagerAsset gameRound;

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
        AvatarControllerAsset controls;

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
                client.RegisterHandler(PlayerPositionMessage.GetCustomMsgType(), OnPlayerPosition);
                client.RegisterHandler(PlayerReachedTheEndMessage.GetCustomMsgType(), OnPlayerReachedTheEnd);
                client.RegisterHandler(PlayerRotationMessage.GetCustomMsgType(), OnPlayerRotation);
                client.RegisterHandler(PlayerTilesToPaintMessage.GetCustomMsgType(), OnPlayerTilesToPaint);
                //client.RegisterHandler(ReturnToDivergencePointAnswerMessage.GetCustomMsgType(), OnReturnToDivergencePointAnswer);

                client.Connect(pairedIpAdress.Value, serverPort);
            }
            else
            {
                Client.Instance.State = ClientGameState.Ready;
            }
        }

        void StopClient()
        {
            if (client != null)
            {
                client.Disconnect();
                client = null;
            }
        }

        void OnGameStateChanged()
        {
            if (Client.Instance.State == ClientGameState.Playing || Client.Instance.State == ClientGameState.PlayingTutorial)
            {
                SendRequestForGameInformation();
            }
        }

        void OnConnect(NetworkMessage netMsg)
        {
            directive.Directive.OnValueChangedHandler += SendDirective;
            algorithmRespect.OnReturnToDivergencePointRequestHandler += SendReturnToDivergencePointRequest;

            Client.Instance.clientStateChangedEvent += OnGameStateChanged;

            isConnectedToPair.Value = true;
        }

        void OnDisconnect(NetworkMessage netMsg)
        {
            isConnectedToPair.Value = false;

            Client.Instance.clientStateChangedEvent -= OnGameStateChanged;
            directive.Directive.OnValueChangedHandler -= SendDirective;
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

        void OnPlayerInformation(NetworkMessage netMsg)
        {
            PlayerInformationMessage msg = netMsg.ReadMessage<PlayerInformationMessage>();
            playerInformation.SetPlayerInformation(msg.teamId);

            Client.Instance.State = ClientGameState.Ready;
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
            controls.PlayerRotation.Value = msg.rotation;
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
            msg.gameRound = gameRound.Round.Value;

            client.Send(msg.GetMsgType(), msg);
        }

        void SendReturnToDivergencePointRequest()
        {
            ReturnToDivergencePointRequestMessage msg = new ReturnToDivergencePointRequestMessage();
            client.Send(msg.GetMsgType(), msg);
        }
    }
}