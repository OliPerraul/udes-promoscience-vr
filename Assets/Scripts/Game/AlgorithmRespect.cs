﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmRespect : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger algorithmId;

    [SerializeField]
    ScriptableFloat algorithmRespect;

    [SerializeField]
    ScriptableGameState gameState;

    [SerializeField]
    ScriptableBoolean isDiverging;

    [SerializeField]
    ScriptableTile playerPaintTile;

    [SerializeField]
    ScriptableAction playerReachedTheEnd;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    RightHandAlgorithm rightHandAlgorithm;

    [SerializeField]
    LongestStraightAlgorithm longestStraightAlgorithm;

    [SerializeField]
    ShortestFlighDistanceAlgorithm shortestFlighDistanceAlgorithm;

    [SerializeField]
    StandardAlgorithm standardAlgorithm;

    [SerializeField]
    GameObject algorithRespectBar;

    [SerializeField]
    Transform cameraTransform;

    bool isAlgorithmRespectActive = false;

    int errorCounter;

    const float E = 2.71828f;

    Vector2Int currentLabyrinthPosition;

    //Steps the two first value are the map position and the third value is the tile color value
    List<Vector3Int> algorithmSteps = new List<Vector3Int>();
    List<Vector3Int> playerSteps = new List<Vector3Int>();
    List<Vector3Int> wrongColorTilesWhenDiverging = new List<Vector3Int>();

    void Start()
    {
       gameState.valueChangedEvent += OnGameStateChanged;
       playerPaintTile.valueChangedEvent += OnPlayerPaintTile;
    }

    private void Update()
    {
        if (isAlgorithmRespectActive)
        {
            Vector2Int labyrinthPosition = labyrinth.GetWorldPositionInLabyrinthPosition(cameraTransform.position.x, cameraTransform.position.z);

            if(labyrinthPosition != currentLabyrinthPosition)
            {
                if (labyrinthPosition == labyrinth.GetLabyrithEndPosition() && !(algorithmRespect.Value < 1.0f))
                {
                    playerReachedTheEnd.FireAction();
                    return;
                }
                else if (!isDiverging.Value)
                {
                    int previousTileColorId = (int) labyrinth.GetTileColor(currentLabyrinthPosition);

                    if (labyrinthPosition.x != algorithmSteps[playerSteps.Count].x || labyrinthPosition.y != algorithmSteps[playerSteps.Count].y || previousTileColorId != algorithmSteps[playerSteps.Count - 1].z)
                    {
                        isDiverging.Value = true;
                        errorCounter++;
                        algorithmRespect.Value = RespectValueComputation((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
                    }
                    else
                    {
                        playerSteps[playerSteps.Count - 1] = new Vector3Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y, previousTileColorId);
                        playerSteps.Add(new Vector3Int(labyrinthPosition.x, labyrinthPosition.y, 0));
                    }
                }
                else
                {
                    if (labyrinthPosition.x == playerSteps[playerSteps.Count - 1].x && labyrinthPosition.y == playerSteps[playerSteps.Count - 1].y)
                    {
                        if(wrongColorTilesWhenDiverging.Count == 0)
                        {
                            isDiverging.Value = false;
                            algorithmRespect.Value = 1.0f;
                        }
                        else if(wrongColorTilesWhenDiverging.Count == 1 && wrongColorTilesWhenDiverging[0].x == labyrinthPosition.x && wrongColorTilesWhenDiverging[0].y == labyrinthPosition.y)
                        {
                            wrongColorTilesWhenDiverging.RemoveAt(0);
                            isDiverging.Value = false;
                            algorithmRespect.Value = 1.0f;
                        }
                        else
                        {
                            algorithmRespect.Value = RespectValueComputation((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
                        }
                    }
                    else
                    {
                        algorithmRespect.Value = RespectValueComputation((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
                    }
                }

                currentLabyrinthPosition = labyrinthPosition;
            }
        }
    }


    void OnGameStateChanged()
    {
        if (gameState.Value == GameState.PlayingTutorial || gameState.Value == GameState.Playing)
        {
            if(gameState.Value == GameState.PlayingTutorial)
            {
                SetAlgorithmStepsWithId(Constants.TUTORIAL_ALGORITHM);
            }
            else
            {
                SetAlgorithmStepsWithId(algorithmId.Value);
            }

            ResetAlgorithmRespect();

            algorithRespectBar.SetActive(true);
            isAlgorithmRespectActive = true;
        }
        else if (gameState.Value == GameState.WaitingForNextRound)
        {
            isAlgorithmRespectActive = false;
            algorithRespectBar.SetActive(false);
        }
    }

    void OnPlayerPaintTile()
    {
        //To avoid the fact that we don't know if it was painted before or will be painted right after, we keep the tile painting here instead of in TabletControls
        int tilePreviousColor = (int)labyrinth.GetTileColor(playerPaintTile.TilePosition);
        PaintTile(playerPaintTile.TilePosition, playerPaintTile.TileColor);

        if (isDiverging.Value)
        {
            Vector2Int labyrinthPosition = playerPaintTile.TilePosition;

            int tileColorId = (int)playerPaintTile.TileColor;

            for (int i = 0; i < wrongColorTilesWhenDiverging.Count; i++)
            {
                if (labyrinthPosition.x == wrongColorTilesWhenDiverging[i].x && labyrinthPosition.y == wrongColorTilesWhenDiverging[i].y)
                {
                    if (tileColorId == wrongColorTilesWhenDiverging[i].z)
                    {
                        wrongColorTilesWhenDiverging.RemoveAt(i);

                        if (labyrinthPosition.x == playerSteps[playerSteps.Count - 1].x && labyrinthPosition.y == playerSteps[playerSteps.Count - 1].y && wrongColorTilesWhenDiverging.Count == 0)
                        {
                            isDiverging.Value = false;
                            algorithmRespect.Value = 1.0f;
                        }
                        else
                        {
                            algorithmRespect.Value = RespectValueComputation((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
                        }
                    }

                    return;
                }
            }

            wrongColorTilesWhenDiverging.Add(new Vector3Int(labyrinthPosition.x, labyrinthPosition.y, tilePreviousColor));
            algorithmRespect.Value = RespectValueComputation((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
        }

      
    }

    void SetAlgorithmStepsWithId(int id)
    {
        if(id == Constants.RIGHT_HAND_ALGORITHM)
        {
            algorithmSteps = rightHandAlgorithm.GetAlgorithmSteps();
        }
        else if (id == Constants.LONGEST_STRAIGHT_ALGORITHM)
        {
            algorithmSteps = longestStraightAlgorithm.GetAlgorithmSteps();
        }
        else if (id == Constants.SHORTEST_FLIGHT_DISTANCE_ALGORITHM)
        {
            algorithmSteps = shortestFlighDistanceAlgorithm.GetAlgorithmSteps();
        }
        else if (id == Constants.STANDARD_ALGORITHM)
        {
            algorithmSteps = standardAlgorithm.GetAlgorithmSteps();
        }
    }

    void PaintTile(Vector2Int tilePosition, TileColor tileColor)
    {
        GameObject tile = labyrinth.GetTile(tilePosition);
        FloorPainter floorPainter = tile.GetComponentInChildren<FloorPainter>();

        if (floorPainter != null)
        {
            floorPainter.PaintFloorWithColor(tileColor);
        }
    }

    void PaintCurrentPositionTile()
    {
        Vector2Int position = labyrinth.GetWorldPositionInLabyrinthPosition(cameraTransform.position.x, cameraTransform.position.z);

        GameObject tile = labyrinth.GetTile(position.x, position.y);
        FloorPainter floorPainter = tile.GetComponentInChildren<FloorPainter>();

        if (floorPainter != null)
        {
            floorPainter.PaintFloor();
        }
    }


    float RespectValueComputation(float x)
    {
        return Mathf.Pow(E, -x/Constants.TILE_SIZE);
    }

    void ResetAlgorithmRespect()
    {
        playerSteps.Clear();
        wrongColorTilesWhenDiverging.Clear();
        errorCounter = 0;
        isDiverging.Value = false;
        algorithmRespect.Value = 1.0f;
        currentLabyrinthPosition = labyrinth.GetLabyrithStartPosition();
        playerSteps.Add(new Vector3Int(currentLabyrinthPosition.x, currentLabyrinthPosition.y, (int)labyrinth.GetTileColor(currentLabyrinthPosition)));
    }

    public void ReturnToDivergencePoint()//Not finished
    {
        //Update real player position
        currentLabyrinthPosition = new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y);
    }
}
