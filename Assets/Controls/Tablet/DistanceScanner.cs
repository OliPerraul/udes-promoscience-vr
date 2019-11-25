using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Controls;
//using UdeS.Promoscience.Utils;

using System.Linq;

namespace UdeS.Promoscience.Controls
{
    public class DistanceScanner : MonoBehaviour
    {
        [SerializeField]
        Characters.AvatarCharacter avatar;

        [SerializeField]
        Transform raycastStartPoint;

        const string TAG_FLOOR = "Floor";
        const string TAG_WALL = "Wall";

        readonly int[] xByDirection = { 0, 1, 0, -1 };
        readonly int[] yByDirection = { -1, 0, 1, 0 };

        float raycastRange = 100 * Labyrinths.Utils.TileSize;

        public float ExecuteDistanceScan(Direction direction)
        {
            if (Client.Instance.Labyrinth == null)
                return -1f;

            Ray ray = new Ray(raycastStartPoint.position, Utils.GetDirectionVector(direction));
            var res = Physics.RaycastAll(ray, raycastRange);
            var any = res.Where(x => x.collider.gameObject.GetComponentInChildren<Labyrinths.Piece>() != null);

            if (any.Count() != 0)
            {
                float dist = any.Min(x => x.distance);
                return ((int)dist) / Labyrinths.Utils.TileSize;
            }

            return -1f;
        }
    }
}
