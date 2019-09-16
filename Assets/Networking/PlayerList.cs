using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UdeS.Promoscience.Network
{

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

        public int GetPlayerId(Player player)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == player)
                {
                    return i;
                }
            }

            return -1;
        }

        public Player GetPlayerWithId(int id)
        {
            if (id < 0 || id >= list.Count)
            {
                Debug.Log("GetPlayerwithId : Out of range!");
                return null;
            }

            return list[id];
        }

        public Player GetPlayerWithConnection(NetworkConnection conn)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].connectionToClient == conn)
                {
                    return list[i];
                }
            }

            return null;
        }

        public int GetLastPlayerId()
        {
            return list.Count - 1;
        }

        void OnAddPlayer()
        {
            if (addPlayerEvent != null)
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

}