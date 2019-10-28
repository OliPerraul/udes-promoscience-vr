using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
//using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.Tests
{
    public class TutorialLabyrinthDataGenerator : MonoBehaviour
    {
        //[SerializeField]
        //ScriptableClientGameState client;

        void Start()
        {
            Client.Instance.clientStateChangedEvent += OnGameStateChanged;
        }

        void OnGameStateChanged()
        {
            if (Client.Instance.State == ClientGameState.GeneratingTutorialLabyrinthDataForTest)
            {
                GenerateTutorialLabyrinthData();
                Client.Instance.State = ClientGameState.TutorialLabyrinthReady;
            }
        }

        public void GenerateTutorialLabyrinthData()
        {
            int id = -3;
            int sizeX = 13;
            int sizeY = 13;

            int[] data = new int[sizeX * sizeY];

            //Colum 0
            data[(0 * sizeY) + 0] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(0 * sizeY) + 1] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 2] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 3] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 4] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 5] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(0 * sizeY) + 6] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 7] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 8] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 9] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 10] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(0 * sizeY) + 11] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_B_ID;
            data[(0 * sizeY) + 12] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;

            //Colum 1
            data[(1 * sizeY) + 0] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
            data[(1 * sizeY) + 1] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 2] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 3] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 4] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 5] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
            data[(1 * sizeY) + 6] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 7] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 8] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 9] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 10] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 11] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(1 * sizeY) + 12] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;

            //Colum 2
            data[(2 * sizeY) + 0] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_B_ID;
            data[(2 * sizeY) + 1] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(2 * sizeY) + 2] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(2 * sizeY) + 3] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(2 * sizeY) + 4] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_B_ID;
            data[(2 * sizeY) + 5] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(2 * sizeY) + 6] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(2 * sizeY) + 7] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_SCAFFOLDING_B_ID;
            data[(2 * sizeY) + 8] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(2 * sizeY) + 9] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_B_ID;
            data[(2 * sizeY) + 10] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(2 * sizeY) + 11] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(2 * sizeY) + 12] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_B_ID;

            //Colum 3
            data[(3 * sizeY) + 0] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
            data[(3 * sizeY) + 1] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 2] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_ID;
            data[(3 * sizeY) + 3] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 4] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 5] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 6] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 7] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 8] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 9] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 10] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 11] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(3 * sizeY) + 12] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;

            //Colum 4
            data[(4 * sizeY) + 0] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_B_ID;
            data[(4 * sizeY) + 1] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(4 * sizeY) + 2] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(4 * sizeY) + 3] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(4 * sizeY) + 4] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(4 * sizeY) + 5] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(4 * sizeY) + 6] = Promoscience.Utils.TILE_KART_TOWER_WALL_2_ID;
            data[(4 * sizeY) + 7] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(4 * sizeY) + 8] = Promoscience.Utils.TILE_KART_TOWER_WALL_2_ID;
            data[(4 * sizeY) + 9] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(4 * sizeY) + 10] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(4 * sizeY) + 11] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(4 * sizeY) + 12] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_B_ID;

            //Colum 5
            data[(5 * sizeY) + 0] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 1] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(5 * sizeY) + 2] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(5 * sizeY) + 3] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(5 * sizeY) + 4] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(5 * sizeY) + 5] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(5 * sizeY) + 6] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 7] = Promoscience.Utils.TILE_KART_END_ID;
            data[(5 * sizeY) + 8] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 9] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(5 * sizeY) + 10] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
            data[(5 * sizeY) + 11] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(5 * sizeY) + 12] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;

            //Colum 6
            data[(6 * sizeY) + 0] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_B_ID;
            data[(6 * sizeY) + 1] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(6 * sizeY) + 2] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(6 * sizeY) + 3] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(6 * sizeY) + 4] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(6 * sizeY) + 5] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(6 * sizeY) + 6] = Promoscience.Utils.TILE_KART_TOWER_WALL_2_ID;
            data[(6 * sizeY) + 7] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(6 * sizeY) + 8] = Promoscience.Utils.TILE_KART_TOWER_WALL_2_ID;
            data[(6 * sizeY) + 9] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(6 * sizeY) + 10] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(6 * sizeY) + 11] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(6 * sizeY) + 12] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_B_ID;

            //Colum 7
            data[(7 * sizeY) + 0] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 1] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 2] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 3] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 4] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
            data[(7 * sizeY) + 5] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 6] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 7] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 8] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 9] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 10] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 11] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(7 * sizeY) + 12] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;

            //Colum 8
            data[(8 * sizeY) + 0] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_B_ID;
            data[(8 * sizeY) + 1] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(8 * sizeY) + 2] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_B_ID;
            data[(8 * sizeY) + 3] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(8 * sizeY) + 4] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_B_ID;
            data[(8 * sizeY) + 5] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(8 * sizeY) + 6] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(8 * sizeY) + 7] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(8 * sizeY) + 8] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_SCAFFOLDING_B_ID;
            data[(8 * sizeY) + 9] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(8 * sizeY) + 10] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_B_ID;
            data[(8 * sizeY) + 11] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(8 * sizeY) + 12] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;

            //Colum 9
            data[(9 * sizeY) + 0] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
            data[(9 * sizeY) + 1] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 2] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
            data[(9 * sizeY) + 3] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 4] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
            data[(9 * sizeY) + 5] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 6] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
            data[(9 * sizeY) + 7] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 8] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 9] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 10] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 11] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(9 * sizeY) + 12] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;

            //Colum 10
            data[(10 * sizeY) + 0] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_B_ID;
            data[(10 * sizeY) + 1] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(10 * sizeY) + 2] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(10 * sizeY) + 3] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(10 * sizeY) + 4] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(10 * sizeY) + 5] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_B_ID;
            data[(10 * sizeY) + 6] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(10 * sizeY) + 7] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(10 * sizeY) + 8] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(10 * sizeY) + 9] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(10 * sizeY) + 10] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(10 * sizeY) + 11] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(10 * sizeY) + 12] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;

            //Colum 11
            data[(11 * sizeY) + 0] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
            data[(11 * sizeY) + 1] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 2] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 3] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 4] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 5] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 6] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 7] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 8] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_ID;
            data[(11 * sizeY) + 9] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 10] = Promoscience.Utils.TILE_KART_FLOOR_ID;
            data[(11 * sizeY) + 11] = Promoscience.Utils.TILE_KART_START_ID;
            data[(11 * sizeY) + 12] = Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;

            //Colum 12
            data[(12 * sizeY) + 0] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(12 * sizeY) + 1] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(12 * sizeY) + 2] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_B_ID;
            data[(12 * sizeY) + 3] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(12 * sizeY) + 4] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_B_ID;
            data[(12 * sizeY) + 5] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(12 * sizeY) + 6] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_B_ID;
            data[(12 * sizeY) + 7] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(12 * sizeY) + 8] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;
            data[(12 * sizeY) + 9] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(12 * sizeY) + 10] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_B_ID;
            data[(12 * sizeY) + 11] = Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID;
            data[(12 * sizeY) + 12] = Promoscience.Utils.TILE_KART_TOWER_WALL_ID;

            Client.Instance.LabyrinthData = new Labyrinths.Data(
                id,
                data,
                sizeX,
                sizeY
                );
            
        }
    }
}
