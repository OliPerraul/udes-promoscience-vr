﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.Labyrinths;

namespace UdeS.Promoscience.Algorithms
{
    public class StandardAlgorithm : Algorithm
    {
        readonly int[] xByDirection = { 0, 1, 0, -1 };

        readonly int[] yByDirection = { -1, 0, 1, 0 };

        TileColor tileColor;

        Vector2Int position;

        List<Tile> algorithmSteps;


        public override Id Id => Id.Standard;

        public override Direction[] GetPrioritizedDirections(AlgorithmExecutionState state, ILabyrinth labyrinth)
        {
            return new Direction[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left };
        }


        //public override List<Tile> GetAlgorithmSteps(Labyrinths.IData labyrinth)
        //{
        //    algorithmSteps = new List<Tile>();

        //    //lastVisitedIntersection the two first value are the map position and the third value is the step number to get to the intersection
        //    List<Vector3Int> lastVisitedIntersection = new List<Vector3Int>();

        //    bool[,] alreadyVisitedTile = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()];

        //    bool asReachedTheEnd = false;

        //    int direction = labyrinth.StartDirection;

        //    tileColor = TileColor.Yellow;
        //    position = labyrinth.StartPos;
        //    Vector2Int endPosition = labyrinth.EndPos;

        //    algorithmSteps.Add(new Tile(position.x, position.y, tileColor));

        //    while (!asReachedTheEnd)
        //    {
        //        bool[] isDirectionWalkableAndNotVisited = new bool[4];
        //        isDirectionWalkableAndNotVisited[0] = labyrinth.GetIsTileWalkable(position.x + xByDirection[0], position.y + yByDirection[0]) && !alreadyVisitedTile[position.x + xByDirection[0], position.y + yByDirection[0]];
        //        isDirectionWalkableAndNotVisited[1] = labyrinth.GetIsTileWalkable(position.x + xByDirection[1], position.y + yByDirection[1]) && !alreadyVisitedTile[position.x + xByDirection[1], position.y + yByDirection[1]];
        //        isDirectionWalkableAndNotVisited[2] = labyrinth.GetIsTileWalkable(position.x + xByDirection[2], position.y + yByDirection[2]) && !alreadyVisitedTile[position.x + xByDirection[2], position.y + yByDirection[2]];
        //        isDirectionWalkableAndNotVisited[3] = labyrinth.GetIsTileWalkable(position.x + xByDirection[3], position.y + yByDirection[3]) && !alreadyVisitedTile[position.x + xByDirection[3], position.y + yByDirection[3]];

        //        if (!alreadyVisitedTile[position.x, position.y])//Should be changed if we don't start in a deadend
        //        {
        //            if ((isDirectionWalkableAndNotVisited[direction] && isDirectionWalkableAndNotVisited[(direction + 1) % 4])
        //               || (isDirectionWalkableAndNotVisited[direction] && isDirectionWalkableAndNotVisited[(direction + 3) % 4])
        //               || (isDirectionWalkableAndNotVisited[(direction + 1) % 4] && isDirectionWalkableAndNotVisited[(direction + 3) % 4]))
        //            {
        //                lastVisitedIntersection.Add(new Vector3Int(position.x, position.y, algorithmSteps.Count - 1));
        //            }

        //            alreadyVisitedTile[position.x, position.y] = true;
        //        }

        //        if (isDirectionWalkableAndNotVisited[0])
        //        {
        //            direction = 0;
        //            MoveInDirection(direction);
        //        }
        //        else if (isDirectionWalkableAndNotVisited[1])
        //        {
        //            direction = 1;
        //            MoveInDirection(direction);
        //        }
        //        else if (isDirectionWalkableAndNotVisited[2])
        //        {
        //            direction = 2;
        //            MoveInDirection(direction);
        //        }
        //        else if (isDirectionWalkableAndNotVisited[3])
        //        {
        //            direction = 3;
        //            MoveInDirection(direction);
        //        }
        //        else
        //        {
        //            int i;
        //            if (position.x == lastVisitedIntersection[lastVisitedIntersection.Count - 1].x && position.y == lastVisitedIntersection[lastVisitedIntersection.Count - 1].y)
        //            {
        //                i = lastVisitedIntersection[lastVisitedIntersection.Count - 1].z - 1;
        //                lastVisitedIntersection.RemoveAt(lastVisitedIntersection.Count - 1);
        //            }
        //            else
        //            {
        //                i = algorithmSteps.Count - 2;
        //            }

        //            algorithmSteps[algorithmSteps.Count - 1] = new Tile(algorithmSteps[algorithmSteps.Count - 1].x, algorithmSteps[algorithmSteps.Count - 1].y, TileColor.Red);

        //            bool isReturnedToLastIntersection = false;

        //            while (!isReturnedToLastIntersection)
        //            {
        //                if (i < 0)
        //                {
        //                    //Labyrith is impossible!
        //                    return algorithmSteps;
        //                }

        //                if (algorithmSteps[i].x == lastVisitedIntersection[lastVisitedIntersection.Count - 1].x && algorithmSteps[i].y == lastVisitedIntersection[lastVisitedIntersection.Count - 1].y)
        //                {
        //                    isReturnedToLastIntersection = true;
        //                    position.x = algorithmSteps[i].x;
        //                    position.y = algorithmSteps[i].y;
        //                    algorithmSteps.Add(new Tile(position.x, position.y, tileColor));
        //                }
        //                else
        //                {
        //                    algorithmSteps.Add(new Tile(algorithmSteps[i].x, algorithmSteps[i].y, TileColor.Red));
        //                }

        //                i--;
        //            }
        //        }

        //        if (position.x == endPosition.x && position.y == endPosition.y)
        //        {
        //            asReachedTheEnd = true;
        //        }
        //    }

        //    return algorithmSteps;
        //}

        //void MoveInDirection(int direction)
        //{
        //    position.x += xByDirection[direction];
        //    position.y += yByDirection[direction];
        //    algorithmSteps.Add(new Tile(position.x, position.y, tileColor));
        //}
    }
}
