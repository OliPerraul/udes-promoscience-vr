using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;
//using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.UI
{

    public class PlayerLobbyEntry : MonoBehaviour
    {
        [SerializeField]
        ScriptableServerPlayerInformation playersInformation;

        //[SerializeField]
        //Teams.Resources teamList;

        [SerializeField]
        LocalizeString connectingString = new LocalizeString("Connecting..", "Connexion en cours..");

        [SerializeField]
        LocalizeString disconnectedString = new LocalizeString("Disconnected", "Déconnecté");

        [SerializeField]
        LocalizeString longestStraightString = new LocalizeString("Longest Straight", "Chemin le plus long");

        [SerializeField]
        LocalizeString noAssociatedPairString = new LocalizeString("No associated pair.", "Aucun pair associé.");

        [SerializeField]
        LocalizeString notReadyString = new LocalizeString("Waiting..", "En attente..");

        [SerializeField]
        LocalizeString pairedString = new LocalizeString("Device paired.", "appareil jumelé.");

        [SerializeField]
        LocalizeString pairingString = new LocalizeString("Device is pairing.", "Aparreil en jumelage.");

        [SerializeField]
        LocalizeString playingString = new LocalizeString("In game.", "Dans une partie.");

        [SerializeField]
        LocalizeString playingTutorialString = new LocalizeString("In tutorial", "Dans un tutoriel.");

        [SerializeField]
        LocalizeString readyString = new LocalizeString("Ready", "Prêt");

        [SerializeField]
        LocalizeString reconnectingString = new LocalizeString("Reconnecting", "Reconnexion");

        [SerializeField]
        LocalizeString shortestFlightString = new LocalizeString("Shortest Flight", "Vol le plus court");

        [SerializeField]
        LocalizeString standardAlgorithmString = new LocalizeString("Standard", "Standard");

        [SerializeField]
        LocalizeString unknownStatusString = new LocalizeString("Unknown", "Inconnu");

        [SerializeField]
        LocalizeString waitingForNextRoundString = new LocalizeString("Waiting for next round", "Inconnu");

        [SerializeField]
        LocalizeString waitingForPairConnectionString = new LocalizeString("Waiting for pair connection.", "En attente de connection a pair.");


        [SerializeField]
        LocalizeString viewingLocalPlayback = new LocalizeString("Viewing local playback.", "Reprise globale.");

        [SerializeField]
        LocalizeString viewingGlobalPlayback = new LocalizeString("Global playback.", "Reprise globale");

        [SerializeField]
        LocalizeString waitingPlaybackString  = new LocalizeString("Waiting playback", "Attente de la reprise.");

        [SerializeField]
        Image image;

        [SerializeField]
        Text nameText;

        [SerializeField]
        Text teamNameText;

        [SerializeField]
        Image color;

        [SerializeField]
        Text statusText;

        PlayerInformation playerInformation;

        int slotId;

        private void OnDestroy()
        {
            if (playerInformation != null)
            {
                playersInformation.orderChangedEvent -= OnOrderChanged;
            }
        }

        public void SetId(int id)
        {
            slotId = id;

            playersInformation.orderChangedEvent += OnOrderChanged;

            SetPlayer();
        }

        void OnOrderChanged()
        {
            if (playerInformation != null)
            {
                playerInformation.playerChangedEvent -= SetPlayer;
                playerInformation.playerTeamIdChangedEvent -= UpdateLobbyPlayerTeam;
                playerInformation.playerGameStateChangedEvent -= UpdateLobbyPlayerStatusText;
            }

            SetPlayer();
        }

        void SetPlayer()
        {
            playerInformation = playersInformation.GetPlayerInformationWithId(slotId);

            playerInformation.playerChangedEvent += SetPlayer;
            playerInformation.playerTeamIdChangedEvent += UpdateLobbyPlayerTeam;
            playerInformation.playerGameStateChangedEvent += UpdateLobbyPlayerStatusText;

            SetLobbyPlayerImage();
            SetLobbyPlayerName();
            UpdateLobbyPlayerTeam();
            UpdateLobbyPlayerStatusText();
        }

        void SetLobbyPlayerImage()
        {
            if (playerInformation.Player != null)
            {
                //Should change depending on the device type between a heaset or a tablet
            }
        }

        void SetLobbyPlayerName()
        {
            if (playerInformation.Player != null)
            {
                nameText.text = playerInformation.Player.serverDeviceType.ToString() + " " + playerInformation.Player.ServerDeviceName;
                //nameText.text = playerInformation.Player.serverDeviceType.ToString();
            }
        }

        void UpdateLobbyPlayerTeam()
        {
            if (playerInformation.PlayerTeamId != -1)
            {
                Teams.TeamResource team = Teams.Resources.Instance.GetScriptableTeamWithId(playerInformation.PlayerTeamId);
                teamNameText.text = team.TeamName;
                color.color = team.TeamColor;
            }
            else
            {
                teamNameText.text = "";
                color.color = Color.white;
            }
        }

        void UpdateLobbyPlayerStatusText()
        {
            if (playerInformation.Player == null)
            {
                statusText.text = disconnectedString.Value;
            }
            else
            {
                if (playerInformation.PlayerGameState == ClientGameState.Connecting)
                {
                    statusText.text = connectingString.Value;
                }
                else if (playerInformation.PlayerGameState == ClientGameState.NotReady)
                {
                    statusText.text = notReadyString.Value;
                }
                else if (playerInformation.PlayerGameState == ClientGameState.Pairing)
                {
                    statusText.text = pairingString.Value;
                }
                else if (
                    playerInformation.PlayerGameState == ClientGameState.NoAssociatedPair || 
                    playerInformation.PlayerGameState == ClientGameState.ReconnectingNoAssociatedPair)
                {
                    statusText.text = noAssociatedPairString.Value;
                }
                else if (playerInformation.PlayerGameState == ClientGameState.Paired)
                {
                    statusText.text = pairedString.Value;
                }
                else if (playerInformation.PlayerGameState == ClientGameState.Ready)
                {
                    statusText.text = readyString.Value;
                }
                else if (playerInformation.PlayerGameState == ClientGameState.PlayingTutorial)
                {
                    statusText.text = playingTutorialString.Value;
                }
                else if (playerInformation.PlayerGameState == ClientGameState.Playing)
                {
                    string text = playingString.Value;

                    if (playerInformation.Player.serverDeviceType == DeviceType.Headset)
                    {
                        if (playerInformation.Player.serverAlgorithm == Algorithms.Id.ShortestFlightDistance)
                        {
                            text += " - " + shortestFlightString.Value;
                        }
                        else if (playerInformation.Player.serverAlgorithm == Algorithms.Id.LongestStraight)
                        {
                            text += " - " + longestStraightString.Value;
                        }
                        else if (playerInformation.Player.serverAlgorithm == Algorithms.Id.Standard)
                        {
                            text += " - " + standardAlgorithmString.Value;
                        }
                    }

                    statusText.text = text;
                }
                else if (playerInformation.PlayerGameState == ClientGameState.WaitingForNextRound)
                {
                    statusText.text = waitingForNextRoundString.Value;
                }
                else if (playerInformation.PlayerGameState == ClientGameState.Reconnecting)
                {
                    statusText.text = reconnectingString.Value;
                }
                else if (playerInformation.PlayerGameState == ClientGameState.WaitingForPairConnection)
                {
                    statusText.text = waitingForPairConnectionString.Value;
                }
                else if (playerInformation.PlayerGameState == ClientGameState.WaitingReplay)
                {
                    statusText.text = waitingForPairConnectionString.Value;
                }
                else if (playerInformation.PlayerGameState == ClientGameState.ViewingLocalReplay)
                {
                    statusText.text = waitingForPairConnectionString.Value;
                }
                else
                {
                    statusText.text = unknownStatusString.Value;
                }
            }
        }
    }
}
