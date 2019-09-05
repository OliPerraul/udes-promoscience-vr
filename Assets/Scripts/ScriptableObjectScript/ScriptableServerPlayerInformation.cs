using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/ServerPlayerInformation", order = 1)]
    public class ScriptableServerPlayerInformation : ScriptableObject
    {
        [SerializeField]
        List<PlayerInformation> playersInformation = new List<PlayerInformation>();

        public Action addPlayerEvent;
        public Action removePlayerEvent;

        public Action orderChangedEvent;

        void OnAddPlayer()
        {
            if (addPlayerEvent != null)
            {
                addPlayerEvent();
            }
        }

        void OnRemovePlayer()
        {
            if (removePlayerEvent != null)
            {
                removePlayerEvent();
            }
        }

        void OnOrderChanged()
        {
            if (removePlayerEvent != null)
            {
                removePlayerEvent();
            }
        }

        public void AddPlayerInformation(PlayerInformation playerInformation)
        {
            playersInformation.Add(playerInformation);

            OnAddPlayer();
        }

        public void AddPlayerOrReconnect(Player player)
        {
            for (int i = 0; i < playersInformation.Count; i++)
            {
                if (playersInformation[i].PlayerDeviceUniqueIdentifier == player.deviceUniqueIdentifier)
                {
                    playersInformation[i].OnPlayerReconnect(player);
                    return;
                }
            }

            PlayerInformation playerinformation = new PlayerInformation(player);
            playersInformation.Add(playerinformation);

            OnAddPlayer();
        }

        public void OnPlayerDisconnect(string playerDeviceUniqueIdentifier)
        {
            for (int i = 0; i < playersInformation.Count; i++)
            {
                if (playersInformation[i].PlayerDeviceUniqueIdentifier == playerDeviceUniqueIdentifier)
                {
                    playersInformation[i].OnPlayerDisconnect();
                    return;
                }
            }
        }

        public PlayerInformation GetPlayerInformationWithId(int id)
        {
            if (id >= 0 && id < playersInformation.Count)
            {
                return playersInformation[id];
            }
            else
            {
                Debug.Log("GetPlayerInformationWithId : Out of bound");//Temp
                return null;
            }
        }

        public int GetPlayerCount()
        {
            return playersInformation.Count;
        }

        public void LoadPlayerInformationFromDatabase()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            SQLiteUtilities.SetServerPlayerInformation(this);
#endif
        }

        public void SavePlayerInformationToDatabase()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            SQLiteUtilities.InsertServerPlayerInformation(this);
#endif
        }

        public void ClearPlayerInformation()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            SQLiteUtilities.ResetServerPlayerInformation();
#endif
        }
    }
}

