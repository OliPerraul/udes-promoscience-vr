using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardRecursiveAlgorithm : MonoBehaviour
{
    [SerializeField]
    GameLabyrinth labyrinth;

    bool[,] alreadyVisitedPosition;
    bool[,] possiblePath;
    List<int> algorithmSteps = new List<int>();

    public List<int> GetAlgorithmSteps()
    {
        algorithmSteps.Clear();
        alreadyVisitedPosition = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithXLenght()];
        possiblePath = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithXLenght()];

        Vector2Int startPosition = labyrinth.GetLabyrithStartPosition();

        recursiveSolve(startPosition.x, startPosition.y);

        return algorithmSteps;
    }
    
    //a modifier pour obtenir le chemin le plus court seulement
    bool recursiveSolve(int x, int y)
    {
        Vector2Int endPosition = labyrinth.GetLabyrithEndPosition();

        if (x == endPosition.x && y == endPosition.y)
        {
            return true;
        }

        if (labyrinth.GetIsTileWalkable(x,y) || alreadyVisitedPosition[x, y])
        {
            return false;
        }

        alreadyVisitedPosition[x,y] = true;

        if (x != 0)
        {
            if (recursiveSolve(x - 1, y))
            {
                possiblePath[x,y] = true;
                return true;
            }
        }

        if (x != labyrinth.GetLabyrithXLenght() - 1)
        {
            if (recursiveSolve(x + 1, y))
            {
                possiblePath[x, y] = true;
                return true;
            }
        }

        if (y != 0)
        {
            if (recursiveSolve(x, y - 1))
            {
                possiblePath[x, y] = true;
                return true;
            }
        }

        if (y != labyrinth.GetLabyrithYLenght() - 1)
        {
            if (recursiveSolve(x, y + 1))
            {
                possiblePath[x, y] = true;
                return true;
            }
        }

        return false;
    }
}
