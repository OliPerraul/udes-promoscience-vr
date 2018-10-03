﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardAlgorithm : MonoBehaviour
{
    [SerializeField]
    GameLabyrinth labyrinth;

    int[] xByDirection = { 0, 1, 0, -1 };
    int[] yByDirection = { -1, 0, 1, 0 };

    public List<Vector2Int> GetAlgorithmSteps()
    {
        int iterCount = 0;
        List<Vector2Int> algorithmStepsPosition = new List<Vector2Int>();
        List<Vector3Int> lastVisitedIntersection = new List<Vector3Int>();

        bool[,] alreadyVisitedTile = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()];

        bool asReachedTheEnd = false;

        int direction = 0;//Hardcoded start direction, could be get from deadend start exist to optimise
        Vector2Int position = labyrinth.GetLabyrithStartPosition();
        Vector2Int endPosition = labyrinth.GetLabyrithEndPosition();
        algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));

        while (!asReachedTheEnd)
        {
            iterCount++;//temp

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
                    lastVisitedIntersection.Add(new Vector3Int(position.x, position.y, algorithmStepsPosition.Count - 1));
                }

                alreadyVisitedTile[position.x, position.y] = true;
            }

            if (isDirectionWalkableAndNotVisited[0])
            {
                direction = 0;
                position.x += xByDirection[direction];
                position.y += yByDirection[direction];
                algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));
            }
            else if (isDirectionWalkableAndNotVisited[1])
            {
                direction = 1;
                position.x += xByDirection[direction];
                position.y += yByDirection[direction];
                algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));
            }
            else if (isDirectionWalkableAndNotVisited[2])
            {
                direction = 2;
                position.x += xByDirection[direction];
                position.y += yByDirection[direction];
                algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));
            }
            else if (isDirectionWalkableAndNotVisited[3])
            {
                direction = 3;
                position.x += xByDirection[direction];
                position.y += yByDirection[direction];
                algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));
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
                        //direction = (direction + 3) % 4;//Not usefull for position based movement in this case
                        position.x = algorithmStepsPosition[i].x;
                        position.y = algorithmStepsPosition[i].y;
                    }

                    i--;
                }
            }

            if ((position.x == endPosition.x && position.y == endPosition.y) || iterCount > 100)
            {
                asReachedTheEnd = true;
            }
        }

        return algorithmStepsPosition;
    }
}