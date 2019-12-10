using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;
using Cirrus.Extensions;
using UdeS.Promoscience.Algorithms;
using System;

namespace UdeS.Promoscience.Labyrinths
{ 
    [System.Serializable]
    public class Camera
    {
        // TODO remove this..
        public static Camera CurrentCamera;

        [SerializeField]
        private RenderTexture renderTexture;

        public RenderTexture RenderTexture => renderTexture;

        [SerializeField]
        public UnityEngine.Camera Source;

        [SerializeField]
        public float HeightOffset;

        [SerializeField]
        public GameObject Overlay;

        public void Init()
        {
            Source.gameObject.SetActive(true);
            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
            rt.name = Source.name;
            renderTexture = rt;
        }

        public bool Enabled
        {
            set
            {
                Source.gameObject.SetActive(value);
            }
        }

        [SerializeField]
        private bool outputToTexture = false;

        public bool OutputToTexture
        {
            set
            {
                outputToTexture = value;
                Source.targetTexture = value ? RenderTexture : null;
            }
        }
    }


    public class LabyrinthObject : MonoBehaviour, ISkin, ILabyrinth
    {
        [SerializeField]
        private Piece[] pieces;

        public IEnumerable<Piece> Pieces => data.Skin == null ? pieces : data.Skin.Pieces;

        public string Name => data.Skin == null ? name : data.Skin.Name;

        [SerializeField]
        public Camera Camera;

        [SerializeField]
        private Resource scriptableData;

        private ILabyrinth data = null;

        [SerializeField]
        private GameObject[,] labyrinthTiles;

        int[,] labyrinth;

        public ILabyrinth Data => data == null ? scriptableData : data;

        public int Id => data.Id;

        Vector2Int StartPosition => data.StartPos;

        Vector2Int EndPosition => data.EndPos;

        int ILabyrinth.Id { get => Data.Id; set => Data.Id = value; }
        public TileType[] Tiles2 { get => Data.Tiles2; set => Data.Tiles2 = value; }
        public int SizeX { get => Data.SizeX; set => Data.SizeX = value; }
        public int SizeY { get => Data.SizeY; set => Data.SizeY = value; }
        public Vector2Int StartPos { get => Data.StartPos; set => Data.StartPos = value; }
        public Vector2Int EndPos { get => Data.EndPos; set => Data.EndPos = value; }

        public int StartDirection => Data.StartDirection;

        public string Json => Data.Json;

        public SkinResource Skin => Data.Skin;

        string ILabyrinth.Name { get => Data.Name; set => Data.Name = value; }

        private Vector3 offset;

        public void Awake()
        {
            Camera.Init();
        }

        private void Start()
        {
            offset = transform.position;

            Debug.Log(offset);
            //Client.Instance.State.OnValueChangedHandler += OnGameStateChanged;
        }

        //public void SetCamera(
        //    int numLabyrinths,
        //    int maxHorizontal,
        //    int index)
        //{
        //    Camera.Source.gameObject.SetActive(true);

        //    //Camera.Split(
        //    //    maxHorizontal,
        //    //    numLabyrinths / maxHorizontal,
        //    //    index);
        //}

        public void OnGameStateChanged()
        {
            //if (Client.Instance.State.Value == ClientGameState.TutorialLabyrinthReady)
            //{
            //    GenerateLabyrinthVisual();
            //    Client.Instance.State.Value = ClientGameState.PlayingTutorial;
            //}
            //else if (Client.Instance.State.Value == ClientGameState.LabyrinthReady)
            //{
            //    GenerateLabyrinthVisual();
            //    Client.Instance.State.Value = ClientGameState.Playing;
            //}
            //else if (Client.Instance.State.Value == ClientGameState.ViewingLocalReplay)
            //{
            //    GenerateLabyrinthVisual();

            //}
            //else if (Client.Instance.State.Value == ClientGameState.WaitingForNextRound)
            //{
            //    DestroyLabyrinth();
            //}
        }

        public void GenerateLabyrinthVisual(SkinResource skin=null)
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
                    labyrinthTiles[x, y] = InstantiateLabyrithTile(x, y, labyrinth[x, y], skin);
                }
            }
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

        GameObject InstantiateLabyrithTile(int x, int y, int tileId, SkinResource skin=null)
        {
            if (tileId == 0)
                return null;

            GameObject tile = null;

            Vector3 tilePosition = GetLabyrinthPositionInWorldPosition(x, y);

            Piece piece = this.GetPiece((TileType)tileId);

            if (piece == null)
                return null;

            tile = Instantiate(
                piece.gameObject, 
                tilePosition,
                Quaternion.identity, 
                gameObject.transform); 

            return tile;
        }

        private Vector3 DoGetLabyrinthPositionInWorldPosition(int x, int y)
        {
            return new Vector3((x - StartPosition.x) * Utils.TileSize, 0, (-y + StartPosition.y) * Utils.TileSize) + offset;
        }

        public Vector3 GetLabyrinthPositionInWorldPosition(int x, int y)
        {
            return DoGetLabyrinthPositionInWorldPosition(x, y);
        }

        public Vector3 GetLabyrinthPositionInWorldPosition(Vector2Int position)
        {
            return DoGetLabyrinthPositionInWorldPosition(position.x, position.y);
        }

        private Vector2Int DoGetWorldPositionInLabyrinthPosition(float x, float y)
        {
            float xx = x - offset.x;
            float yy = y - offset.y;
            return new Vector2Int(Mathf.RoundToInt((xx / Utils.TileSize)) + StartPosition.x, Mathf.RoundToInt((-yy / Utils.TileSize)) + StartPosition.y);
        }


        public Vector2Int GetWorldPositionInLabyrinthPosition(float x, float y)
        {
            return DoGetWorldPositionInLabyrinthPosition(x, y);
        }

        public Vector2Int GetWorldPositionInLabyrinthPosition(Vector2 position)
        {
            return DoGetWorldPositionInLabyrinthPosition(position.x, position.y);
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

        public Piece GetTilePiece(Vector2Int position)
        {
            return labyrinthTiles[position.x, position.y].GetComponentInChildren<Piece>();
        }

        public Piece GetTilePiece(int x, int y)
        {
            return labyrinthTiles[x, y].GetComponentInChildren<Piece>();
        }


        public void Init(bool enableCamera)
        {
            Camera.Source.gameObject.SetActive(true);
            Camera.Source.transform.position += GetLabyrinthPositionInWorldPosition(0, 0);
            Camera.Source.transform.position += new Vector3(
                labyrinthTiles.GetLength(0) * Utils.TileSize,
                0,
                -labyrinthTiles.GetLength(1) * Utils.TileSize) / 2;

            Camera.Source.transform.position += Vector3.up * Camera.HeightOffset;
            Camera.Source.enabled = enableCamera;

            //TODO remove
            // Used for editor
            if (enableCamera)
            { 
                Camera.CurrentCamera = Camera;
            }
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

            if (labyrinthTiles[tile.x, tile.y] != null)
            {
                FloorPainter floorPainter = labyrinthTiles[tile.x, tile.y].GetComponentInChildren<FloorPainter>();
                if (floorPainter != null)
                {
                    color = floorPainter.GetFloorColor();
                }

            }
            return color;
        }

        public void SetTileColor(Vector2Int tile, TileColor color)
        {
            if (labyrinthTiles[tile.x, tile.y] != null)
            {
                GameObject gobj = labyrinthTiles[tile.x, tile.y];

                var floorPainter = gobj.GetComponentInChildren<FloorPainter>();
                if (floorPainter != null)
                {
                    floorPainter.PaintFloorWithColor(color, paintfloor:true);
                }
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
            foreach (GameObject tile in labyrinthTiles)
            {
                if (tile == null)
                    continue;

                Destroy(tile.gameObject);
            }

            labyrinthTiles = null;
        }

        public LabyrinthObject Create(ILabyrinth data)
        {
            var labyrinth = this.Create();
            labyrinth.data = data;

            return labyrinth;
        }

        public int GetLabyrithValueAt(int x, int y)
        {
            return Data.GetLabyrithValueAt(x, y);
        }

        public void SetLabyrithValueAt(int x, int y, TileType tile)
        {
            if (x < 0)
                return;
            if (x >= SizeX)
                return;
            if (y < 0)
                return;
            if (y >= SizeY)
                return;


            labyrinthTiles[x, y].Destroy();
            labyrinthTiles[x, y] = InstantiateLabyrithTile(x, y, (int)tile, Data.Skin);
            Data.SetLabyrithValueAt(x, y, tile);
        }

        public void SetLabyrithValueAt(Vector2Int pos, TileType tile)
        {
            SetLabyrithValueAt(pos.x, pos.y, tile);
        }

        public void PopulateStartAndEndPositions()
        {
            Data.PopulateStartAndEndPositions();
        }
    }

}