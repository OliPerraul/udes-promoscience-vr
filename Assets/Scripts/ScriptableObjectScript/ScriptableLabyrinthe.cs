#if UNITY_EDITOR || UNITY_STANDALONE_WIN

using System.Collections;
using UnityEngine;


//For now there all the player use the same labyrinthe so there is no need to keep the data of more then one, 
//also in that case it will still work but if id is alway changed then the data will always comme from the database directly
 [CreateAssetMenu(fileName = "Data", menuName = "Data/Labyrinthe", order = 1)]
public class ScriptableLabyrinthe : ScriptableObject
{
    int currentId = -1;
    int[,] data;

    public int[,] GetLabyritheDatawithId(int id)
    {
        if(id!= currentId)
        {
            SQLiteUtilities.GetLabyrintheDataWithId(id);
            currentId = id;
        }
        return data;
    }
}

#endif
