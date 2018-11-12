using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Ressources", order = 1)]
public class ScriptableRessources : ScriptableObject
{
    [SerializeField]
    GameObject startTilePrefab;
    [SerializeField]
    GameObject floorTilePrefab;
    [SerializeField]
    GameObject wallTilePrefab;
    [SerializeField]
    GameObject endTilePrefab;

    [SerializeField]
    GameObject romeStartTilePrefab;
    [SerializeField]
    GameObject romeFloorTilePrefab;
    [SerializeField]
    GameObject romeHorizontalWallTilePrefab;
    [SerializeField]
    GameObject romeHorizontalWallBTilePrefab;
    [SerializeField]
    GameObject romeVerticalWallTilePrefab;
    [SerializeField]
    GameObject romeVerticalWallBTilePrefab;
    [SerializeField]
    GameObject romeTowerWallTilePrefab;
    [SerializeField]
    GameObject romeEndTilePrefab;

    [SerializeField]
    GameObject ptolStartTilePrefab;
    [SerializeField]
    GameObject ptolFloorTilePrefab;
    [SerializeField]
    GameObject ptolHorizontalWallTilePrefab;
    [SerializeField]
    GameObject ptolHorizontalWallBTilePrefab;
    [SerializeField]
    GameObject ptolVerticalWallTilePrefab;
    [SerializeField]
    GameObject ptolVerticalWallBTilePrefab;
    [SerializeField]
    GameObject ptolTowerWallTilePrefab;
    [SerializeField]
    GameObject ptolEndTilePrefab;

    public GameObject GetTilePrefabWithId(int tileId)
    {
        GameObject tile = null;

        if(tileId >= Constants.TILE_START_START_ID && tileId <= Constants.TILE_START_END_ID)
        {
            if (tileId == Constants.TILE_ROME_START_ID)
            {
                tile = romeStartTilePrefab;
            }
            else if (tileId == Constants.TILE_PTOL_START_ID)
            {
                tile = ptolStartTilePrefab;
            }
            else
            {
                tile = startTilePrefab;
            }
        }
        if (tileId >= Constants.TILE_FLOOR_START_ID && tileId <= Constants.TILE_FLOOR_END_ID)
        {
            if (tileId == Constants.TILE_ROME_FLOOR_ID)
            {
                tile = romeFloorTilePrefab;
            }
            else if (tileId == Constants.TILE_PTOL_FLOOR_ID)
            {
                tile = ptolFloorTilePrefab;
            }
            else
            {
                tile = floorTilePrefab;
            }
        }
        if (tileId >= Constants.TILE_END_START_ID && tileId <= Constants.TILE_END_END_ID)
        {
            if (tileId == Constants.TILE_ROME_END_ID)
            {
                tile = romeEndTilePrefab;
            }
            else if (tileId == Constants.TILE_PTOL_END_ID)
            {
                tile = ptolEndTilePrefab;
            }
            else
            {
                tile = endTilePrefab;
            }
        }
        if (tileId >= Constants.TILE_WALL_START_ID && tileId <= Constants.TILE_WALL_END_ID)
        {
            if (tileId == Constants.TILE_ROME_HORIZONTAL_WALL_ID)
            {
                tile = romeHorizontalWallTilePrefab;
            }
            else if (tileId == Constants.TILE_ROME_HORIZONTAL_WALL_B_ID)
            {
                tile = romeHorizontalWallBTilePrefab;
            }
            else if (tileId == Constants.TILE_ROME_VERTICAL_WALL_ID)
            {
                tile = romeVerticalWallTilePrefab;
            }
            else if (tileId == Constants.TILE_ROME_VERTICAL_WALL_B_ID)
            {
                tile = romeVerticalWallBTilePrefab;
            }
            else if (tileId == Constants.TILE_ROME_TOWER_WALL_ID)
            {
                tile = romeTowerWallTilePrefab;
            }
            else if (tileId == Constants.TILE_PTOL_HORIZONTAL_WALL_ID)
            {
                tile = ptolHorizontalWallTilePrefab;
            }
            else if (tileId == Constants.TILE_PTOL_HORIZONTAL_WALL_B_ID)
            {
                tile = ptolHorizontalWallBTilePrefab;
            }
            else if (tileId == Constants.TILE_PTOL_VERTICAL_WALL_ID)
            {
                tile = ptolVerticalWallTilePrefab;
            }
            else if (tileId == Constants.TILE_PTOL_VERTICAL_WALL_B_ID)
            {
                tile = ptolVerticalWallBTilePrefab;
            }
            else if (tileId == Constants.TILE_PTOL_TOWER_WALL_ID)
            {
                tile = ptolTowerWallTilePrefab;
            }
            else
            {
                tile = wallTilePrefab;
            }
        }

        return tile;
    }
}

