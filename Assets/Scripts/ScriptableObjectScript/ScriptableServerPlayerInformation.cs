using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void AddPlayerOrReconnect(int playerId, string playerDeviceUniqueIdentifier)
    {
        for(int i = 0; i < playersInformation.Count; i++)
        {
            if(playersInformation[i].playerDeviceUniqueIdentifier == playerDeviceUniqueIdentifier)
            {
                playersInformation[i].OnPlayerReconnect(playerId);
                return ;
            }
        }

        PlayerInformation playerinformation = new PlayerInformation(playerId, playerDeviceUniqueIdentifier);
        playersInformation.Add(playerinformation);

        OnAddPlayer();
    }

    public void OnPlayerDisconnect(string playerDeviceUniqueIdentifier)
    {
        for (int i = 0; i < playersInformation.Count; i++)
        {
            if (playersInformation[i].playerDeviceUniqueIdentifier == playerDeviceUniqueIdentifier)
            {
                playersInformation[i].OnPlayerDisconnect();
                return;
            }
        }
    }

    public PlayerInformation GetPlayerInformationWithId(int id)
    {
        if(id >= 0 && id < playersInformation.Count)
        {
            return playersInformation[id];
        }
        else
        {
            Debug.Log("GetPlayerInformationWithId : Out of bound");//Temp
            return null;
        }
    }

    public void LoadInformationFromDatabase()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        //Check if there is stored information
        //SQLiteUtilities.

        playersInformation.Clear();
#endif
    }

    public void SaveInformationToDatabase()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        //Check if there is stored information
        //SQLiteUtilities.
#endif
    }

    public void ClearPlayerInformationList()//Might need to add a destroy part in struct to deregister from event
    {
        playersInformation.Clear();
        OnRemovePlayer();
    }
}

