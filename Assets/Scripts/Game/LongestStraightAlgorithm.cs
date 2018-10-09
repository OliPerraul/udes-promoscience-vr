using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongestStraightAlgorithm : MonoBehaviour
{
    [SerializeField]
    GameLabyrinth labyrinth;

    int[] xByDirection = { 0, 1, 0, -1 };
    int[] yByDirection = { -1, 0, 1, 0 };
    int[] directionStraight = new int[4];

    bool asReachedTheEnd;
    bool[] isDirectionWalkableAndNotVisited = new bool[4];
    bool[,] alreadyVisitedTile;

    Vector2Int position;
    Vector2Int endPosition;

    List<Vector2Int> algorithmStepsPosition;
    List<Vector3Int> lastVisitedIntersection;

    public List<Vector2Int> GetAlgorithmSteps()
    {
        algorithmStepsPosition = new List<Vector2Int>();
        lastVisitedIntersection = new List<Vector3Int>();

        alreadyVisitedTile = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()];

        asReachedTheEnd = false;

        int direction = 0;//Hardcoded start direction
        position = labyrinth.GetLabyrithStartPosition();
        endPosition = labyrinth.GetLabyrithEndPosition();
        algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));

        alreadyVisitedTile[position.x, position.y] = true;

        while (!asReachedTheEnd)
        {
            directionStraight[0] = alreadyVisitedTile[position.x + xByDirection[0], position.y + yByDirection[0]] ? 0 : GetStraightLenghtInDirection(new Vector2Int(position.x, position.y), 0);
            directionStraight[1] = alreadyVisitedTile[position.x + xByDirection[1], position.y + yByDirection[1]] ? 0 : GetStraightLenghtInDirection(new Vector2Int(position.x, position.y), 1);
            directionStraight[2] = alreadyVisitedTile[position.x + xByDirection[2], position.y + yByDirection[2]] ? 0 : GetStraightLenghtInDirection(new Vector2Int(position.x, position.y), 2);
            directionStraight[3] = alreadyVisitedTile[position.x + xByDirection[3], position.y + yByDirection[3]] ? 0 : GetStraightLenghtInDirection(new Vector2Int(position.x, position.y), 3);

            if (directionStraight[0] > 0 && directionStraight[0] >= directionStraight[1] && directionStraight[0] >= directionStraight[2] && directionStraight[0] >= directionStraight[3])
            {
                direction = 0;
                MoveInDirection(direction);
            }
            else if (directionStraight[1] > 0 && directionStraight[1] >= directionStraight[2] && directionStraight[1] >= directionStraight[3])
            {
                direction = 1;
                MoveInDirection(direction);
            }
            else if (directionStraight[2] > 0 && directionStraight[2] >= directionStraight[3])
            {
                direction = 2;
                MoveInDirection(direction);
            }
            else if (directionStraight[3] > 0)
            {
                direction = 3;
                MoveInDirection(direction);
            }
            else
            {
                int i;
                if (position.x == lastVisitedIntersection[lastVisitedIntersection.Count - 1].x && position.y == lastVisitedIntersection[lastVisitedIntersection.Count - 1].y)
                {
                    i = lastVisitedIntersection[lastVisitedIntersection.Count - 1].z - 1;
                    lastVisitedIntersection.RemoveAt(lastVisitedIntersection.Count - 1);
                }
                else
                {
                    i = algorithmStepsPosition.Count - 2;
                }

                bool isReturnedToLastIntersection = false;
               
                while (!isReturnedToLastIntersection)
                {
                    if (i < 0)
                    {
                        //Labyrith is impossible!
                        return algorithmStepsPosition;
                    }

                    algorithmStepsPosition.Add(algorithmStepsPosition[i]);

                    if (algorithmStepsPosition[i].x == lastVisitedIntersection[lastVisitedIntersection.Count - 1].x && algorithmStepsPosition[i].y == lastVisitedIntersection[lastVisitedIntersection.Count - 1].y)
                    {
                        isReturnedToLastIntersection = true;
                        position.x = algorithmStepsPosition[i].x;
                        position.y = algorithmStepsPosition[i].y;
                    }

                    i--;
                }
            }

            if (position.x == endPosition.x && position.y == endPosition.y)
            {
                asReachedTheEnd = true;
            }
        }

        return algorithmStepsPosition;
    }

    void MoveInDirection(int direction)
    {
        for (int i = 0; i < directionStraight[direction]; i++)
        {
            position.x += xByDirection[direction];
            position.y += yByDirection[direction];

            isDirectionWalkableAndNotVisited[0] = labyrinth.GetIsTileWalkable(position.x + xByDirection[0], position.y + yByDirection[0]) && !alreadyVisitedTile[position.x + xByDirection[0], position.y + yByDirection[0]];
            isDirectionWalkableAndNotVisited[1] = labyrinth.GetIsTileWalkable(position.x + xByDirection[1], position.y + yByDirection[1]) && !alreadyVisitedTile[position.x + xByDirection[1], position.y + yByDirection[1]];
            isDirectionWalkableAndNotVisited[2] = labyrinth.GetIsTileWalkable(position.x + xByDirection[2], position.y + yByDirection[2]) && !alreadyVisitedTile[position.x + xByDirection[2], position.y + yByDirection[2]];
            isDirectionWalkableAndNotVisited[3] = labyrinth.GetIsTileWalkable(position.x + xByDirection[3], position.y + yByDirection[3]) && !alreadyVisitedTile[position.x + xByDirection[3], position.y + yByDirection[3]];

            if (!alreadyVisitedTile[position.x, position.y])
            {
                if ((isDirectionWalkableAndNotVisited[direction] && isDirectionWalkableAndNotVisited[(direction + 1) % 4])
                   || (isDirectionWalkableAndNotVisited[direction] && isDirectionWalkableAndNotVisited[(direction + 3) % 4])
                   || (isDirectionWalkableAndNotVisited[(direction + 1) % 4] && isDirectionWalkableAndNotVisited[(direction + 3) % 4]))
                {
                    lastVisitedIntersection.Add(new Vector3Int(position.x, position.y, algorithmStepsPosition.Count - 1));
                }

                alreadyVisitedTile[position.x, position.y] = true;
            }

            algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));


            if (position.x == endPosition.x && position.y == endPosition.y)
            {
                asReachedTheEnd = true;
                break;
            }
        }
    }

    int GetStraightLenghtInDirection(Vector2Int position, int direction)
    {
        int straightLenght = 0;

        while(labyrinth.GetIsTileWalkable(position.x + xByDirection[(direction) % 4], position.y + yByDirection[(direction) % 4]))
        {
            straightLenght++;
            position.x += xByDirection[direction];
            position.y += yByDirection[direction];
        }

        return straightLenght;
    }
}
