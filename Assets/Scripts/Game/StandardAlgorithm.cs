using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardAlgorithm : MonoBehaviour
{
    [SerializeField]
    GameLabyrinth labyrinth;

    int[] xByDirection = { 0, 1, 0, -1 };
    int[] yByDirection = { -1, 0, 1, 0 };

    int tileColor;
    Vector2Int position;

    //Steps the two first value are the map position and the third value is the tile color value
    List<Vector3Int> algorithmSteps;

    public List<Vector3Int> GetAlgorithmSteps()
    {
        algorithmSteps = new List<Vector3Int>();
        //lastVisitedIntersection the two first value are the map position and the third value is the step number to get to the intersection
        List<Vector3Int> lastVisitedIntersection = new List<Vector3Int>();

        bool[,] alreadyVisitedTile = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()];

        bool asReachedTheEnd = false;

        int direction = labyrinth.GetStartDirection();

        tileColor = Constants.YELLOW_COLOR_ID;
        position = labyrinth.GetLabyrithStartPosition();
        Vector2Int endPosition = labyrinth.GetLabyrithEndPosition();

        algorithmSteps.Add(new Vector3Int(position.x, position.y, tileColor));

        while (!asReachedTheEnd)
        {
            bool[] isDirectionWalkableAndNotVisited = new bool[4];
            isDirectionWalkableAndNotVisited[0] = labyrinth.GetIsTileWalkable(position.x + xByDirection[0], position.y + yByDirection[0]) && !alreadyVisitedTile[position.x + xByDirection[0], position.y + yByDirection[0]];
            isDirectionWalkableAndNotVisited[1] = labyrinth.GetIsTileWalkable(position.x + xByDirection[1], position.y + yByDirection[1]) && !alreadyVisitedTile[position.x + xByDirection[1], position.y + yByDirection[1]];
            isDirectionWalkableAndNotVisited[2] = labyrinth.GetIsTileWalkable(position.x + xByDirection[2], position.y + yByDirection[2]) && !alreadyVisitedTile[position.x + xByDirection[2], position.y + yByDirection[2]];
            isDirectionWalkableAndNotVisited[3] = labyrinth.GetIsTileWalkable(position.x + xByDirection[3], position.y + yByDirection[3]) && !alreadyVisitedTile[position.x + xByDirection[3], position.y + yByDirection[3]];

            if (!alreadyVisitedTile[position.x, position.y])//Should be changed if we don't start in a deadend
            {
                if ((isDirectionWalkableAndNotVisited[direction] && isDirectionWalkableAndNotVisited[(direction + 1) % 4])
                   || (isDirectionWalkableAndNotVisited[direction] && isDirectionWalkableAndNotVisited[(direction + 3) % 4])
                   || (isDirectionWalkableAndNotVisited[(direction + 1) % 4] && isDirectionWalkableAndNotVisited[(direction + 3) % 4]))
                {
                    lastVisitedIntersection.Add(new Vector3Int(position.x, position.y, algorithmSteps.Count - 1));
                }

                alreadyVisitedTile[position.x, position.y] = true;
            }

            if (isDirectionWalkableAndNotVisited[0])
            {
                direction = 0;
                MoveInDirection(direction);
            }
            else if (isDirectionWalkableAndNotVisited[1])
            {
                direction = 1;
                MoveInDirection(direction);
            }
            else if (isDirectionWalkableAndNotVisited[2])
            {
                direction = 2;
                MoveInDirection(direction);
            }
            else if (isDirectionWalkableAndNotVisited[3])
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
                    i = algorithmSteps.Count - 2;
                }

                algorithmSteps[algorithmSteps.Count - 1] = new Vector3Int(algorithmSteps[algorithmSteps.Count - 1].x, algorithmSteps[algorithmSteps.Count - 1].y, Constants.RED_COLOR_ID);

                bool isReturnedToLastIntersection = false;

                while (!isReturnedToLastIntersection)
                {
                    if (i < 0)
                    {
                        //Labyrith is impossible!
                        return algorithmSteps;
                    }

                    if (algorithmSteps[i].x == lastVisitedIntersection[lastVisitedIntersection.Count - 1].x && algorithmSteps[i].y == lastVisitedIntersection[lastVisitedIntersection.Count - 1].y)
                    {
                        isReturnedToLastIntersection = true;
                        position.x = algorithmSteps[i].x;
                        position.y = algorithmSteps[i].y;
                        algorithmSteps.Add(new Vector3Int(position.x, position.y, Constants.YELLOW_COLOR_ID));
                    }
                    else
                    {
                        algorithmSteps.Add(new Vector3Int(algorithmSteps[i].x, algorithmSteps[i].y, Constants.RED_COLOR_ID));
                    }

                    i--;
                }
            }

            if (position.x == endPosition.x && position.y == endPosition.y)
            {
                asReachedTheEnd = true;
            }
        }

        return algorithmSteps;
    }

    void MoveInDirection(int direction)
    {
        position.x += xByDirection[direction];
        position.y += yByDirection[direction];
        algorithmSteps.Add(new Vector3Int(position.x, position.y, tileColor));
    }
}
