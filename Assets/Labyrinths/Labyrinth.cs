using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;
using Cirrus.Extensions;
using UdeS.Promoscience.Algorithms;

namespace UdeS.Promoscience.Labyrinths
{
    [System.Serializable]
    public enum TileType
    {
        Start,
        End,
        Empty,

        Floor1,
        Floor2,
        Floor3,
        Horizontal1,
        Horizontal2,
        Horizontal3,
        Horizontal4,
        Vertical1,
        Vertical2,
        Vertical3,
        Vertical4,
        Corner1,
        Corner2,
        Corner3,


    }

    public enum Type
    {
        Unknown,
        Small,
        Medium,
        Large
    }

    public static class Utils
    {
        public const int NumLabyrinth = 4;

        public const int SizeSmall = 9;

        public const int SizeMedium = 11;

        public const int SizeLarge = 13;


        public static Type GetType(IData data)
        {
            if (data.SizeX <= SizeSmall && data.SizeY <= SizeSmall)
            {
                return Type.Small;
            }
            else if (data.SizeX <= SizeMedium && data.SizeY <= SizeMedium)
            {
                return Type.Medium;
            }
            else
            {
                return Type.Large;
            }
        }

        public static TileType[] ConvertToTiles(int[] tiles)
        {
            TileType[] res = new TileType[tiles.Length];

            for (int i = 0; i < tiles.Length; i++)
            {
                res[i] = TileType.Empty;

                if (res[i] == 0)
                    continue;

                if (tiles[i] >= Promoscience.Utils.TILE_START_START_ID && tiles[i] <= Promoscience.Utils.TILE_START_END_ID)
                {
                    res[i] = TileType.Start;
                }

                if (tiles[i] >= Promoscience.Utils.TILE_FLOOR_START_ID && tiles[i] <= Promoscience.Utils.TILE_FLOOR_END_ID)
                {
                    res[i] = TileType.Floor1;
                }

                if (tiles[i] >= Promoscience.Utils.TILE_END_START_ID && tiles[i] <= Promoscience.Utils.TILE_END_END_ID)
                {
                    res[i] = TileType.End;
                }

                if (tiles[i] >= Promoscience.Utils.TILE_WALL_START_ID && tiles[i] <= Promoscience.Utils.TILE_WALL_END_ID)
                {
                    switch (tiles[i])
                    {
                        //public const int TILE_ROME_HORIZONTAL_WALL_ID = 151;
                        //public const int TILE_ROME_HORIZONTAL_WALL_B_ID = 152;
                        //public const int TILE_ROME_VERTICAL_WALL_ID = 153;
                        //public const int TILE_ROME_VERTICAL_WALL_B_ID = 154;
                        //public const int TILE_ROME_TOWER_WALL_ID = 155;

                        //public const int TILE_PTOL_HORIZONTAL_WALL_ID = 156;
                        //public const int TILE_PTOL_HORIZONTAL_WALL_B_ID = 157;
                        //public const int TILE_PTOL_VERTICAL_WALL_ID = 158;
                        //public const int TILE_PTOL_VERTICAL_WALL_B_ID = 159;
                        //public const int TILE_PTOL_TOWER_WALL_ID = 160;

                        //public const int TILE_BRIT_HORIZONTAL_WALL_ID = 161;
                        //public const int TILE_BRIT_VERTICAL_WALL_ID = 162;
                        //public const int TILE_BRIT_TOWER_WALL_ID = 163;
                        //public const int TILE_BRIT_TOWER_WALL_2_ID = 164;

                        //public const int TILE_KART_HORIZONTAL_WALL_ID = 165;
                        //public const int TILE_KART_HORIZONTAL_WALL_B_ID = 166;
                        //public const int TILE_KART_VERTICAL_WALL_ID = 167;
                        //public const int TILE_KART_VERTICAL_WALL_B_ID = 168;
                        //public const int TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_ID = 169;
                        //public const int TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_B_ID = 170;
                        //public const int TILE_KART_VERTICAL_WALL_SCAFFOLDING_ID = 171;
                        //public const int TILE_KART_VERTICAL_WALL_SCAFFOLDING_B_ID = 172;
                        //public const int TILE_KART_TOWER_WALL_ID = 173;
                        //public const int TILE_KART_TOWER_WALL_2_ID = 174;
                        //public const int TILE_KART_TOWER_WALL_SCAFFOLDING_ID = 175;

                        // Horizontal

                        case Promoscience.Utils.TILE_ROME_HORIZONTAL_WALL_ID:
                        case Promoscience.Utils.TILE_PTOL_HORIZONTAL_WALL_ID:
                        case Promoscience.Utils.TILE_BRIT_HORIZONTAL_WALL_ID:
                        case Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID:
                            res[i] = TileType.Horizontal1;
                            break;

                        case Promoscience.Utils.TILE_ROME_HORIZONTAL_WALL_B_ID:
                        case Promoscience.Utils.TILE_PTOL_HORIZONTAL_WALL_B_ID:
                        case Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_B_ID:
                            res[i] = TileType.Horizontal2;
                            break;

                        case Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_ID:
                            res[i] = TileType.Horizontal3;
                            break;

                        case Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_B_ID:
                            res[i] = TileType.Horizontal4;
                            break;

                        // Vertical
                        
                        case Promoscience.Utils.TILE_ROME_VERTICAL_WALL_ID:
                        case Promoscience.Utils.TILE_PTOL_VERTICAL_WALL_ID:
                        case Promoscience.Utils.TILE_BRIT_VERTICAL_WALL_ID:
                        case Promoscience.Utils.TILE_KART_VERTICAL_WALL_ID:
                            res[i] = TileType.Vertical1;
                            break;

                        case Promoscience.Utils.TILE_ROME_VERTICAL_WALL_B_ID:
                        case Promoscience.Utils.TILE_PTOL_VERTICAL_WALL_B_ID:
                        case Promoscience.Utils.TILE_KART_VERTICAL_WALL_B_ID:
                            res[i] = TileType.Vertical2;
                            break;

                        case Promoscience.Utils.TILE_KART_VERTICAL_WALL_SCAFFOLDING_ID:
                            res[i] = TileType.Vertical3;
                            break;

                        case Promoscience.Utils.TILE_KART_VERTICAL_WALL_SCAFFOLDING_B_ID:
                            res[i] = TileType.Vertical4;
                            break;

                        // Corners

                        case Promoscience.Utils.TILE_ROME_TOWER_WALL_ID:
                        case Promoscience.Utils.TILE_PTOL_TOWER_WALL_ID:
                        case Promoscience.Utils.TILE_BRIT_TOWER_WALL_ID:
                        case Promoscience.Utils.TILE_KART_TOWER_WALL_ID:
                            res[i] = TileType.Corner1;
                            break;

                        case Promoscience.Utils.TILE_BRIT_TOWER_WALL_2_ID:
                        case Promoscience.Utils.TILE_KART_TOWER_WALL_2_ID:
                            res[i] = TileType.Corner2;
                            break;

                        case Promoscience.Utils.TILE_KART_TOWER_WALL_SCAFFOLDING_ID:                        
                            res[i] = TileType.Corner3;
                            break;

                        default:
                            res[i] = TileType.Horizontal1;
                            break;                       

                    }

                }

            }

            return res;
        }
    }

    [System.Serializable]
    public class Camera
    {
        [SerializeField]
        public UnityEngine.Camera Source;

        [SerializeField]
        public float HeightOffset;

        [SerializeField]
        public GameObject Overlay;

        public void Maximize()
        {
            Source.gameObject.SetActive(true);
            Source.rect = new Rect(0, 0, 1, 1);
        }

        public void Split(
            int horizontal,
            int vertical,
            int index)
        {
            var x = ((float)index.Mod(horizontal));

            var y = ((float)(index / horizontal));
            y = (vertical - 1) - y;

            Source.rect = new Rect(
                (x / horizontal),
                y / vertical,
                1f / horizontal,
                1f / vertical);
        }
    }

    public delegate void OnLabyrinthEvent(Labyrinth labyrinth);

    public delegate void OnDataEvent(IData labyrinth);

    public class Labyrinth : MonoBehaviour
    {
        [SerializeField]
        public Camera Camera;

        [SerializeField]
        private Resource scriptableData;

        private IData data = null;

        public IData Data
        {
            get
            {
                return data == null ? scriptableData : data;
            }
        }

        public int Id
        {
            get
            {
                return data.Id;
            }
        }

        GameObject[,] labyrinthTiles;

        int[,] labyrinth;

        Vector2Int StartPosition
        {
            get
            {
                return data.StartPos;
            }
        }

        Vector2Int EndPosition
        {
            get
            {
                return data.EndPos;
            }
        }

        private void Start()
        {
            //Client.Instance.clientStateChangedEvent += OnGameStateChanged;
        }

        public void SetCamera(
            int numLabyrinths,
            int maxHorizontal,
            int index)
        {
            Camera.Source.gameObject.SetActive(true);

            Camera.Split(
                maxHorizontal,
                numLabyrinths / maxHorizontal,
                index);
        }

        public void OnGameStateChanged()
        {
            //if (Client.Instance.State == ClientGameState.TutorialLabyrinthReady)
            //{
            //    GenerateLabyrinthVisual();
            //    Client.Instance.State = ClientGameState.PlayingTutorial;
            //}
            //else if (Client.Instance.State == ClientGameState.LabyrinthReady)
            //{
            //    GenerateLabyrinthVisual();
            //    Client.Instance.State = ClientGameState.Playing;
            //}
            //else if (Client.Instance.State == ClientGameState.ViewingLocalReplay)
            //{
            //    GenerateLabyrinthVisual();

            //}
            //else if (Client.Instance.State == ClientGameState.WaitingForNextRound)
            //{
            //    DestroyLabyrinth();
            //}
        }

        public void GenerateLabyrinthVisual()
        {
            if (labyrinthTiles != null)
            {
                DestroyLabyrinth();
            }

            PopulateLabyrinth();

            labyrinthTiles = new GameObject[labyrinth.GetLength(0), labyrinth.GetLength(1)];

            for (int x = 0; x < labyrinthTiles.GetLength(0); x++)
            {
                for (int y = 0; y < labyrinthTiles.GetLength(1); y++)
                {
                    labyrinthTiles[x, y] = InstantiateLabyrithTile(x, y, labyrinth[x, y]);
                }
            }

            Camera.Source.transform.position += GetLabyrinthPositionInWorldPosition(0, 0);
            Camera.Source.transform.position += new Vector3(
                labyrinthTiles.GetLength(0) * Promoscience.Utils.TILE_SIZE,
                0,
                -labyrinthTiles.GetLength(1) * Promoscience.Utils.TILE_SIZE) / 2;

            Camera.Source.transform.position += Vector3.up * Camera.HeightOffset;
        }

        void PopulateLabyrinth()
        {
            labyrinth = new int[Data.SizeX, Data.SizeY];

            for (int x = 0; x < labyrinth.GetLength(0); x++)
            {
                for (int y = 0; y < labyrinth.GetLength(1); y++)
                {
                    labyrinth[x, y] = Data.GetLabyrithValueAt(x, y);
                }
            }
        }

        GameObject InstantiateLabyrithTile(int x, int y, int tileId)
        {
            GameObject tile = null;

            Vector3 tilePosition = GetLabyrinthPositionInWorldPosition(x, y);

            tile = Instantiate(Resources.Instance.GetTilePrefabWithId(tileId), tilePosition, Quaternion.identity, gameObject.transform);

            return tile;
        }

        public Vector3 GetLabyrinthPositionInWorldPosition(int x, int y)
        {
            return new Vector3((x - StartPosition.x) * Promoscience.Utils.TILE_SIZE, 0, (-y + StartPosition.y) * Promoscience.Utils.TILE_SIZE);
        }

        public Vector3 GetLabyrinthPositionInWorldPosition(Vector2Int position)
        {
            return GetLabyrinthPositionInWorldPosition(position.x, position.y);
        }

        public Vector2Int GetWorldPositionInLabyrinthPosition(float x, float y)
        {
            return new Vector2Int(Mathf.RoundToInt((x / Promoscience.Utils.TILE_SIZE)) + StartPosition.x, Mathf.RoundToInt((-y / Promoscience.Utils.TILE_SIZE)) + StartPosition.y);
        }

        public Vector2Int GetLabyrithStartPosition()
        {
            return StartPosition;
        }

        public Vector2Int GetLabyrithEndPosition()
        {
            return EndPosition;
        }
        public Vector3 GetLabyrithEndPositionInWorldPosition()
        {
            return GetLabyrinthPositionInWorldPosition(EndPosition.x, EndPosition.y);
        }

        public int GetLabyrithXLenght()
        {
            return labyrinth.GetLength(0);
        }

        public int GetLabyrithYLenght()
        {
            return labyrinth.GetLength(1);
        }

        public GameObject GetTile(Vector2Int position)
        {
            return labyrinthTiles[position.x, position.y];
        }

        public GameObject GetTile(int x, int y)
        {
            return labyrinthTiles[x, y];
        }

        //Labyrith start should always be in a dead end
        public int GetStartDirection()
        {
            // up
            int direction = 0;

            // right
            if (GetIsTileWalkable(StartPosition.x + 1, StartPosition.y))
            {
                direction = 1;
            }
            // down
            else if (GetIsTileWalkable(StartPosition.x, StartPosition.y + 1))
            {
                direction = 2;
            }
            // Left
            else if (GetIsTileWalkable(StartPosition.x - 1, StartPosition.y))
            {
                direction = 3;
            }

            return direction;
        }

        public bool GetIsTileWalkable(int x, int y)
        {
            return data.GetIsTileWalkable(x, y);
        }

        public bool GetIsTileWalkable(Vector2Int tile)
        {
            return data.GetIsTileWalkable(tile);
        }

        public TileColor GetTileColor(Vector2Int tile)
        {
            TileColor color = TileColor.NoColor;
            FloorPainter floorPainter = labyrinthTiles[tile.x, tile.y].GetComponentInChildren<FloorPainter>();
            if (floorPainter != null)
            {
                color = floorPainter.GetFloorColor();
            }
            return color;
        }

        public void SetTileColor(Vector2Int tile, TileColor color)
        {
            GameObject gobj = labyrinthTiles[tile.x, tile.y];

            var floorPainter = gobj.GetComponentInChildren<FloorPainter>();
            if (floorPainter != null)
            {
                floorPainter.PaintFloorWithColor(color);
            }
        }

        public Tile[] GetTilesToPaint()
        {
            Queue<Tile> paintedTile = new Queue<Tile>();

            for (int x = 0; x < labyrinthTiles.GetLength(0); x++)
            {
                for (int y = 0; y < labyrinthTiles.GetLength(1); y++)
                {
                    TileColor color = GetTileColor(new Vector2Int(x, y));
                    if (color != TileColor.Grey)
                    {
                        paintedTile.Enqueue(new Tile(x, y, color));
                    }
                }
            }

            return paintedTile.ToArray();
        }

        public void DestroyLabyrinth()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            labyrinthTiles = null;
        }

        public Labyrinth Create(IData data)
        {
            var labyrinth = this.Create();
            labyrinth.data = data;
            return labyrinth;
        }

    }

}