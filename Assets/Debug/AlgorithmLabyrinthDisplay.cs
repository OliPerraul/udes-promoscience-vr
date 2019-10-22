﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Tests
{
    public class AlgorithmLabyrinthDisplay : MonoBehaviour
    {
        [SerializeField]
        ScriptableClientGameState client;

        [SerializeField]
        Labyrinths.Labyrinth labyrinth;

        [SerializeField]
        GameObject sphere;

        List<GameObject> objectList = new List<GameObject>();

        List<Tile> algorithmStepsPosition = new List<Tile>();

        void Start()
        {
            client.clientStateChangedEvent += OnGameStateChanged;
        }


        void OnGameStateChanged()
        {
            if (client.Value == ClientGameState.PlayingTutorial || client.Value == ClientGameState.Playing)
            {
                GenerateVisualForAlgorithm(Utils.Algorithm.ShortestFlightDistance);
            }
        }

        void GenerateVisualForAlgorithm(Utils.Algorithm algorithm)
        {
            algorithmStepsPosition = client.Algorithm.GetAlgorithmSteps();

            Debug.Log("algorithmStepsPosition Count : " + algorithmStepsPosition.Count);

            for (int i = 0; i < algorithmStepsPosition.Count; i++)
            {
                Debug.Log("algorithmStepsPosition #" + i + " : " + algorithmStepsPosition[i].x + " , " + algorithmStepsPosition[i].y + " , " + algorithmStepsPosition[i].color);

                GameObject obj = Instantiate(sphere, GetWorldPosition(algorithmStepsPosition[i].x, algorithmStepsPosition[i].y), Quaternion.identity, gameObject.transform);
                Algorithms.FloorPainter floorPainter = labyrinth.GetTile(algorithmStepsPosition[i].x, algorithmStepsPosition[i].y).GetComponentInChildren<Algorithms.FloorPainter>();

                if (floorPainter != null)
                {
                    floorPainter.PaintFloorWithColor(algorithmStepsPosition[i].color);
                }

                objectList.Add(obj);
            }
        }

        Vector3 GetWorldPosition(int x, int y)
        {
            Vector3 worldPos = new Vector3();

            Vector2Int startPosition = labyrinth.GetLabyrithStartPosition();

            worldPos.x = (x - startPosition.x) * Constants.TILE_SIZE;
            worldPos.y = 0.5f;
            worldPos.z = (-y + startPosition.y) * Constants.TILE_SIZE;

            return worldPos;
        }
    }
}
