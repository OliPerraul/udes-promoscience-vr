using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/PlayerList", order = 1)]
public class ScriptablePlayerList : ScriptableObject
{
    public List<Player> list = new List<Player>();//should be public

    public Action addPlayerEvent;
    public Action<int> removePlayerEvent;

    public void AddPlayer(Player p)
    {
        list.Add(p);
        OnAddPlayer();
    }

    public void RemovePlayer(Player p)
    {
        int id = list.IndexOf(p);
        list.RemoveAt(id);
        OnRemovePlayer(id);
        Destroy(p);
    }

    public Player GetPlayerWithId(int id)
    {
        if(id < 0 || id >= list.Count)
        {
            //Exeption out of range
            Debug.Log("GetPlayerwithId Out of range!");
            return null;
        }

        return list[id];
    }

    public Player GetLastPlayer()
    {
        if (list.Count == 0)
        {
            //Exeption out of range
            Debug.Log("GetLastPlayer Out of range! No player in list");
            return null;
        }
        Debug.Log("List count : "+list.Count);
        return list[list.Count-1];
    }

    void OnAddPlayer()
    {
        if(addPlayerEvent != null)
        {
            addPlayerEvent();
        }
    }

    void OnRemovePlayer(int id)
    {
        if (removePlayerEvent != null)
        {
            removePlayerEvent(id);
        }
    }
}

