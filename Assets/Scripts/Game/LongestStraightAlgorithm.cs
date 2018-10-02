using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongestStraightAlgorithm : MonoBehaviour
{
    [SerializeField]
    GameLabyrinth labyrinth;

    int[] xByDirection = { 0, 1, 0, -1 };
    int[] yByDirection = { -1, 0, 1, 0 };

    public List<Vector2Int> GetAlgorithmSteps()
    {
        int iterCount = 0;
        List<Vector2Int> algorithmStepsPosition = new List<Vector2Int>();
        List<Vector2Int> lastVisitedIntersection = new List<Vector2Int>();

        bool[,] alreadyVisitedTile = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()];

        bool asReachedTheEnd = false;

        int direction = 0;//Hardcoded start direction
        Vector2Int position = labyrinth.GetLabyrithStartPosition();
        Vector2Int endPosition = labyrinth.GetLabyrithEndPosition();
        algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));

        alreadyVisitedTile[position.x, position.y] = true;

        while (!asReachedTheEnd)
        {
            int[] directionStraight = new int[4];
            directionStraight[0] = alreadyVisitedTile[position.x + xByDirection[0], position.y + yByDirection[0]] ? 0 : GetStraightLenghtInDirection(new Vector2Int(position.x, position.y), 0);
            directionStraight[1] = alreadyVisitedTile[position.x + xByDirection[1], position.y + yByDirection[1]] ? 0 : GetStraightLenghtInDirection(new Vector2Int(position.x, position.y), 1);
            directionStraight[2] = alreadyVisitedTile[position.x + xByDirection[2], position.y + yByDirection[2]] ? 0 : GetStraightLenghtInDirection(new Vector2Int(position.x, position.y), 2);
            directionStraight[3] = alreadyVisitedTile[position.x + xByDirection[3], position.y + yByDirection[3]] ? 0 : GetStraightLenghtInDirection(new Vector2Int(position.x, position.y), 3);

            if (directionStraight[0] > 0 && directionStraight[0] >= directionStraight[1] && directionStraight[0] >= directionStraight[2] && directionStraight[0] >= directionStraight[3])
            {
                direction = 0;

                for (int i = 0; i < directionStraight[0]; i++)
                {
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];


                    bool[] isDirectionWalkable = new bool[4];
                    isDirectionWalkable[0] = labyrinth.GetIsTileWalkable(position.x + xByDirection[0], position.y + yByDirection[0]);
                    isDirectionWalkable[1] = labyrinth.GetIsTileWalkable(position.x + xByDirection[1], position.y + yByDirection[1]);
                    isDirectionWalkable[2] = labyrinth.GetIsTileWalkable(position.x + xByDirection[2], position.y + yByDirection[2]);
                    isDirectionWalkable[3] = labyrinth.GetIsTileWalkable(position.x + xByDirection[3], position.y + yByDirection[3]);

                    if (!alreadyVisitedTile[position.x, position.y])//Should be changed if we don't start in a deadend
                    {
                        if (isDirectionWalkable[direction] && isDirectionWalkable[(direction + 1) % 4]
                            || isDirectionWalkable[direction] && isDirectionWalkable[(direction + 3) % 4])
                        {
                            lastVisitedIntersection.Add(new Vector2Int(position.x, position.y));
                        }
                    }

                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));
                  
                     
                    iterCount++;//temp
                    if ((position.x == endPosition.x && position.y == endPosition.y) || iterCount > 100)
                    {
                        asReachedTheEnd = true;
                        break;
                    }
                }
            }
            else if (directionStraight[1] > 0 && directionStraight[1] >= directionStraight[2] && directionStraight[1] >= directionStraight[3])
            {
                direction = 1;

                for (int i = 0; i < directionStraight[1]; i++)
                {
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];

                    bool[] isDirectionWalkable = new bool[4];
                    isDirectionWalkable[0] = labyrinth.GetIsTileWalkable(position.x + xByDirection[0], position.y + yByDirection[0]);
                    isDirectionWalkable[1] = labyrinth.GetIsTileWalkable(position.x + xByDirection[1], position.y + yByDirection[1]);
                    isDirectionWalkable[2] = labyrinth.GetIsTileWalkable(position.x + xByDirection[2], position.y + yByDirection[2]);
                    isDirectionWalkable[3] = labyrinth.GetIsTileWalkable(position.x + xByDirection[3], position.y + yByDirection[3]);

                    if (!alreadyVisitedTile[position.x, position.y])//Should be changed if we don't start in a deadend
                    {
                        if (isDirectionWalkable[direction] && isDirectionWalkable[(direction + 1) % 4]
                            || isDirectionWalkable[direction] && isDirectionWalkable[(direction + 3) % 4])
                        {
                            lastVisitedIntersection.Add(new Vector2Int(position.x, position.y));
                        }
                    }

                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));
                    Debug.Log("position : " + position + " direction : " + direction);//temp

                    iterCount++;//temp
                    if ((position.x == endPosition.x && position.y == endPosition.y) || iterCount > 100)
                    {
                        asReachedTheEnd = true;
                        break;
                    }
                }
            }
            else if (directionStraight[2] > 0 && directionStraight[2] >= directionStraight[3])
            {
                direction = 2;

                for (int i = 0; i < directionStraight[2]; i++)
                {
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];

                    bool[] isDirectionWalkable = new bool[4];
                    isDirectionWalkable[0] = labyrinth.GetIsTileWalkable(position.x + xByDirection[0], position.y + yByDirection[0]);
                    isDirectionWalkable[1] = labyrinth.GetIsTileWalkable(position.x + xByDirection[1], position.y + yByDirection[1]);
                    isDirectionWalkable[2] = labyrinth.GetIsTileWalkable(position.x + xByDirection[2], position.y + yByDirection[2]);
                    isDirectionWalkable[3] = labyrinth.GetIsTileWalkable(position.x + xByDirection[3], position.y + yByDirection[3]);

                    if (!alreadyVisitedTile[position.x, position.y])//Should be changed if we don't start in a deadend
                    {
                        if (isDirectionWalkable[direction] && isDirectionWalkable[(direction + 1) % 4]
                            || isDirectionWalkable[direction] && isDirectionWalkable[(direction + 3) % 4])
                        {
                            lastVisitedIntersection.Add(new Vector2Int(position.x, position.y));
                        }
                    }

                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));
                    Debug.Log("position : " + position + " direction : " + direction);//temp

                    iterCount++;//temp
                    if ((position.x == endPosition.x && position.y == endPosition.y) || iterCount > 100)
                    {
                        asReachedTheEnd = true;
                        break;
                    }
                }
            }
            else if (directionStraight[3] > 0)
            {
                direction = 3;

                for (int i = 0; i < directionStraight[3]; i++)
                {
                    //wrap dans une fonction? dépend du fonctionnement de l'algorithme
                    position.x += xByDirection[direction];
                    position.y += yByDirection[direction];

                    bool[] isDirectionWalkable = new bool[4];
                    isDirectionWalkable[0] = labyrinth.GetIsTileWalkable(position.x + xByDirection[0], position.y + yByDirection[0]);
                    isDirectionWalkable[1] = labyrinth.GetIsTileWalkable(position.x + xByDirection[1], position.y + yByDirection[1]);
                    isDirectionWalkable[2] = labyrinth.GetIsTileWalkable(position.x + xByDirection[2], position.y + yByDirection[2]);
                    isDirectionWalkable[3] = labyrinth.GetIsTileWalkable(position.x + xByDirection[3], position.y + yByDirection[3]);

                    if (!alreadyVisitedTile[position.x, position.y])//Should be changed if we don't start in a deadend
                    {
                        if (isDirectionWalkable[direction] && isDirectionWalkable[(direction + 1) % 4]
                            || isDirectionWalkable[direction] && isDirectionWalkable[(direction + 3) % 4])
                        {
                            lastVisitedIntersection.Add(new Vector2Int(position.x, position.y));
                        }
                    }

                    alreadyVisitedTile[position.x, position.y] = true;
                    algorithmStepsPosition.Add(new Vector2Int(position.x, position.y));
                    Debug.Log("position : " + position + " direction : " + direction);//temp

                    iterCount++;//temp
                    if ((position.x == endPosition.x && position.y == endPosition.y) || iterCount > 100)
                    {
                        asReachedTheEnd = true;
                        break;
                    }
                }
            }
            else
            {
                if (position == lastVisitedIntersection[lastVisitedIntersection.Count - 1])
                {
                    lastVisitedIntersection.RemoveAt(lastVisitedIntersection.Count - 1);
                }

                bool isReturnedToLastIntersection = false;
                int i = algorithmStepsPosition.Count - 2;
                while (!isReturnedToLastIntersection)
                {
                    if (i < 0)
                    {
                        //Labyrith is impossible!
                        return algorithmStepsPosition;
                    }

                    algorithmStepsPosition.Add(algorithmStepsPosition[i]);

                    if (algorithmStepsPosition[i] == lastVisitedIntersection[lastVisitedIntersection.Count - 1])
                    {
                        isReturnedToLastIntersection = true;
                        //direction = (direction + 3) % 4;//Not usefull for position based movement in this case
                        position.x = algorithmStepsPosition[i].x;
                        position.y = algorithmStepsPosition[i].y;
                    }

                    i--;
                }
            }


            iterCount++;//temp
            if ((position.x == endPosition.x && position.y == endPosition.y) || iterCount > 100)
            {
                asReachedTheEnd = true;
            }
        }

        return algorithmStepsPosition;
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
