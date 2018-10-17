using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestFlighDistanceAlgorithm : MonoBehaviour
{
    [SerializeField]
    GameLabyrinth labyrinth;

    readonly int[] xByDirection = { 0, 1, 0, -1 };
    readonly int[] yByDirection = { -1, 0, 1, 0 };

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

        tileColor = (int) FloorColor.Yellow;
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

            float[] directionDistance = new float[4];
            directionDistance[0] = isDirectionWalkableAndNotVisited[0] ? (endPosition - new Vector2Int(position.x + xByDirection[0], position.y + yByDirection[0])).magnitude : -1;
            directionDistance[1] = isDirectionWalkableAndNotVisited[1] ? (endPosition - new Vector2Int(position.x + xByDirection[1], position.y + yByDirection[1])).magnitude : -1;
            directionDistance[2] = isDirectionWalkableAndNotVisited[2] ? (endPosition - new Vector2Int(position.x + xByDirection[2], position.y + yByDirection[2])).magnitude : -1;
            directionDistance[3] = isDirectionWalkableAndNotVisited[3] ? (endPosition - new Vector2Int(position.x + xByDirection[3], position.y + yByDirection[3])).magnitude : -1;


            if (directionDistance[0] != -1 
                && (directionDistance[1] == -1 || directionDistance[0] <= directionDistance[1]) 
                && (directionDistance[2] == -1 || directionDistance[0] <= directionDistance[2]) 
                && (directionDistance[3] == -1 || directionDistance[0] <= directionDistance[3]))
            {
                direction = 0;
                MoveInDirection(direction);
            }
            else if (directionDistance[1] != -1
                && (directionDistance[2] == -1 || directionDistance[1] <= directionDistance[2]) 
                && (directionDistance[3] == -1 || directionDistance[1] <= directionDistance[3]))
            {
                direction = 1;
                MoveInDirection(direction);
            }
            else if (directionDistance[2] != -1
                && (directionDistance[3] == -1 || directionDistance[2] <= directionDistance[3]))
            {
                direction = 2;
                MoveInDirection(direction);
            }
            else if (directionDistance[3] != -1)
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

                algorithmSteps[algorithmSteps.Count - 1] = new Vector3Int(algorithmSteps[algorithmSteps.Count - 1].x, algorithmSteps[algorithmSteps.Count - 1].y, (int) FloorColor.Red);

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
                        algorithmSteps.Add(new Vector3Int(position.x, position.y, tileColor));
                    }
                    else
                    {
                        algorithmSteps.Add(new Vector3Int(algorithmSteps[i].x, algorithmSteps[i].y, (int) FloorColor.Red));
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
