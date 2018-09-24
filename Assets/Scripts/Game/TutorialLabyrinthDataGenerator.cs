using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLabyrinthDataGenerator : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger currentGameState;

    [SerializeField]
    ScriptableLabyrinth labyrinth;

    int id = -2;//negative id so that it is not mess up with id of those from the database
    int sizeX = 10;
    int sizeY = 10;
    void Start()
    {
        currentGameState.valueChangedEvent += OnGameStateChanged;
    }

    void OnGameStateChanged()
    {
        if (currentGameState.value == Constants.READY)
        {
            GenerateTutorialLabyrinthData();
            currentGameState.value = Constants.PLAYING_TUTORIAL;
        }
    }

    public void GenerateTutorialLabyrinthData()
    {
        int[,] map = new int[sizeX, sizeY];

        //Outerwall
        for (int i = 0; i < map.GetLength(0); i++)
        {
            map[i, 0] = Constants.TILE_WALL_1_ID;
            map[i, sizeY-1] = Constants.TILE_WALL_1_ID;
        }

        for (int j = 1; j < map.GetLength(1)-1; j++)
        {
            map[0, j] = Constants.TILE_WALL_1_ID;
            map[sizeX-1, j] = Constants.TILE_WALL_1_ID;
        }

        //Row 1
        map[1, 1] = Constants.TILE_START_1_ID;
        map[2, 1] = Constants.TILE_FLOOR_1_ID;
        map[3, 1] = Constants.TILE_FLOOR_1_ID;
        map[4, 1] = Constants.TILE_FLOOR_1_ID;
        map[5, 1] = Constants.TILE_FLOOR_1_ID;
        map[6, 1] = Constants.TILE_FLOOR_1_ID;
        map[7, 1] = Constants.TILE_FLOOR_1_ID;
        map[8, 1] = Constants.TILE_FLOOR_1_ID;

        //Row 2
        map[1, 2] = Constants.TILE_FLOOR_1_ID;
        map[2, 2] = Constants.TILE_WALL_1_ID;
        map[3, 2] = Constants.TILE_FLOOR_1_ID;
        map[4, 2] = Constants.TILE_WALL_1_ID;
        map[5, 2] = Constants.TILE_WALL_1_ID;
        map[6, 2] = Constants.TILE_FLOOR_1_ID;
        map[7, 2] = Constants.TILE_WALL_1_ID;
        map[8, 2] = Constants.TILE_FLOOR_1_ID;

        //Row 3
        map[1, 3] = Constants.TILE_FLOOR_1_ID;
        map[2, 3] = Constants.TILE_FLOOR_1_ID;
        map[3, 3] = Constants.TILE_FLOOR_1_ID;
        map[4, 3] = Constants.TILE_WALL_1_ID;
        map[5, 3] = Constants.TILE_WALL_1_ID;
        map[6, 3] = Constants.TILE_WALL_1_ID;
        map[7, 3] = Constants.TILE_WALL_1_ID;
        map[8, 3] = Constants.TILE_FLOOR_1_ID;

        //Row 4
        map[1, 4] = Constants.TILE_FLOOR_1_ID;
        map[2, 4] = Constants.TILE_WALL_1_ID;
        map[3, 4] = Constants.TILE_FLOOR_1_ID;
        map[4, 4] = Constants.TILE_WALL_1_ID;
        map[5, 4] = Constants.TILE_FLOOR_1_ID;
        map[6, 4] = Constants.TILE_FLOOR_1_ID;
        map[7, 4] = Constants.TILE_FLOOR_1_ID;
        map[8, 4] = Constants.TILE_FLOOR_1_ID;

        //Row 5
        map[1, 5] = Constants.TILE_FLOOR_1_ID;
        map[2, 5] = Constants.TILE_FLOOR_1_ID;
        map[3, 5] = Constants.TILE_WALL_1_ID;
        map[4, 5] = Constants.TILE_FLOOR_1_ID;
        map[5, 5] = Constants.TILE_FLOOR_1_ID;
        map[6, 5] = Constants.TILE_WALL_1_ID;
        map[7, 5] = Constants.TILE_FLOOR_1_ID;
        map[8, 5] = Constants.TILE_FLOOR_1_ID;

        //Row 6
        map[1, 6] = Constants.TILE_WALL_1_ID;
        map[2, 6] = Constants.TILE_WALL_1_ID;
        map[3, 6] = Constants.TILE_WALL_1_ID;
        map[4, 6] = Constants.TILE_WALL_1_ID;
        map[5, 6] = Constants.TILE_WALL_1_ID;
        map[6, 6] = Constants.TILE_FLOOR_1_ID;
        map[7, 6] = Constants.TILE_FLOOR_1_ID;
        map[8, 6] = Constants.TILE_WALL_1_ID;

        //Row 7
        map[1, 7] = Constants.TILE_WALL_1_ID;
        map[2, 7] = Constants.TILE_WALL_1_ID;
        map[3, 7] = Constants.TILE_FLOOR_1_ID;
        map[4, 7] = Constants.TILE_FLOOR_1_ID;
        map[5, 7] = Constants.TILE_FLOOR_1_ID;
        map[6, 7] = Constants.TILE_WALL_1_ID;
        map[7, 7] = Constants.TILE_FLOOR_1_ID;
        map[8, 7] = Constants.TILE_FLOOR_1_ID;

        //Row 8
        map[1, 8] = Constants.TILE_END_1_ID;
        map[2, 8] = Constants.TILE_FLOOR_1_ID;
        map[3, 8] = Constants.TILE_FLOOR_1_ID;
        map[4, 8] = Constants.TILE_WALL_1_ID;
        map[5, 8] = Constants.TILE_FLOOR_1_ID;
        map[6, 8] = Constants.TILE_FLOOR_1_ID;
        map[7, 8] = Constants.TILE_FLOOR_1_ID;
        map[8, 8] = Constants.TILE_FLOOR_1_ID;

        labyrinth.SetLabyrithDataWitId( map, id);
    }
	

}
