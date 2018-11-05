using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLabyrinthDataGenerator : MonoBehaviour
{
    [SerializeField]
    ScriptableGameState gameState;

    [SerializeField]
    ScriptableLabyrinth labyrinthData;

    int id = -2;//negative id so that it is not mess up with id of those from the database
    int sizeX = 11;
    int sizeY = 11;
    void Start()
    {
        gameState.valueChangedEvent += OnGameStateChanged;
    }

    void OnGameStateChanged()
    {
        if (gameState.Value == GameState.ReadyTutorial)
        {
            GenerateTutorialLabyrinthData();
            gameState.Value = GameState.TutorialLabyrinthReady;
        }
    }

    public void GenerateTutorialLabyrinthData()
    {
        int[,] map = new int[sizeX, sizeY];

        //Row 0
        map[0, 0] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[1, 0] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[2, 0] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[3, 0] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[4, 0] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[5, 0] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[6, 0] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[7, 0] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[8, 0] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[9, 0] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[10, 0] = Constants.TILE_ROME_TOWER_WALL_ID;

        //Row 1
        map[0, 1] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        map[1, 1] = Constants.TILE_ROME_START_ID;
        map[2, 1] = Constants.TILE_ROME_FLOOR_ID;
        map[3, 1] = Constants.TILE_ROME_FLOOR_ID;
        map[4, 1] = Constants.TILE_ROME_FLOOR_ID;
        map[5, 1] = Constants.TILE_ROME_FLOOR_ID;
        map[6, 1] = Constants.TILE_ROME_FLOOR_ID;
        map[7, 1] = Constants.TILE_ROME_FLOOR_ID;
        map[8, 1] = Constants.TILE_ROME_FLOOR_ID;
        map[9, 1] = Constants.TILE_ROME_FLOOR_ID;
        map[10, 1] = Constants.TILE_ROME_VERTICAL_WALL_ID;

        //Row 2
        map[0, 2] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[1, 2] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[2, 2] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[3, 2] = Constants.TILE_ROME_FLOOR_ID;
        map[4, 2] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[5, 2] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[6, 2] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[7, 2] = Constants.TILE_ROME_FLOOR_ID;
        map[8, 2] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[9, 2] = Constants.TILE_ROME_FLOOR_ID;
        map[10, 2] = Constants.TILE_ROME_TOWER_WALL_ID;

        //Row 3
        map[0, 3] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        map[1, 3] = Constants.TILE_ROME_FLOOR_ID;
        map[2, 3] = Constants.TILE_ROME_FLOOR_ID;
        map[3, 3] = Constants.TILE_ROME_FLOOR_ID;
        map[4, 3] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        map[5, 3] = Constants.TILE_ROME_FLOOR_ID;
        map[6, 3] = Constants.TILE_ROME_FLOOR_ID;
        map[7, 3] = Constants.TILE_ROME_FLOOR_ID;
        map[8, 3] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        map[9, 3] = Constants.TILE_ROME_FLOOR_ID;
        map[10, 3] = Constants.TILE_ROME_VERTICAL_WALL_ID;

        //Row 4
        map[0, 4] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[1, 4] = Constants.TILE_ROME_FLOOR_ID;
        map[2, 4] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[3, 4] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[4, 4] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[5, 4] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[6, 4] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[7, 4] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[8, 4] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[9, 4] = Constants.TILE_ROME_FLOOR_ID;
        map[10, 4] = Constants.TILE_ROME_TOWER_WALL_ID;

        //Row 5
        map[0, 5] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        map[1, 5] = Constants.TILE_ROME_FLOOR_ID;
        map[2, 5] = Constants.TILE_ROME_FLOOR_ID;
        map[3, 5] = Constants.TILE_ROME_FLOOR_ID;
        map[4, 5] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        map[5, 5] = Constants.TILE_ROME_END_ID;
        map[6, 5] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        map[7, 5] = Constants.TILE_ROME_FLOOR_ID;
        map[8, 5] = Constants.TILE_ROME_FLOOR_ID;
        map[9, 5] = Constants.TILE_ROME_FLOOR_ID;
        map[10, 5] = Constants.TILE_ROME_VERTICAL_WALL_ID;

        //Row 6
        map[0, 6] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[1, 6] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[2, 6] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[3, 6] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[4, 6] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[5, 6] = Constants.TILE_ROME_FLOOR_ID;
        map[6, 6] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[7, 6] = Constants.TILE_ROME_FLOOR_ID;
        map[8, 6] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[9, 6] = Constants.TILE_ROME_FLOOR_ID;
        map[10, 6] = Constants.TILE_ROME_TOWER_WALL_ID;

        //Row 7
        map[0, 7] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        map[1, 7] = Constants.TILE_ROME_FLOOR_ID;
        map[2, 7] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        map[3, 7] = Constants.TILE_ROME_FLOOR_ID;
        map[4, 7] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        map[5, 7] = Constants.TILE_ROME_FLOOR_ID;
        map[6, 7] = Constants.TILE_ROME_FLOOR_ID;
        map[7, 7] = Constants.TILE_ROME_FLOOR_ID;
        map[8, 7] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        map[9, 7] = Constants.TILE_ROME_FLOOR_ID;
        map[10, 7] = Constants.TILE_ROME_VERTICAL_WALL_ID;

        //Row 8
        map[0, 8] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[1, 8] = Constants.TILE_ROME_FLOOR_ID;
        map[2, 8] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[3, 8] = Constants.TILE_ROME_FLOOR_ID;
        map[4, 8] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[5, 8] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[6, 8] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[7, 8] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[8, 8] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[9, 8] = Constants.TILE_ROME_FLOOR_ID;
        map[10, 8] = Constants.TILE_ROME_TOWER_WALL_ID;

        //Row 9
        map[0, 9] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        map[1, 9] = Constants.TILE_ROME_FLOOR_ID;
        map[2, 9] = Constants.TILE_ROME_FLOOR_ID;
        map[3, 9] = Constants.TILE_ROME_FLOOR_ID;
        map[4, 9] = Constants.TILE_ROME_FLOOR_ID;
        map[5, 9] = Constants.TILE_ROME_FLOOR_ID;
        map[6, 9] = Constants.TILE_ROME_FLOOR_ID;
        map[7, 9] = Constants.TILE_ROME_FLOOR_ID;
        map[8, 9] = Constants.TILE_ROME_FLOOR_ID;
        map[9, 9] = Constants.TILE_ROME_FLOOR_ID;
        map[10, 9] = Constants.TILE_ROME_VERTICAL_WALL_ID;

        //Row 10
        map[0, 10] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[1, 10] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[2, 10] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[3, 10] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[4, 10] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[5, 10] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[6, 10] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[7, 10] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[8, 10] = Constants.TILE_ROME_TOWER_WALL_ID;
        map[9, 10] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        map[10, 10] = Constants.TILE_ROME_TOWER_WALL_ID;

        labyrinthData.SetLabyrithData( map, id);
    }
	

}
