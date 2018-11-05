using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmRespect : MonoBehaviour
{
    [SerializeField]
    ScriptableAlgorithm algorithm;

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
    ScriptablePositionRotationAndTile playerPositionRotationAndTiles;

    [SerializeField]
    ScriptableBoolean returnToDivergencePointAnswer;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    RightHandAlgorithm rightHandAlgorithm;

    [SerializeField]
    LongestStraightAlgorithm longestStraightAlgorithm;

    [SerializeField]
    ShortestFlightDistanceAlgorithm shortestFlightDistanceAlgorithm;

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

    List<Tile> algorithmSteps = new List<Tile>();
    List<Tile> playerSteps = new List<Tile>();
    List<Tile> wrongColorTilesWhenDiverging = new List<Tile>();

    Quaternion rotationAtDivergence;

    void Start()
    {
       gameState.valueChangedEvent += OnGameStateChanged;
       playerPaintTile.valueChangedEvent += OnPlayerPaintTile;
       returnToDivergencePointAnswer.valueChangedEvent += OnReturnToDivergencePointAnswer;
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
                    TileColor previousTileColor = labyrinth.GetTileColor(currentLabyrinthPosition);

                    if (labyrinthPosition.x != algorithmSteps[playerSteps.Count].x || labyrinthPosition.y != algorithmSteps[playerSteps.Count].y || previousTileColor != algorithmSteps[playerSteps.Count - 1].color)
                    {
                        isDiverging.Value = true;
                        errorCounter++;
                        algorithmRespect.Value = RespectValueComputation((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
                        rotationAtDivergence = cameraTransform.rotation;
                    }
                    else
                    {
                        playerSteps[playerSteps.Count - 1] = new Tile(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y, previousTileColor);
                        playerSteps.Add(new Tile(labyrinthPosition.x, labyrinthPosition.y, TileColor.NoColor));
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
                SetAlgorithmStepsWithId(Algorithm.Tutorial);
            }
            else
            {
                SetAlgorithmStepsWithId(algorithm.Value);
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
        TileColor tilePreviousColor = labyrinth.GetTileColor(playerPaintTile.TilePosition);
        PaintTile(playerPaintTile.TilePosition, playerPaintTile.TileColor);
        
        if(!isDiverging.Value)
        {
            if (playerPaintTile.TilePosition != currentLabyrinthPosition)
            {
                TileColor previousTileColor = labyrinth.GetTileColor(currentLabyrinthPosition);

                if (previousTileColor != algorithmSteps[playerSteps.Count - 1].color 
                    || playerPaintTile.TilePosition.x != algorithmSteps[playerSteps.Count].x 
                    || playerPaintTile.TilePosition.y != algorithmSteps[playerSteps.Count].y)
                {
                    isDiverging.Value = true;
                    errorCounter++;
                    algorithmRespect.Value = RespectValueComputation((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - currentLabyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
                    rotationAtDivergence = cameraTransform.rotation;
                }
            }
        }

        if (isDiverging.Value)
        {
            Vector2Int labyrinthPosition = playerPaintTile.TilePosition;

            TileColor tileColorId = playerPaintTile.TileColor;

            for (int i = 0; i < wrongColorTilesWhenDiverging.Count; i++)
            {
                if (labyrinthPosition.x == wrongColorTilesWhenDiverging[i].x && labyrinthPosition.y == wrongColorTilesWhenDiverging[i].y)
                {
                    if (tileColorId == wrongColorTilesWhenDiverging[i].color)
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

            wrongColorTilesWhenDiverging.Add(new Tile(labyrinthPosition.x, labyrinthPosition.y, tilePreviousColor));
            algorithmRespect.Value = RespectValueComputation((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
        }
    }

    void SetAlgorithmStepsWithId(Algorithm algo)
    {
        if(algo == Algorithm.RightHand)
        {
            algorithmSteps = rightHandAlgorithm.GetAlgorithmSteps();
        }
        else if (algo == Algorithm.LongestStraight)
        {
            algorithmSteps = longestStraightAlgorithm.GetAlgorithmSteps();
        }
        else if (algo == Algorithm.ShortestFlightDistance)
        {
            algorithmSteps = shortestFlightDistanceAlgorithm.GetAlgorithmSteps();
        }
        else if (algo == Algorithm.Standard)
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
        playerSteps.Add(new Tile(currentLabyrinthPosition.x, currentLabyrinthPosition.y, labyrinth.GetTileColor(currentLabyrinthPosition)));
    }

    public void OnReturnToDivergencePointAnswer()
    {
        if (returnToDivergencePointAnswer.Value)
        {
            errorCounter += 5;

            Tile[] tiles = new Tile[wrongColorTilesWhenDiverging.Count];

            for (int i = 0; i < wrongColorTilesWhenDiverging.Count; i++)
            {
                tiles[i] = wrongColorTilesWhenDiverging[i];
            }

            Vector3 position = labyrinth.GetLabyrinthPositionInWorldPosition(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) + new Vector3(0, cameraTransform.position.y, 0);
            Quaternion rotation = rotationAtDivergence;

            playerPositionRotationAndTiles.SetPositionRotationAndTiles(position, rotation, tiles);
        }
    }
}
