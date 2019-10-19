using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.Replays
{
    public static class Utils
    {
        public static Vector2Int GetMoveDestination(Vector2Int lpos, GameAction action)
        {
            // From ReturnToDivergent, EndAction, CompleteRound
            // Simply walk to the 'nextlpos', we do not increment nextlpos

            lpos.y += action == GameAction.MoveUp ? -1 : 0;
            lpos.y += action == GameAction.MoveDown ? 1 : 0;
            lpos.x += action == GameAction.MoveLeft ? -1 : 0;
            lpos.x += action == GameAction.MoveRight ? 1 : 0;
            return lpos;
        }

        public static Vector3 GetTileEdgePositionFromDirection(Direction action)
        {
            switch (action)
            {
                case Direction.Down:
                    return (Vector3.forward * -Constants.TILE_SIZE / 2);

                case Direction.Up:
                    return (Vector3.forward * Constants.TILE_SIZE / 2);

                case Direction.Left:
                    return (Vector3.right * -Constants.TILE_SIZE / 2);

                case Direction.Right:
                    return (Vector3.right * Constants.TILE_SIZE / 2);
            }

            return Vector3.zero;
        }

        public static float GetDirectionSideOffset(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    return 1f;
                case Direction.Up:
                    return -1f;

                case Direction.Right:
                case Direction.Left:
                    return 0;
            }

            return 0;
        }

        public static float GetDirectionFrontOffset(Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return -1f;
                case Direction.Left:
                    return 1f;

                case Direction.Down:
                case Direction.Up:
                    return 0;
            }

            return 0;
        }

        public static Direction GetDirection(Vector2Int origin, Vector2Int destination)
        {
            if (origin.y < destination.y)
            {
                return Direction.Down;
            }
            else if (origin.y > destination.y)
            {
                return Direction.Up;
            }
            else if (origin.x > destination.x)
            {
                return Direction.Left;
            }
            else
            {
                return Direction.Right;
            }
        }

        public static bool IsOppositeDirection(Direction direction, Direction other)
        {
            switch (direction)
            {
                case Direction.Up:
                    return
                        other == Direction.Down;// ||
                                                //other == Direction.Right;

                case Direction.Down:
                    return
                        other == Direction.Up;// ||
                                              //other == Direction.Left;

                case Direction.Left:
                    return
                        //other == Direction.Down ||
                        other == Direction.Right;

                case Direction.Right:
                    return
                        //other == Direction.Up ||
                        other == Direction.Left;

                default:
                    return false;
            }
        }

    }
}