using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Labyrinths
{
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
            Source.rect = new Rect(0, 0, 1, 1);
        }

        public void Split(
            int horizontal,
            int vertical,
            int index)
        {
            var x =  ((float)index.Mod(horizontal));
            
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

    public class Labyrinth : MonoBehaviour
    {
        [SerializeField]
        private ScriptableClientGameState gameState;

        [SerializeField]
        public Camera Camera;

        [SerializeField]
        private ScriptableLabyrinth Resource;

        private IData data = null;

        IData Data
        {
            get
            {
                return data == null ? Resource : data;
            }
        }

        public int Id
        {
            get
            {
                return data.currentId;
            }
        }

        [SerializeField]
        ScriptableResources ressources;

        GameObject[,] labyrinthTiles;

        int[,] labyrinth;

        Vector2Int startPosition;
        Vector2Int endPosition;

        private void Start()
        {
            gameState.clientStateChangedEvent += OnGameStateChanged;
        }

        public void SetCamera(
            int numLabyrinths, 
            int maxHorizontal,
            float offset,
            int index)
        {
            Camera.Split(
                maxHorizontal,
                numLabyrinths / maxHorizontal,
                index);

            Camera.Source.transform.position += Vector3.up * offset / 2;
            Camera.Source.gameObject.SetActive(true);
        }

        public void OnGameStateChanged()
        {
            if (gameState.Value == ClientGameState.TutorialLabyrinthReady)
            {
                GenerateLabyrinthVisual();
                gameState.Value = ClientGameState.PlayingTutorial;
            }
            else if (gameState.Value == ClientGameState.LabyrithReady)
            {
                GenerateLabyrinthVisual();
                gameState.Value = ClientGameState.Playing;
            }
            else if (gameState.Value == ClientGameState.ViewingLocalReplay)
            {
                GenerateLabyrinthVisual();
                
            }
            else if (gameState.Value == ClientGameState.WaitingForNextRound)
            {
                DestroyLabyrinth();
            }
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
                labyrinthTiles.GetLength(0) *Constants.TILE_SIZE,
                0,
                -labyrinthTiles.GetLength(1) * Constants.TILE_SIZE)/ 2;
        }

        void PopulateLabyrinth()
        {
            labyrinth = new int[Data.sizeX, Data.sizeY];

            for (int x = 0; x < labyrinth.GetLength(0); x++)
            {
                for (int y = 0; y < labyrinth.GetLength(1); y++)
                {
                    labyrinth[x, y] = Data.GetLabyrithValueAt(x, y);

                    if (
                        labyrinth[x, y] >= Constants.TILE_START_START_ID && 
                        labyrinth[x, y] <= Constants.TILE_START_END_ID)
                    {
                        startPosition = new Vector2Int(x, y);
                    }
                    else if (
                        labyrinth[x, y] >= Constants.TILE_END_START_ID && 
                        labyrinth[x, y] <= Constants.TILE_END_END_ID)
                    {
                        endPosition = new Vector2Int(x, y);
                    }
                }
            }
        }

        GameObject InstantiateLabyrithTile(int x, int y, int tileId)
        {
            GameObject tile = null;

            Vector3 tilePosition = GetLabyrinthPositionInWorldPosition(x, y);

            tile = Instantiate(ressources.GetTilePrefabWithId(tileId), tilePosition, Quaternion.identity, gameObject.transform);

            return tile;
        }

        public Vector3 GetLabyrinthPositionInWorldPosition(int x, int y)
        {
            return new Vector3((x - startPosition.x) * Constants.TILE_SIZE, 0, (-y + startPosition.y) * Constants.TILE_SIZE);
        }

        public Vector3 GetLabyrinthPositionInWorldPosition(Vector2Int position)
        {
            return GetLabyrinthPositionInWorldPosition(position.x, position.y);
        }

        public Vector2Int GetWorldPositionInLabyrinthPosition(float x, float y)
        {
            return new Vector2Int(Mathf.RoundToInt((x / Constants.TILE_SIZE)) + startPosition.x, Mathf.RoundToInt((-y / Constants.TILE_SIZE)) + startPosition.y);
        }

        public Vector2Int GetLabyrithStartPosition()
        {
            return startPosition;
        }

        public Vector2Int GetLabyrithEndPosition()
        {
            return endPosition;
        }
        public Vector3 GetLabyrithEndPositionInWorldPosition()
        {
            return GetLabyrinthPositionInWorldPosition(endPosition.x, endPosition.y);
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
            if (GetIsTileWalkable(startPosition.x + 1, startPosition.y))
            {
                direction = 1;
            }
            // down
            else if (GetIsTileWalkable(startPosition.x, startPosition.y + 1))
            {
                direction = 2;
            }
            // Left
            else if (GetIsTileWalkable(startPosition.x - 1, startPosition.y))
            {
                direction = 3;
            }

            return direction;
        }

        public bool GetIsTileWalkable(int x, int y)
        {
            if (labyrinth == null)
                return false;

            if (x >= 0 && x < labyrinth.GetLength(0) && y >= 0 && y < labyrinth.GetLength(1))
            {
                if ((labyrinth[x, y] >= Constants.TILE_FLOOR_START_ID && labyrinth[x, y] <= Constants.TILE_FLOOR_END_ID)
                    || (labyrinth[x, y] >= Constants.TILE_START_START_ID && labyrinth[x, y] <= Constants.TILE_START_END_ID)
                    || (labyrinth[x, y] >= Constants.TILE_END_START_ID && labyrinth[x, y] <= Constants.TILE_END_END_ID))
                {
                    return true;
                }
            }

            return false;
        }

        public bool GetIsTileWalkable(Vector2Int tile)
        {
            return GetIsTileWalkable(tile.x, tile.y);
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
