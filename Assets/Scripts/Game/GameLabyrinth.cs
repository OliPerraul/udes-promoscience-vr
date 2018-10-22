using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLabyrinth : MonoBehaviour
{
    [SerializeField]
    ScriptableGameState gameState;

    [SerializeField]
    ScriptableLabyrinth labyrinthData;

    GameObject[,] labyrinthTiles;

    //List of gameObjectPrefab could be load with a resource manager but for now....
    [SerializeField]
    GameObject startTilePrefab;
    [SerializeField]
    GameObject floorTilePrefab;
    [SerializeField]
    GameObject wallTilePrefab;
    [SerializeField]
    GameObject endTilePrefab;

    int[,] labyrinth;

    Vector2Int startPosition;
    Vector2Int endPosition;

    private void Start()
    {
        gameState.valueChangedEvent += OnGameStateChanged;
    }

    public void OnGameStateChanged()
    {
        if(gameState.value == GameState.TutorialLabyrinthReady)
        {
            GenerateLabyrinthVisual();
            gameState.value = GameState.PlayingTutorial;
        }
        else if (gameState.value == GameState.LabyrithReady)
        {
            GenerateLabyrinthVisual();
            gameState.value = GameState.Playing;
        }
    }

    void GenerateLabyrinthVisual()
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
                labyrinthTiles[x, y] = InstantiateLabyrithTile(x,y, labyrinth[x,y]);
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
                labyrinth[x,y] = labyrinthData.GetLabyrithValueAt(x, y);

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

    GameObject InstantiateLabyrithTile(int x,int y,int tileId)
    {
        GameObject tile = null;
        Vector3 tilePosition = GetLabyrinthPositionInWorldPosition(x, y);

        if(tileId == Constants.TILE_START_1_ID)
        {
            tile = (GameObject) Instantiate(startTilePrefab, tilePosition, Quaternion.identity, gameObject.transform);
        }
        else if(tileId == Constants.TILE_FLOOR_1_ID)
        {
            tile = (GameObject) Instantiate(floorTilePrefab, tilePosition, Quaternion.identity, gameObject.transform);
        }
        else if (tileId == Constants.TILE_WALL_1_ID)
        {
            tile = (GameObject) Instantiate(wallTilePrefab, tilePosition, Quaternion.identity, gameObject.transform);
        }
        else if (tileId == Constants.TILE_END_1_ID)
        {
            tile = (GameObject) Instantiate(endTilePrefab, tilePosition, Quaternion.identity, gameObject.transform);
        }

        return tile;
    }

    public Vector3 GetLabyrinthPositionInWorldPosition(int x, int y)
    {
        return new Vector3((x - startPosition.x) * Constants.TILE_SIZE, 0, (-y + startPosition.y) * Constants.TILE_SIZE);
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

        if(GetIsTileWalkable(startPosition.x + 1, startPosition.y))
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
        bool isWalkable = false;

        if (x >= 0 && x < labyrinth.GetLength(0) && y >= 0 && y < labyrinth.GetLength(1))
        {
            if ((labyrinth[x, y] >= Constants.TILE_FLOOR_START_ID && labyrinth[x, y] <= Constants.TILE_FLOOR_END_ID)
                || (labyrinth[x, y] >= Constants.TILE_START_START_ID && labyrinth[x, y] <= Constants.TILE_START_END_ID)
                || (labyrinth[x, y] >= Constants.TILE_END_START_ID && labyrinth[x, y] <= Constants.TILE_END_END_ID))
            {
                isWalkable = true;
            }
        }

        return isWalkable;
    }

    public bool GetIsTileWalkable(Vector2Int tile)
    {
        return GetIsTileWalkable(tile.x, tile.y);
    }

    public TileColor GetTileColorId(Vector2Int tile)
    {
        TileColor colorId = TileColor.NoColor;
        FloorPainter floorPainter = labyrinthTiles[tile.x, tile.y].GetComponentInChildren<FloorPainter>();
        if (floorPainter != null)
        {
            colorId = floorPainter.GetFloorColorId();
        }
        return colorId;
    }

    public void SetTileColorWithId(Vector2Int tile, int colorId)
    {
        FloorPainter floorPainter = labyrinthTiles[tile.x, tile.y].GetComponentInChildren<FloorPainter>();
        if (floorPainter != null)
        {
            floorPainter.PaintFloorWithColorId(colorId);
        }
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
