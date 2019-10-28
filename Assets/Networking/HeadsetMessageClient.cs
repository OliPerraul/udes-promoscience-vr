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
        [SerializeField]
        private Resources resources;


        [SerializeField]
        ScriptableDirective directive;

        [SerializeField]
        ScriptableInteger gameRound;

        [SerializeField]
        ScriptableBoolean isConnectedToPair;

        [SerializeField]
        ScriptableBoolean isDiverging;

        [SerializeField]
        ScriptableString pairedIpAdress;

        [SerializeField]
        ScriptablePlayerInformation playerInformation;

        [SerializeField]
        ScriptableVector3 playerPosition;

        [SerializeField]
        ScriptableQuaternion playerRotation;

        [SerializeField]
        Labyrinths.ScriptableTile playerPaintTile;

        [SerializeField]
        ScriptableAction playerReachedTheEnd;

        [SerializeField]
        Labyrinths.ScriptableTileArray playerTilesToPaint;

        [SerializeField]
        ScriptableBoolean returnToDivergencePointAnswer;

        [SerializeField]
        ScriptableAction returnToDivergencePointRequest;

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
                client.RegisterHandler(ReturnToDivergencePointAnswerMessage.GetCustomMsgType(), OnReturnToDivergencePointAnswer);

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
            directive.valueChangedEvent += SendDirective;
            returnToDivergencePointRequest.action += SendReturnToDivergencePointRequest;
            Client.Instance.clientStateChangedEvent += OnGameStateChanged;

            isConnectedToPair.Value = true;
        }

        void OnDisconnect(NetworkMessage netMsg)
        {
            isConnectedToPair.Value = false;

            Client.Instance.clientStateChangedEvent -= OnGameStateChanged;
            directive.valueChangedEvent -= SendDirective;
            returnToDivergencePointRequest.action -= SendReturnToDivergencePointRequest;

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
            Client.Instance.Respect = msg.algorithmRespect;

            if (Client.Instance.Respect >= 1 && isDiverging.Value)
            {
                isDiverging.Value = false;
            }
            else if (Client.Instance.Respect < 1 && !isDiverging.Value)
            {
                isDiverging.Value = true;
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
            playerPaintTile.SetTile(msg.tilePositionX, msg.tilePositionY, msg.tileColor);
        }

        void OnPlayerPosition(NetworkMessage netMsg)
        {
            PlayerPositionMessage msg = netMsg.ReadMessage<PlayerPositionMessage>();
            playerPosition.Value = msg.position;
        }

        void OnPlayerReachedTheEnd(NetworkMessage netMsg)
        {
            playerReachedTheEnd.FireAction();
        }

        void OnPlayerRotation(NetworkMessage netMsg)
        {
            PlayerRotationMessage msg = netMsg.ReadMessage<PlayerRotationMessage>();
            playerRotation.Value = msg.rotation;
        }

        void OnPlayerTilesToPaint(NetworkMessage netMsg)
        {
            PlayerTilesToPaintMessage msg = netMsg.ReadMessage<PlayerTilesToPaintMessage>();
            playerTilesToPaint.Value = msg.tiles;
        }

        void OnReturnToDivergencePointAnswer(NetworkMessage netMsg)
        {
            ReturnToDivergencePointAnswerMessage msg = netMsg.ReadMessage<ReturnToDivergencePointAnswerMessage>();
            returnToDivergencePointAnswer.Value = msg.answer;
        }

        void SendDirective()
        {
            DirectiveMessage msg = new DirectiveMessage();
            msg.directive = directive.Value;

            client.Send(msg.GetMsgType(), msg);
        }

        void SendRequestForGameInformation()
        {
            RequestForGameInformationMessage msg = new RequestForGameInformationMessage();
            msg.gameRound = gameRound.Value;

            client.Send(msg.GetMsgType(), msg);
        }

        void SendReturnToDivergencePointRequest()
        {
            ReturnToDivergencePointRequestMessage msg = new ReturnToDivergencePointRequestMessage();

            client.Send(msg.GetMsgType(), msg);
        }
    }
}