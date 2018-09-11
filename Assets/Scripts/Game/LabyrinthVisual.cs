using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthVisual : MonoBehaviour
{
    const float sizeOfTiles = 5.0f;

    [SerializeField]
    ScriptableLabyrinth labyrinthData;
    //List of gameObjectPrefab could be load with a resource manager but for now....
    GameObject[,] labyrinthTiles;

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
        //Usually should listen to game status but for now let's hijack it
        labyrinthData.valueChangedEvent += GenerateLabyrinthVisual;

    }

    void GenerateLabyrinthVisual()
    {
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

        worldPos.x = (x - labyrinthData.GetLabyrithStartPosition().x) * sizeOfTiles;
        worldPos.y = (y - labyrinthData.GetLabyrithStartPosition().y) * sizeOfTiles;
        worldPos.z = 0;

        return worldPos;
    }
	

}
