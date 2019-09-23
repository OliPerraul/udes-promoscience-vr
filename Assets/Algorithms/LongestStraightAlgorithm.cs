﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience
{
    public class LongestStraightAlgorithm : MonoBehaviour
    {
        [SerializeField]
        Labyrinth labyrinth;

        bool asReachedTheEnd;
        bool[] isDirectionWalkableAndNotVisited = new bool[4];
        bool[,] alreadyVisitedTile;

        readonly int[] xByDirection = { 0, 1, 0, -1 };
        readonly int[] yByDirection = { -1, 0, 1, 0 };

        int[] directionStraight = new int[4];

        TileColor tileColor;

        Vector2Int position;
        Vector2Int endPosition;

        List<Tile> algorithmSteps;

        //lastVisitedIntersection the two first value are the map position and the third value is the step number to get to the intersection
        List<Vector3Int> lastVisitedIntersection;

        public List<Tile> GetAlgorithmSteps()
        {
            algorithmSteps = new List<Tile>();
            lastVisitedIntersection = new List<Vector3Int>();

            alreadyVisitedTile = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()];

            asReachedTheEnd = false;

            int direction = labyrinth.GetStartDirection();

            tileColor = TileColor.Yellow;
            position = labyrinth.GetLabyrithStartPosition();
            endPosition = labyrinth.GetLabyrithEndPosition();

            algorithmSteps.Add(new Tile(position.x, position.y, tileColor));

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
                        i = algorithmSteps.Count - 2;
                    }

                    algorithmSteps[algorithmSteps.Count - 1] = new Tile(algorithmSteps[algorithmSteps.Count - 1].x, algorithmSteps[algorithmSteps.Count - 1].y, TileColor.Red);

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
                            algorithmSteps.Add(new Tile(position.x, position.y, tileColor));
                        }
                        else
                        {
                            algorithmSteps.Add(new Tile(algorithmSteps[i].x, algorithmSteps[i].y, TileColor.Red));
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
                        lastVisitedIntersection.Add(new Vector3Int(position.x, position.y, algorithmSteps.Count));
                    }

                    alreadyVisitedTile[position.x, position.y] = true;
                }

                algorithmSteps.Add(new Tile(position.x, position.y, tileColor));


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

            while (labyrinth.GetIsTileWalkable(position.x + xByDirection[(direction) % 4], position.y + yByDirection[(direction) % 4]) && !alreadyVisitedTile[position.x + xByDirection[(direction) % 4], position.y + yByDirection[(direction) % 4]])
            {
                straightLenght++;
                position.x += xByDirection[direction];
                position.y += yByDirection[direction];
            }

            return straightLenght;
        }
    }
}