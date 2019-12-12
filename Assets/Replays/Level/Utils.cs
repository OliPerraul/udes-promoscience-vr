using UnityEngine;
using System.Collections;
//using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.Replays
{
    public static class Utils
    {
        public const float MinPlaybackSpeed = 0.5f;

        public const float MaxPlaybackSpeed = 2.5f;

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
                    return (Vector3.forward * -Labyrinths.Utils.TileSize / 2);

                case Direction.Up:
                    return (Vector3.forward * Labyrinths.Utils.TileSize / 2);

                case Direction.Left:
                    return (Vector3.right * -Labyrinths.Utils.TileSize / 2);

                case Direction.Right:
                    return (Vector3.right * Labyrinths.Utils.TileSize / 2);
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

    }
}