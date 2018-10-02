using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLabyrinth : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger currentGameState;

    [SerializeField]
    ScriptableLabyrinth labyrinthData;

    Vector2Int startPosition;
    Vector2Int endPosition;

    int[,] labyrinth;
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

    private void Start()
    {
        currentGameState.valueChangedEvent += OnGameStateChanged;
    }

    public void OnGameStateChanged()
    {
        if(currentGameState.value == Constants.TUTORIAL_LABYRITH_READY)
        {
            GenerateLabyrinthVisual();
            currentGameState.value = Constants.PLAYING_TUTORIAL;
        }
        else if (currentGameState.value == Constants.LABYRITH_READY)
        {
            GenerateLabyrinthVisual();
            currentGameState.value = Constants.PLAYING;
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
                    startPosition.x = x;
                    startPosition.y = y;
                }
                else if (labyrinth[x, y] >= Constants.TILE_END_START_ID && labyrinth[x, y] <= Constants.TILE_END_END_ID)
                {
                    endPosition.x = x;
                    endPosition.y = y;
                }
            }
        }
    }

    GameObject InstantiateLabyrithTile(int x,int y,int tileId)
    {
        GameObject tile = null;
        Vector3 tilePosition = GetWorldPosition(x, y);

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

    Vector3 GetWorldPosition(int x, int y)
    {
        Vector3 worldPos = new Vector3();

        worldPos.x = (x - startPosition.x) * Constants.TILE_SIZE;
        worldPos.y = 0;
        worldPos.z = (-y + startPosition.y) * Constants.TILE_SIZE;

        return worldPos;
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
        return GetWorldPosition(endPosition.x, endPosition.y);
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

    public void DestroyLabyrinth()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        labyrinthTiles = null;

    }
}
