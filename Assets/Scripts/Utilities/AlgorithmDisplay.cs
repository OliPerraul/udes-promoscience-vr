using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmDisplay : MonoBehaviour
{
    [SerializeField]
    RightHandAlgorithm rightHandAlgorithm;

    [SerializeField]
    LongestStraightAlgorithm longestStraightAlgorithm;

    [SerializeField]
    ShortestFlighDistanceAlgorithm shortestFlighDistanceAlgorithm;

    [SerializeField]
    StandardRecursiveAlgorithm standardRecursiveAlgorithm;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    GameObject sphere;

    List<GameObject> objectList = new List<GameObject>();

    List<Vector2Int> algorithmStepsPosition = new List<Vector2Int>();
    Vector2Int startPosition;

    public bool isFire = false;

    void Update ()
    {
		if (isFire)
        {
            isFire = false;
            GenerateVisualForAlgorithmWithId(Constants.SHORTEST_FLIGHT_DISTANCE_ALGORITH);
        }
	}

    void GenerateVisualForAlgorithmWithId(int id)
    {
        if (id == Constants.RIGHT_HAND_ALGORITH)
        {
            algorithmStepsPosition = rightHandAlgorithm.GetAlgorithmStepsPosition();
        }
        else if (id == Constants.LONGEST_STRAIGHT_ALGORITH)
        {
            algorithmStepsPosition = longestStraightAlgorithm.GetAlgorithmStepsPosition();
        }
        else if (id == Constants.SHORTEST_FLIGHT_DISTANCE_ALGORITH)
        {
            algorithmStepsPosition = shortestFlighDistanceAlgorithm.GetAlgorithmStepsPosition();
        }

        for (int i = 0; i < algorithmStepsPosition.Count; i++)
        {
            GameObject obj = (GameObject)Instantiate(sphere, GetWorldPosition( algorithmStepsPosition[i].x, algorithmStepsPosition[i].y), Quaternion.identity, gameObject.transform);
            objectList.Add(obj);
        }
    }

    Vector3 GetWorldPosition(int x, int y)
    {
        Vector3 worldPos = new Vector3();

        Vector2Int startPosition = labyrinth.GetLabyrithStartPosition();

        worldPos.x = (x - startPosition.x) * Constants.tileSize;
        worldPos.y = 0.5f;
        worldPos.z = (-y + startPosition.y) * Constants.tileSize;

        return worldPos;
    }
}
