using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
////using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Algorithms
{
    public class RightHandAlgorithm : Algorithm
    {
        readonly int[] xByDirection = { 0, 1, 0, -1 };
        readonly int[] yByDirection = { -1, 0, 1, 0 };

        public override Promoscience.Algorithms.Id Id
        {
            get
            {
                return Promoscience.Algorithms.Id.RightHand;
            }
        }

        public override List<Tile> GetAlgorithmSteps(Labyrinths.IData labyrinth)
        {
            List<Tile> algorithmSteps = new List<Tile>();

            bool[,] alreadyVisitedTile = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()];

            bool asReachedTheEnd = false;

            int direction = labyrinth.StartDirection;
            Vector2Int position = labyrinth.StartPos;
            Vector2Int endPosition = labyrinth.EndPos;
            algorithmSteps.Add(new Tile(position.x, position.y, TileColor.Yellow));
            alreadyVisitedTile[position.x, position.y] = true;

            while (!asReachedTheEnd)
            {
                bool[] isDirectionWalkable = new bool[4];
                isDirectionWalkable[0] = labyrinth.GetIsTileWalkable(position.x + xByDirection[0], position.y + yByDirection[0]);
                isDirectionWalkable[1] = labyrinth.GetIsTileWalkable(position.x + xByDirection[1], position.y + yByDirection[1]);
                isDirectionWalkable[2] = labyrinth.GetIsTileWalkable(position.x + xByDirection[2], position.y + yByDirection[2]);
                isDirectionWalkable[3] = labyrinth.GetIsTileWalkable(position.x + xByDirection[3], position.y + yByDirection[3]);

                if (isDirectionWalkable[direction % 4] && !isDirectionWalkable[(direction + 1) % 4])
                {
                    direction = direction % 4;
                }
                else if (isDirectionWalkable[(direction + 1) % 4])
                {
                    direction = (direction + 1) % 4;
                }
                else if (isDirectionWalkable[(direction + 3) % 4])
                {
                    direction = (direction + 3) % 4;
                }
                else if (isDirectionWalkable[(direction + 2) % 4])
                {
                    direction = (direction + 2) % 4;
                }
                else//case when direction isn't determined at start, or isn't a good one
                {
                    Debug.Log("No possible solution!");//Temp
                }

                position.x += xByDirection[direction];
                position.y += yByDirection[direction];

                bool[] isDirectionWalkableAndNotVisited = new bool[4];
                isDirectionWalkableAndNotVisited[0] = labyrinth.GetIsTileWalkable(position.x + xByDirection[0], position.y + yByDirection[0]) && !alreadyVisitedTile[position.x + xByDirection[0], position.y + yByDirection[0]];
                isDirectionWalkableAndNotVisited[1] = labyrinth.GetIsTileWalkable(position.x + xByDirection[1], position.y + yByDirection[1]) && !alreadyVisitedTile[position.x + xByDirection[1], position.y + yByDirection[1]];
                isDirectionWalkableAndNotVisited[2] = labyrinth.GetIsTileWalkable(position.x + xByDirection[2], position.y + yByDirection[2]) && !alreadyVisitedTile[position.x + xByDirection[2], position.y + yByDirection[2]];
                isDirectionWalkableAndNotVisited[3] = labyrinth.GetIsTileWalkable(position.x + xByDirection[3], position.y + yByDirection[3]) && !alreadyVisitedTile[position.x + xByDirection[3], position.y + yByDirection[3]];

                if (position.x == endPosition.x && position.y == endPosition.y)
                {
                    asReachedTheEnd = true;
                }

                TileColor tileColor = asReachedTheEnd || isDirectionWalkableAndNotVisited[0] || isDirectionWalkableAndNotVisited[1] || isDirectionWalkableAndNotVisited[2] || isDirectionWalkableAndNotVisited[3] ? TileColor.Yellow : TileColor.Red;

                algorithmSteps.Add(new Tile(position.x, position.y, tileColor));

                alreadyVisitedTile[position.x, position.y] = true;


            }

            return algorithmSteps;
        }
    }
}