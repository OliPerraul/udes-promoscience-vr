using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLabyrinthDataGenerator : MonoBehaviour
{
    [SerializeField]
    ScriptableClientGameState gameState;

    [SerializeField]
    ScriptableLabyrinth labyrinthData;

    void Start()
    {
        gameState.valueChangedEvent += OnGameStateChanged;
    }

    void OnGameStateChanged()
    {
        if (gameState.Value == ClientGameState.ReadyTutorial)
        {
            GenerateTutorialLabyrinthData();
            gameState.Value = ClientGameState.TutorialLabyrinthReady;
        }
    }

    public void GenerateTutorialLabyrinthData()
    {
        int id = -2;
        int sizeX = 11;
        int sizeY = 11;

        int[] data = new int[sizeX * sizeY];

        //Colum 0
        data[(0 * sizeY) + 0] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(0 * sizeY) + 1] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(0 * sizeY) + 2] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
        data[(0 * sizeY) + 3] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(0 * sizeY) + 4] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
        data[(0 * sizeY) + 5] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(0 * sizeY) + 6] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
        data[(0 * sizeY) + 7] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(0 * sizeY) + 8] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
        data[(0 * sizeY) + 9] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(0 * sizeY) + 10] = Constants.TILE_ROME_TOWER_WALL_ID;

        //Colum 1
        data[(1 * sizeY) + 0] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        data[(1 * sizeY) + 1] = Constants.TILE_ROME_START_ID;
        data[(1 * sizeY) + 2] = Constants.TILE_ROME_FLOOR_ID;
        data[(1 * sizeY) + 3] = Constants.TILE_ROME_FLOOR_ID;
        data[(1 * sizeY) + 4] = Constants.TILE_ROME_FLOOR_ID;
        data[(1 * sizeY) + 5] = Constants.TILE_ROME_FLOOR_ID;
        data[(1 * sizeY) + 6] = Constants.TILE_ROME_FLOOR_ID;
        data[(1 * sizeY) + 7] = Constants.TILE_ROME_FLOOR_ID;
        data[(1 * sizeY) + 8] = Constants.TILE_ROME_FLOOR_ID;
        data[(1 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
        data[(1 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;

        //Colum 2
        data[(2 * sizeY) + 0] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(2 * sizeY) + 1] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(2 * sizeY) + 2] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(2 * sizeY) + 3] = Constants.TILE_ROME_FLOOR_ID;
        data[(2 * sizeY) + 4] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(2 * sizeY) + 5] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(2 * sizeY) + 6] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(2 * sizeY) + 7] = Constants.TILE_ROME_FLOOR_ID;
        data[(2 * sizeY) + 8] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(2 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
        data[(2 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_B_ID;

        //Colum 3
        data[(3 * sizeY) + 0] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        data[(3 * sizeY) + 1] = Constants.TILE_ROME_FLOOR_ID;
        data[(3 * sizeY) + 2] = Constants.TILE_ROME_FLOOR_ID;
        data[(3 * sizeY) + 3] = Constants.TILE_ROME_FLOOR_ID;
        data[(3 * sizeY) + 4] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        data[(3 * sizeY) + 5] = Constants.TILE_ROME_FLOOR_ID;
        data[(3 * sizeY) + 6] = Constants.TILE_ROME_FLOOR_ID;
        data[(3 * sizeY) + 7] = Constants.TILE_ROME_FLOOR_ID;
        data[(3 * sizeY) + 8] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        data[(3 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
        data[(3 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;

        //Colum 4
        data[(4 * sizeY) + 0] = Constants.TILE_ROME_HORIZONTAL_WALL_B_ID;
        data[(4 * sizeY) + 1] = Constants.TILE_ROME_FLOOR_ID;
        data[(4 * sizeY) + 2] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(4 * sizeY) + 3] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(4 * sizeY) + 4] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(4 * sizeY) + 5] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(4 * sizeY) + 6] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(4 * sizeY) + 7] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(4 * sizeY) + 8] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(4 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
        data[(4 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_B_ID;

        //Colum 5
        data[(5 * sizeY) + 0] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        data[(5 * sizeY) + 1] = Constants.TILE_ROME_FLOOR_ID;
        data[(5 * sizeY) + 2] = Constants.TILE_ROME_FLOOR_ID;
        data[(5 * sizeY) + 3] = Constants.TILE_ROME_FLOOR_ID;
        data[(5 * sizeY) + 4] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        data[(5 * sizeY) + 5] = Constants.TILE_ROME_END_ID;
        data[(5 * sizeY) + 6] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        data[(5 * sizeY) + 7] = Constants.TILE_ROME_FLOOR_ID;
        data[(5 * sizeY) + 8] = Constants.TILE_ROME_FLOOR_ID;
        data[(5 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
        data[(5 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;

        //Colum 6
        data[(6 * sizeY) + 0] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(6 * sizeY) + 1] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(6 * sizeY) + 2] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(6 * sizeY) + 3] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(6 * sizeY) + 4] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(6 * sizeY) + 5] = Constants.TILE_ROME_FLOOR_ID;
        data[(6 * sizeY) + 6] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(6 * sizeY) + 7] = Constants.TILE_ROME_FLOOR_ID;
        data[(6 * sizeY) + 8] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(6 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
        data[(6 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_B_ID;

        //Colum 7
        data[(7 * sizeY) + 0] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        data[(7 * sizeY) + 1] = Constants.TILE_ROME_FLOOR_ID;
        data[(7 * sizeY) + 2] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        data[(7 * sizeY) + 3] = Constants.TILE_ROME_FLOOR_ID;
        data[(7 * sizeY) + 4] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        data[(7 * sizeY) + 5] = Constants.TILE_ROME_FLOOR_ID;
        data[(7 * sizeY) + 6] = Constants.TILE_ROME_FLOOR_ID;
        data[(7 * sizeY) + 7] = Constants.TILE_ROME_FLOOR_ID;
        data[(7 * sizeY) + 8] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        data[(7 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
        data[(7 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;

        //Colum 8
        data[(8 * sizeY) + 0] = Constants.TILE_ROME_HORIZONTAL_WALL_B_ID;
        data[(8 * sizeY) + 1] = Constants.TILE_ROME_FLOOR_ID;
        data[(8 * sizeY) + 2] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(8 * sizeY) + 3] = Constants.TILE_ROME_FLOOR_ID;
        data[(8 * sizeY) + 4] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(8 * sizeY) + 5] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(8 * sizeY) + 6] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
        data[(8 * sizeY) + 7] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(8 * sizeY) + 8] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(8 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
        data[(8 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_B_ID;

        //Colum 9
        data[(9 * sizeY) + 0] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;
        data[(9 * sizeY) + 1] = Constants.TILE_ROME_FLOOR_ID;
        data[(9 * sizeY) + 2] = Constants.TILE_ROME_FLOOR_ID;
        data[(9 * sizeY) + 3] = Constants.TILE_ROME_FLOOR_ID;
        data[(9 * sizeY) + 4] = Constants.TILE_ROME_FLOOR_ID;
        data[(9 * sizeY) + 5] = Constants.TILE_ROME_FLOOR_ID;
        data[(9 * sizeY) + 6] = Constants.TILE_ROME_FLOOR_ID;
        data[(9 * sizeY) + 7] = Constants.TILE_ROME_FLOOR_ID;
        data[(9 * sizeY) + 8] = Constants.TILE_ROME_FLOOR_ID;
        data[(9 * sizeY) + 9] = Constants.TILE_ROME_FLOOR_ID;
        data[(9 * sizeY) + 10] = Constants.TILE_ROME_HORIZONTAL_WALL_ID;

        //Colum 10
        data[(10 * sizeY) + 0] = Constants.TILE_ROME_TOWER_WALL_ID;
        data[(10 * sizeY) + 1] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(10 * sizeY) + 2] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
        data[(10 * sizeY) + 3] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(10 * sizeY) + 4] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
        data[(10 * sizeY) + 5] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(10 * sizeY) + 6] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
        data[(10 * sizeY) + 7] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(10 * sizeY) + 8] = Constants.TILE_ROME_VERTICAL_WALL_B_ID;
        data[(10 * sizeY) + 9] = Constants.TILE_ROME_VERTICAL_WALL_ID;
        data[(10 * sizeY) + 10] = Constants.TILE_ROME_TOWER_WALL_ID;

        labyrinthData.SetLabyrithData(data, sizeX, sizeY, id);
    }
	

}
