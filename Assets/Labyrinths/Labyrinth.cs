using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience
{
    public class Labyrinth : MonoBehaviour
    {
        [SerializeField]
        ScriptableClientGameState gameState;

        [SerializeField]
        ScriptableLabyrinth labyrinthData;

        [SerializeField]
        ScriptableRessources ressources;

        GameObject[,] labyrinthTiles;

        int[,] labyrinth;

        Vector2Int startPosition;
        Vector2Int endPosition;

        private void Start()
        {
            gameState.valueChangedEvent += OnGameStateChanged;
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
            else if (gameState.Value == ClientGameState.ViewingLocalPlayback)
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

            SetLabyrith();

            labyrinthTiles = new GameObject[labyrinth.GetLength(0), labyrinth.GetLength(1)];

            for (int x = 0; x < labyrinthTiles.GetLength(0); x++)
            {
                for (int y = 0; y < labyrinthTiles.GetLength(1); y++)
                {
                    labyrinthTiles[x, y] = InstantiateLabyrithTile(x, y, labyrinth[x, y]);
                }
            }
        }

        void SetLabyrith()
        {
            labyrinth = new int[labyrinthData.GetLabyrithXLenght(), labyrinthData.GetLabyrithYLenght()];

            for (int x = 0; x < labyrinth.GetLength(0); x++)
            {
                for (int y = 0; y < labyrinth.GetLength(1); y++)
                {
                    labyrinth[x, y] = labyrinthData.GetLabyrithValueAt(x, y);

                    if (labyrinth[x, y] >= Constants.TILE_START_START_ID && labyrinth[x, y] <= Constants.TILE_START_END_ID)
                    {
                        startPosition = new Vector2Int(x, y);
                    }
                    else if (labyrinth[x, y] >= Constants.TILE_END_START_ID && labyrinth[x, y] <= Constants.TILE_END_END_ID)
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
            int direction = 0;

            if (GetIsTileWalkable(startPosition.x + 1, startPosition.y))
            {
                direction = 1;
            }
            else if (GetIsTileWalkable(startPosition.x, startPosition.y + 1))
            {
                direction = 2;
            }
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
                GameObject.Destroy(child.gameObject);
            }

            labyrinthTiles = null;

        }
    }
}
