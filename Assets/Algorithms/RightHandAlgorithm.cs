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

        // Up, right, down left


        public override List<Tile> GetAlgorithmSteps(Labyrinths.IData labyrinth)
        {
            List<Tile> algorithmSteps = new List<Tile>();

            bool[,] alreadyVisitedTile = new bool[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()];

            bool hasReachedTheEnd = false;

            int direction = labyrinth.StartDirection;
            Vector2Int position = labyrinth.StartPos;
            Vector2Int endPosition = labyrinth.EndPos;
            algorithmSteps.Add(new Tile(position.x, position.y, TileColor.Yellow));
            alreadyVisitedTile[position.x, position.y] = true;

            while (!hasReachedTheEnd)
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
                    hasReachedTheEnd = true;
                }

                TileColor tileColor = hasReachedTheEnd || isDirectionWalkableAndNotVisited[0] || isDirectionWalkableAndNotVisited[1] || isDirectionWalkableAndNotVisited[2] || isDirectionWalkableAndNotVisited[3] ? TileColor.Yellow : TileColor.Red;

                algorithmSteps.Add(new Tile(position.x, position.y, tileColor));

                alreadyVisitedTile[position.x, position.y] = true;
            }

            return algorithmSteps;
        }

        // Up, right down, left



        public override bool GetNextStep(
            AlgorithmProgressState state, 
            Labyrinths.IData labyrinth, 
            out Tile tile)
        {
            tile = new Tile();

            Direction[] prioritizedDirections = new Direction[4];

            if (state.direction == (int)Direction.Up)
            {
                prioritizedDirections = 
                    new Direction []{
                        Direction.Right,
                        Direction.Up,
                        Direction.Left,
                        Direction.Down};
            }
            else if (state.direction == (int)Direction.Down)
            {
                prioritizedDirections =
                    new Direction[]{
                        Direction.Left,
                        Direction.Down,
                        Direction.Right,
                        Direction.Up};
            }
            else if (state.direction == (int)Direction.Left)
            {
                prioritizedDirections =
                    new Direction[]{
                        Direction.Up,
                        Direction.Left,
                        Direction.Down,
                        Direction.Right};
            }
            else if (state.direction == (int)Direction.Right)
            {
                prioritizedDirections =
                    new Direction[]{
                        Direction.Down,
                        Direction.Right,
                        Direction.Up,
                        Direction.Left};
            }

            Vector2Int dest = state.position;
            bool found = false;

            for (int i = 0; i < 4; i++)
            {
                if (prioritizedDirections[i] == Direction.Up)
                {
                    if (labyrinth.GetIsTileWalkable(
                        dest = Promoscience.Utils.GetMoveDestination(state.position, Direction.Up)) &&
                        !state.IsAlreadyVisisted(dest))
                    {
                        state.direction = (int)Direction.Up;
                        found = true;
                        break;
                    }
                }
                else if (prioritizedDirections[i] == Direction.Down)
                {
                    if (labyrinth.GetIsTileWalkable(
                        dest = Promoscience.Utils.GetMoveDestination(state.position, Direction.Down)) &&
                        !state.IsAlreadyVisisted(dest))
                    {
                        state.direction = (int)Direction.Down;
                        found = true;
                        break;
                    }
                }
                else if (prioritizedDirections[i] == Direction.Left)
                {
                    if (labyrinth.GetIsTileWalkable(
                        dest = Promoscience.Utils.GetMoveDestination(state.position, Direction.Left)) &&
                        !state.IsAlreadyVisisted(dest))
                    {
                        state.direction = (int)Direction.Left;
                        found = true;
                        break;
                    }
                }
                else if (prioritizedDirections[i] == Direction.Right)
                {
                    if (labyrinth.GetIsTileWalkable(
                        dest = Promoscience.Utils.GetMoveDestination(state.position, Direction.Right)) &&
                        !state.IsAlreadyVisisted(dest))
                    {
                        state.direction = (int)Direction.Right;           
                        found = true;
                        break;
                    }
                }
            }

            if (found)
            {
                if (state.lastRemoved != null)
                {
                    state.stack.Add(state.lastRemoved);
                    state.lastRemoved = null;
                }

                state.stack.Add(new Action { pos = dest, dir = (Direction)state.direction });
                state.SetVisited(dest);
                tile = new Tile
                {
                    Position = dest,
                    Color = TileColor.Yellow
                };
                state.position = tile.Position;

                state.hasReachedTheEnd = dest == labyrinth.EndPos;
            }
            else if (state.stack.Count != 0)
            {
                int last = state.stack.Count - 1;
                tile = new Tile
                {
                    Position = state.stack[last].pos,
                    Color = TileColor.Red
                };

                state.position = tile.Position;
                state.direction = (int)Promoscience.Utils.GetOppositeDirection(state.stack[last].dir);
                state.lastRemoved = state.stack[last];
                state.stack.RemoveAt(last);
                
            }
            else return true;


            return !state.hasReachedTheEnd;

        }
    }
}