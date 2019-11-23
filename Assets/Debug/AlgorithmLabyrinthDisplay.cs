using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience;
using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Tests
{
    public class AlgorithmLabyrinthDisplay : MonoBehaviour
    {
        //[SerializeField]
        //ScriptableClientGameState client;

        [SerializeField]
        GameObject sphere;

        List<GameObject> objectList = new List<GameObject>();

        List<Tile> algorithmStepsPosition = new List<Tile>();

        void Start()
        {
            Client.Instance.clientStateChangedEvent += OnGameStateChanged;
        }

        void OnGameStateChanged()
        {
            if (Client.Instance.State == ClientGameState.PlayingTutorial || Client.Instance.State == ClientGameState.Playing)
            {
                GenerateVisualForAlgorithm(Algorithms.Id.ShortestFlightDistance);
            }
        }

        void GenerateVisualForAlgorithm(Algorithms.Id algorithm)
        {
            algorithmStepsPosition = Client.Instance.Algorithm.GetAlgorithmSteps(Client.Instance.LabyrinthData);

            Debug.Log("algorithmStepsPosition Count : " + algorithmStepsPosition.Count);

            for (int i = 0; i < algorithmStepsPosition.Count; i++)
            {
                Debug.Log("algorithmStepsPosition #" + i + " : " + algorithmStepsPosition[i].x + " , " + algorithmStepsPosition[i].y + " , " + algorithmStepsPosition[i].Color);

                GameObject obj = Instantiate(sphere, GetWorldPosition(algorithmStepsPosition[i].x, algorithmStepsPosition[i].y), Quaternion.identity, gameObject.transform);
                Algorithms.FloorPainter floorPainter = Client.Instance.Labyrinth.GetTile(algorithmStepsPosition[i].x, algorithmStepsPosition[i].y).GetComponentInChildren<Algorithms.FloorPainter>();

                if (floorPainter != null)
                {
                    floorPainter.PaintFloorWithColor(algorithmStepsPosition[i].Color);
                }

                objectList.Add(obj);
            }
        }

        Vector3 GetWorldPosition(int x, int y)
        {
            if (Client.Instance.Labyrinth == null)
                return Vector3.zero;

            Vector3 worldPos = new Vector3();

            Vector2Int startPosition = Client.Instance.Labyrinth.GetLabyrithStartPosition();

            worldPos.x = (x - startPosition.x) * Labyrinths.Utils.TILE_SIZE;
            worldPos.y = 0.5f;
            worldPos.z = (-y + startPosition.y) * Labyrinths.Utils.TILE_SIZE;

            return worldPos;
        }
    }
}
