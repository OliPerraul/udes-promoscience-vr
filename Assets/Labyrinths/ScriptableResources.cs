using System;
using System.Collections;
//using UdeS.Promoscience.Utils;
using UnityEngine;

namespace UdeS.Promoscience.Labyrinths
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/Ressources", order = 1)]
    public class ScriptableResources : ScriptableObject
    {
        [SerializeField]
        public Labyrinth Labyrinth; 

        [SerializeField]
        GameObject startTilePrefab;
        [SerializeField]
        GameObject floorTilePrefab;
        [SerializeField]
        GameObject endTilePrefab;
        [SerializeField]
        GameObject wallTilePrefab;

        [SerializeField]
        GameObject romeStartTilePrefab;
        [SerializeField]
        GameObject romeFloorTilePrefab;
        [SerializeField]
        GameObject romeEndTilePrefab;
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
        GameObject ptolStartTilePrefab;
        [SerializeField]
        GameObject ptolFloorTilePrefab;
        [SerializeField]
        GameObject ptolEndTilePrefab;
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
        GameObject britStartTilePrefab;
        [SerializeField]
        GameObject britFloorTilePrefab;
        [SerializeField]
        GameObject britEndTilePrefab;
        [SerializeField]
        GameObject britHorizontalWallTilePrefab;
        [SerializeField]
        GameObject britVerticalWallTilePrefab;
        [SerializeField]
        GameObject britTowerWallTilePrefab;
        [SerializeField]
        GameObject britTowerWall2TilePrefab;


        [SerializeField]
        GameObject kartStartTilePrefab;
        [SerializeField]
        GameObject kartFloorTilePrefab;
        [SerializeField]
        GameObject kartEndTilePrefab;
        [SerializeField]
        GameObject kartHorizontalWallTilePrefab;
        [SerializeField]
        GameObject kartHorizontalWallBTilePrefab;
        [SerializeField]
        GameObject kartVerticalWallTilePrefab;
        [SerializeField]
        GameObject kartVerticalWallBTilePrefab;
        [SerializeField]
        GameObject kartHorizontalWallScaffoldingTilePrefab;
        [SerializeField]
        GameObject kartHorizontalWallScaffoldingBTilePrefab;
        [SerializeField]
        GameObject kartVerticalWallScaffoldingTilePrefab;
        [SerializeField]
        GameObject kartVerticalWallScaffoldingBTilePrefab;
        [SerializeField]
        GameObject kartTowerWallTilePrefab;
        [SerializeField]
        GameObject kartTowerWall2TilePrefab;
        [SerializeField]
        GameObject kartTowerWallScaffoldingTilePrefab;


        public GameObject GetTilePrefabWithId(int tileId)
        {
            GameObject tile = null;

            if (tileId >= Utils.TILE_START_START_ID && tileId <= Utils.TILE_START_END_ID)
            {
                if (tileId == Utils.TILE_ROME_START_ID)
                {
                    tile = romeStartTilePrefab;
                }
                else if (tileId == Utils.TILE_PTOL_START_ID)
                {
                    tile = ptolStartTilePrefab;
                }
                else if (tileId == Utils.TILE_BRIT_START_ID)
                {
                    tile = britStartTilePrefab;
                }
                else if (tileId == Utils.TILE_KART_START_ID)
                {
                    tile = kartStartTilePrefab;
                }
                else
                {
                    tile = startTilePrefab;
                }
            }
            if (tileId >= Utils.TILE_FLOOR_START_ID && tileId <= Utils.TILE_FLOOR_END_ID)
            {
                if (tileId == Utils.TILE_ROME_FLOOR_ID)
                {
                    tile = romeFloorTilePrefab;
                }
                else if (tileId == Utils.TILE_PTOL_FLOOR_ID)
                {
                    tile = ptolFloorTilePrefab;
                }
                else if (tileId == Utils.TILE_BRIT_FLOOR_ID)
                {
                    tile = britFloorTilePrefab;
                }
                else if (tileId == Utils.TILE_KART_FLOOR_ID)
                {
                    tile = kartFloorTilePrefab;
                }
                else
                {
                    tile = floorTilePrefab;
                }
            }
            if (tileId >= Utils.TILE_END_START_ID && tileId <= Utils.TILE_END_END_ID)
            {
                if (tileId == Utils.TILE_ROME_END_ID)
                {
                    tile = romeEndTilePrefab;
                }
                else if (tileId == Utils.TILE_PTOL_END_ID)
                {
                    tile = ptolEndTilePrefab;
                }
                else if (tileId == Utils.TILE_BRIT_END_ID)
                {
                    tile = britEndTilePrefab;
                }
                else if (tileId == Utils.TILE_KART_END_ID)
                {
                    tile = kartEndTilePrefab;
                }
                else
                {
                    tile = endTilePrefab;
                }
            }
            if (tileId >= Utils.TILE_WALL_START_ID && tileId <= Utils.TILE_WALL_END_ID)
            {
                if (tileId == Utils.TILE_ROME_HORIZONTAL_WALL_ID)
                {
                    tile = romeHorizontalWallTilePrefab;
                }
                else if (tileId == Utils.TILE_ROME_HORIZONTAL_WALL_B_ID)
                {
                    tile = romeHorizontalWallBTilePrefab;
                }
                else if (tileId == Utils.TILE_ROME_VERTICAL_WALL_ID)
                {
                    tile = romeVerticalWallTilePrefab;
                }
                else if (tileId == Utils.TILE_ROME_VERTICAL_WALL_B_ID)
                {
                    tile = romeVerticalWallBTilePrefab;
                }
                else if (tileId == Utils.TILE_ROME_TOWER_WALL_ID)
                {
                    tile = romeTowerWallTilePrefab;
                }
                else if (tileId == Utils.TILE_PTOL_HORIZONTAL_WALL_ID)
                {
                    tile = ptolHorizontalWallTilePrefab;
                }
                else if (tileId == Utils.TILE_PTOL_HORIZONTAL_WALL_B_ID)
                {
                    tile = ptolHorizontalWallBTilePrefab;
                }
                else if (tileId == Utils.TILE_PTOL_VERTICAL_WALL_ID)
                {
                    tile = ptolVerticalWallTilePrefab;
                }
                else if (tileId == Utils.TILE_PTOL_VERTICAL_WALL_B_ID)
                {
                    tile = ptolVerticalWallBTilePrefab;
                }
                else if (tileId == Utils.TILE_PTOL_TOWER_WALL_ID)
                {
                    tile = ptolTowerWallTilePrefab;
                }
                else if (tileId == Utils.TILE_BRIT_HORIZONTAL_WALL_ID)
                {
                    tile = britHorizontalWallTilePrefab;
                }
                else if (tileId == Utils.TILE_BRIT_VERTICAL_WALL_ID)
                {
                    tile = britVerticalWallTilePrefab;
                }
                else if (tileId == Utils.TILE_BRIT_TOWER_WALL_ID)
                {
                    tile = britTowerWallTilePrefab;
                }
                else if (tileId == Utils.TILE_BRIT_TOWER_WALL_2_ID)
                {
                    tile = britTowerWall2TilePrefab;
                }
                else if (tileId == Utils.TILE_KART_HORIZONTAL_WALL_ID)
                {
                    tile = kartHorizontalWallTilePrefab;
                }
                else if (tileId == Utils.TILE_KART_HORIZONTAL_WALL_B_ID)
                {
                    tile = kartHorizontalWallBTilePrefab;
                }
                else if (tileId == Utils.TILE_KART_VERTICAL_WALL_ID)
                {
                    tile = kartVerticalWallTilePrefab;
                }
                else if (tileId == Utils.TILE_KART_VERTICAL_WALL_B_ID)
                {
                    tile = kartVerticalWallBTilePrefab;
                }
                else if (tileId == Utils.TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_ID)
                {
                    tile = kartHorizontalWallScaffoldingTilePrefab;
                }
                else if (tileId == Utils.TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_B_ID)
                {
                    tile = kartHorizontalWallScaffoldingBTilePrefab;
                }
                else if (tileId == Utils.TILE_KART_VERTICAL_WALL_SCAFFOLDING_ID)
                {
                    tile = kartVerticalWallScaffoldingTilePrefab;
                }
                else if (tileId == Utils.TILE_KART_VERTICAL_WALL_SCAFFOLDING_B_ID)
                {
                    tile = kartVerticalWallScaffoldingBTilePrefab;
                }
                else if (tileId == Utils.TILE_KART_TOWER_WALL_ID)
                {
                    tile = kartTowerWallTilePrefab;
                }
                else if (tileId == Utils.TILE_KART_TOWER_WALL_2_ID)
                {
                    tile = kartTowerWall2TilePrefab;
                }
                else if (tileId == Utils.TILE_KART_TOWER_WALL_SCAFFOLDING_ID)
                {
                    tile = kartTowerWallScaffoldingTilePrefab;
                }
                else
                {
                    tile = wallTilePrefab;
                }
            }

            return tile;
        }
    }
}

