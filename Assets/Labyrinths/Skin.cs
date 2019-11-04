using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Labyrinths
{
    public class Skin : ScriptableObject
    {
        [SerializeField]
        public GameObject FloorStart;

        [SerializeField]
        public GameObject FloorEnd;

        [SerializeField]
        public GameObject FloorTile1;

        [SerializeField]
        public GameObject FloorTile2;

        [SerializeField]
        public GameObject FloorTile3;

        [SerializeField]
        public GameObject Corner1;

        [SerializeField]
        public GameObject Corner2;

        [SerializeField]
        public GameObject Corner3;
        
        [SerializeField]
        public GameObject Horizontal1;

        [SerializeField]
        public GameObject Horizontal2;

        [SerializeField]
        public GameObject Horizontal3;

        [SerializeField]
        public GameObject Horizontal4;

        [SerializeField]
        public GameObject Vertical1;

        [SerializeField]
        public GameObject Vertical2;

        [SerializeField]
        public GameObject Vertical3;

        [SerializeField]
        public GameObject Vertical4;

        public GameObject GetGameObject(TileType type)
        {
            switch (type)
            {
                case TileType.End:
                    return FloorEnd;

                case TileType.Start:
                    return FloorStart;

                case TileType.Corner1:
                    return Corner1;

                case TileType.Corner2:
                    return Corner2;

                case TileType.Corner3:
                    return Corner3;

                case TileType.Horizontal1:
                    return Horizontal1;

                case TileType.Horizontal2:
                    return Horizontal2;

                case TileType.Horizontal3:
                    return Horizontal3;

                case TileType.Horizontal4:
                    return Horizontal4;

                case TileType.Vertical1:
                    return Vertical1;

                case TileType.Vertical2:
                    return Vertical2;

                case TileType.Vertical3:
                    return Vertical3;

                case TileType.Vertical4:
                    return Vertical4;

                default:
                    return null;
            }
        }
    }
}