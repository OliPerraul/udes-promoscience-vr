using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmDisplay : MonoBehaviour
{

    [SerializeField]
    ScriptableInteger currentGameState;

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
        currentGameState.valueChangedEvent += OnGameStateChanged;
    }


    void OnGameStateChanged()
    {
        if (currentGameState.value == Constants.PLAYING_TUTORIAL || currentGameState.value == Constants.PLAYING)
        {
            GenerateVisualForAlgorithmWithId(Constants.SHORTEST_FLIGHT_DISTANCE_ALGORITH);
        }
    }

    void GenerateVisualForAlgorithmWithId(int id)
    {
        if (id == Constants.RIGHT_HAND_ALGORITH)
        {
            algorithmStepsPosition = rightHandAlgorithm.GetAlgorithmSteps();
        }
        else if (id == Constants.LONGEST_STRAIGHT_ALGORITH)
        {
            algorithmStepsPosition = longestStraightAlgorithm.GetAlgorithmSteps();
        }
        else if (id == Constants.SHORTEST_FLIGHT_DISTANCE_ALGORITH)
        {
            algorithmStepsPosition = shortestFlighDistanceAlgorithm.GetAlgorithmSteps();
        }
        else if (id == Constants.STANDARD_ALGORITH)
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
