using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UdeS.Promoscience.Labyrinths.Editor
{
    public class Resources : BaseResources<Resources>
    {
        [SerializeField]
        private Tile[] tiles;

        public Tile[] Tiles
        {
            get
            {
                if (tiles == null || 
                    tiles.Length == 0)
                {
                    tiles = Cirrus.AssetDatabase.FindObjectsOfType<Tile>();
                }

                return tiles;
            }
        }

        public Tile GetTile(TileType type)
        {
            return Tiles.Where(x => x.Type == type).FirstOrDefault();
        }

        public void OnValidate()
        {
            if (tiles == null || tiles.Length == 0)
            {
                tiles = Cirrus.AssetDatabase.FindObjectsOfType<Tile>();
            }
        }
    }
}