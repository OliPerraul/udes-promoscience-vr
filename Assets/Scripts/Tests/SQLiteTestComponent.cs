using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQLiteTestComponent : MonoBehaviour
{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    void Start ()
    {
        /*
        int teamId = 1;
        int teamName = 1;
        int teamColor = 1;
        int courseId = 1;
        int labyrithId = 1;
        int algorithmId = 1;
        int eventType = 1;
        System.DateTime now = System.DateTime.Now;

        SQLiteUtilities.InsertPlayerAction(teamId, teamName, teamColor, courseId, labyrithId, algorithmId, eventType, now.ToString("yyyy-MM-dd"), now.ToString("HH:mm:ss"));

        SQLiteUtilities.ReadDatabase();
        */
        int id = 1;
        int sizeX = 10;
        int sizeY = 10;

        int[] data = new int[sizeX * sizeY];

        //Colum 0
        data[(0 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 1] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 8] = Constants.TILE_WALL_1_ID;
        data[(0 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 1
        data[(1 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(1 * sizeX) + 1] = Constants.TILE_START_1_ID;
        data[(1 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(1 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(1 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(1 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(1 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 2
        data[(2 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 5] = Constants.TILE_END_1_ID;
        data[(2 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(2 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(2 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 3
        data[(3 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(3 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 2] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(3 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(3 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(3 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 4
        data[(4 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(4 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(4 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(4 * sizeX) + 8] = Constants.TILE_WALL_1_ID;
        data[(4 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 5
        data[(5 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(5 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(5 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 6
        data[(6 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 2] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 6] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(6 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(6 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 7
        data[(7 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(7 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(7 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(7 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 6] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(7 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 8
        data[(8 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(8 * sizeX) + 1] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 2] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 3] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 4] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 5] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(8 * sizeX) + 7] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 8] = Constants.TILE_FLOOR_1_ID;
        data[(8 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        //Colum 9
        data[(9 * sizeX) + 0] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 1] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 2] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 3] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 4] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 5] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 6] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 7] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 8] = Constants.TILE_WALL_1_ID;
        data[(9 * sizeX) + 9] = Constants.TILE_WALL_1_ID;

        SQLiteUtilities.InsertOrReplaceLabyrinth(id, sizeX, sizeY, data);

        int rSizeX = 0;
        int rSizeY = 0;

        int[] rData = new int[0];

        SQLiteUtilities.SetLabyrintheDataWithId(id, ref rSizeX, ref rSizeY, ref rData);

        string specs = rSizeX + " " + rSizeY;

        for (int i = 0; i < sizeX * sizeY; i++)
        {
            specs += " " + rData[i];
        }

        Debug.Log("SizeX = " + rSizeX + " SizeY = " + rSizeY);
        Debug.Log(specs);
    }

#endif
}
