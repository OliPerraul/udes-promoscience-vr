using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/PlayerList", order = 1)]
public class ScriptablePlayerList : ScriptableObject
{
    List<Player> list = new List<Player>();

    //OnAddPlayerEvent
    //OnRemovePlayerEvent(int id)

    public void AddPlayer(Player p)
    {
        list.Add(p);
        //OnaddPlayerFire();
    }

    public void RemovePlayer(Player p)
    {
        int id = list.IndexOf(p);
        list.RemoveAt(id);
        //OnaddPlayerFire(id);
    }

    public Player GetPlayerWithId(int id)
    {
        if(id < 0 || id >= list.Count)
        {
            //Exeption out of range
            return null;
        }
        return list[id];
    }
}

