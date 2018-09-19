using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLabyrinth : MonoBehaviour
{
    [SerializeField]
    ScriptableLabyrinth labyrinthData;

    Vector2Int startPosition;

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


    void Start ()
    {
       // labyrinthData.valueChangedEvent += GenerateLabyrinthVisual;//no needed anymore because labyrinth is an other layer from labyrinthData, witch mean labyrinth data can be changed will labyrinth is running
    }


    public void GenerateLabyrinthVisual()
    {
        if (labyrinthTiles != null)
        {
            DestroyLabyrinth();
        }

        startPosition = labyrinthData.GetLabyrithStartPosition();
        labyrinthTiles = new GameObject[labyrinthData.GetLabyrithXLenght(), labyrinthData.GetLabyrithYLenght()];

        for (int x = 0; x < labyrinthData.GetLabyrithXLenght(); x++)
        {
            for (int y = 0; y < labyrinthData.GetLabyrithYLenght(); y++)
            {
                labyrinthTiles[x, y] = InstantiateLabyrithTile(x,y, labyrinthData.GetLabyrithValueAt(x,y));
            }
        }
    }

    GameObject InstantiateLabyrithTile(int x,int y,int tileId)
    {
        GameObject tile = null;
        Vector3 tilePosition = GetWorldPosition(x, y);

        if(tileId == Constants.TILE_FLOOR_START_ID)
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

        worldPos.x = (x - startPosition.x) * Constants.tileSize;
        worldPos.y = 0;
        worldPos.z = (-y + startPosition.y) * Constants.tileSize;

        return worldPos;
    }

    public Vector2Int GetLabyrithStartPosition()
    {
        return startPosition;
    }

    public int GetLabyrithXLenght()
    {
        return labyrinthTiles.GetLength(0);
    }

    public int GetLabyrithYLenght()
    {
        return labyrinthTiles.GetLength(1);
    }

    public TileInformation GetLabyrinthTileInfomation(int x, int y)
    {
        return labyrinthTiles[x, y].GetComponent<TileInformation>();
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
