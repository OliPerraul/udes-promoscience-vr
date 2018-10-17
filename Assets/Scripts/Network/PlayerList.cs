using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerList : MonoBehaviour
{
    public static PlayerList instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public List<Player> list = new List<Player>();//should not be public

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

    public Player GetPlayerWithId(int id)//Unsign int pour eviter problème
    {
        if(id < 0 || id >= list.Count)
        {
            //Exeption out of range
            Debug.Log("GetPlayerwithId Out of range!");
            return null;
        }

        return list[id];
    }

    public int GetLastPlayerId()
    {
        return list.Count-1;
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

