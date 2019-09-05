using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.Game;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Tests
{
    public class AlgorithmLabyrinthDisplay : MonoBehaviour
    {
        [SerializeField]
        ScriptableClientGameState gameState;

        [SerializeField]
        RightHandAlgorithm rightHandAlgorithm;

        [SerializeField]
        LongestStraightAlgorithm longestStraightAlgorithm;

        [SerializeField]
        ShortestFlightDistanceAlgorithm shortestFlightDistanceAlgorithm;

        [SerializeField]
        StandardAlgorithm standardAlgorithm;

        [SerializeField]
        GameLabyrinth labyrinth;

        [SerializeField]
        GameObject sphere;

        List<GameObject> objectList = new List<GameObject>();

        List<Tile> algorithmStepsPosition = new List<Tile>();

        void Start()
        {
            gameState.valueChangedEvent += OnGameStateChanged;
        }


        void OnGameStateChanged()
        {
            if (gameState.Value == ClientGameState.PlayingTutorial || gameState.Value == ClientGameState.Playing)
            {
                GenerateVisualForAlgorithm(Algorithm.ShortestFlightDistance);
            }
        }

        void GenerateVisualForAlgorithm(Algorithm algorithm)
        {
            if (algorithm == Algorithm.RightHand)
            {
                algorithmStepsPosition = rightHandAlgorithm.GetAlgorithmSteps();
            }
            else if (algorithm == Algorithm.LongestStraight)
            {
                algorithmStepsPosition = longestStraightAlgorithm.GetAlgorithmSteps();
            }
            else if (algorithm == Algorithm.ShortestFlightDistance)
            {
                algorithmStepsPosition = shortestFlightDistanceAlgorithm.GetAlgorithmSteps();
            }
            else if (algorithm == Algorithm.Standard)
            {
                algorithmStepsPosition = standardAlgorithm.GetAlgorithmSteps();
            }

            Debug.Log("algorithmStepsPosition Count : " + algorithmStepsPosition.Count);

            for (int i = 0; i < algorithmStepsPosition.Count; i++)
            {
                Debug.Log("algorithmStepsPosition #" + i + " : " + algorithmStepsPosition[i].x + " , " + algorithmStepsPosition[i].y + " , " + algorithmStepsPosition[i].color);

                GameObject obj = (GameObject)Instantiate(sphere, GetWorldPosition(algorithmStepsPosition[i].x, algorithmStepsPosition[i].y), Quaternion.identity, gameObject.transform);
                FloorPainter floorPainter = labyrinth.GetTile(algorithmStepsPosition[i].x, algorithmStepsPosition[i].y).GetComponentInChildren<FloorPainter>();

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
