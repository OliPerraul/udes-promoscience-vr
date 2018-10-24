using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmDisplay : MonoBehaviour
{

    [SerializeField]
    ScriptableGameState gameState;

    [SerializeField]
    RightHandAlgorithm rightHandAlgorithm;

    [SerializeField]
    LongestStraightAlgorithm longestStraightAlgorithm;

    [SerializeField]
    ShortestFlighDistanceAlgorithm shortestFlighDistanceAlgorithm;

    [SerializeField]
    StandardAlgorithm standardAlgorithm;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    GameObject sphere;

    List<GameObject> objectList = new List<GameObject>();

    List<Vector3Int> algorithmStepsPosition = new List<Vector3Int>();

    void Start()
    {
        gameState.valueChangedEvent += OnGameStateChanged;
    }


    void OnGameStateChanged()
    {
        if (gameState.Value == GameState.PlayingTutorial || gameState.Value == GameState.Playing)
        {
            GenerateVisualForAlgorithmWithId(Constants.STANDARD_ALGORITHM);
        }
    }

    void GenerateVisualForAlgorithmWithId(int id)
    {
        if (id == Constants.RIGHT_HAND_ALGORITHM)
        {
            algorithmStepsPosition = rightHandAlgorithm.GetAlgorithmSteps();
        }
        else if (id == Constants.LONGEST_STRAIGHT_ALGORITHM)
        {
            algorithmStepsPosition = longestStraightAlgorithm.GetAlgorithmSteps();
        }
        else if (id == Constants.SHORTEST_FLIGHT_DISTANCE_ALGORITHM)
        {
            algorithmStepsPosition = shortestFlighDistanceAlgorithm.GetAlgorithmSteps();
        }
        else if (id == Constants.STANDARD_ALGORITHM)
        {
            algorithmStepsPosition = standardAlgorithm.GetAlgorithmSteps();
        }

        for (int i = 0; i < algorithmStepsPosition.Count; i++)
        {
            GameObject obj = (GameObject)Instantiate(sphere, GetWorldPosition( algorithmStepsPosition[i].x, algorithmStepsPosition[i].y), Quaternion.identity, gameObject.transform);
            FloorPainter floorPainter = labyrinth.GetTile(algorithmStepsPosition[i].x, algorithmStepsPosition[i].y).GetComponentInChildren<FloorPainter>();
            if (floorPainter != null)
            {
                floorPainter.PaintFloorWithColorId(algorithmStepsPosition[i].z);
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
