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
    ScriptableClientGameState gameState;

    [SerializeField]
    ScriptableBoolean isDiverging;

    [SerializeField]
    ScriptableAction labyrinthPositionChanged;

    [SerializeField]
    ScriptableTile playerPaintTile;

    [SerializeField]
    ScriptablePositionRotationAndTile playerPositionRotationAndTiles;

    [SerializeField]
    ScriptableAction playerReachedTheEnd;

    [SerializeField]
    ScriptableIntegerArray playerStartSteps;

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
       labyrinthPositionChanged.action += OnLabyrinthPositionChanged;
       returnToDivergencePointAnswer.valueChangedEvent += OnReturnToDivergencePointAnswer;
    }

    void OnLabyrinthPositionChanged()
    {
        if (isAlgorithmRespectActive)
        {
            EvaluateAlgorithmRespectOnPositionChanged(labyrinth.GetWorldPositionInLabyrinthPosition(cameraTransform.position.x, cameraTransform.position.z));
        }
    }
    


    void OnGameStateChanged()
    {
        if (gameState.Value == ClientGameState.PlayingTutorial || gameState.Value == ClientGameState.Playing)
        {
            ResetAlgorithmRespect();

            if (gameState.Value == ClientGameState.PlayingTutorial)
            {
                SetAlgorithmStepsWithId(Algorithm.Tutorial);
            }
            else
            {
                SetAlgorithmStepsWithId(algorithm.Value);

                if(playerStartSteps.Value.Length > 0)
                {
                    StartWithSteps();
                }
            }

            isAlgorithmRespectActive = true;
        }
        else if (gameState.Value == ClientGameState.WaitingForNextRound)
        {
            isAlgorithmRespectActive = false;
        }
    }

    void OnPlayerPaintTile()
    {
        EvaluateAlgorithmRespectOnPaintTile(labyrinth.GetWorldPositionInLabyrinthPosition(cameraTransform.position.x, cameraTransform.position.z), playerPaintTile.TilePosition.x, playerPaintTile.TilePosition.y, playerPaintTile.TileColor, playerPaintTile.TilePreviousColor);
    }

    void EvaluateAlgorithmRespectOnPositionChanged(Vector2Int labyrinthPosition)
    {
        if (labyrinthPosition != currentLabyrinthPosition)
        {
            if (labyrinthPosition == labyrinth.GetLabyrithEndPosition() && !(algorithmRespect.Value < 1.0f))
            {
                playerReachedTheEnd.FireAction();
                return;
            }
            else if (!isDiverging.Value)
            {
                TileColor previousTileColor = labyrinth.GetTileColor(currentLabyrinthPosition);

                if (labyrinthPosition.x != algorithmSteps[playerSteps.Count].x
                    || labyrinthPosition.y != algorithmSteps[playerSteps.Count].y
                    || (previousTileColor != algorithmSteps[playerSteps.Count - 1].color))
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
                    if (wrongColorTilesWhenDiverging.Count == 0)
                    {
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

    void EvaluateAlgorithmRespectOnPaintTile(Vector2Int labyrinthPosition, int x, int y, TileColor color, TileColor previousColor)
    {
        if (isDiverging.Value)
        {
            if (x != playerSteps[playerSteps.Count - 1].x
                || y != playerSteps[playerSteps.Count - 1].y)
            {
                for (int i = 0; i < wrongColorTilesWhenDiverging.Count; i++)
                {
                    if (x == wrongColorTilesWhenDiverging[i].x
                        && y == wrongColorTilesWhenDiverging[i].y)
                    {
                        if (color == wrongColorTilesWhenDiverging[i].color)
                        {
                            wrongColorTilesWhenDiverging.RemoveAt(i);

                            algorithmRespect.Value = RespectValueComputation((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
                        }

                        return;
                    }
                }

                wrongColorTilesWhenDiverging.Add(new Tile(x, y, previousColor));
                algorithmRespect.Value = RespectValueComputation((new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
            }
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

    void StartWithSteps()
    {
        int[] steps = playerStartSteps.Value;
        int forwardDirection = labyrinth.GetStartDirection();
        TileColor[,] tiles = new TileColor[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()];
        Vector2Int position = labyrinth.GetLabyrithStartPosition();

        for (int i = 0; i < steps.Length; i++)
        {
            GameAction action = (GameAction) steps[i];

            if (action == GameAction.MoveUp)
            {
                position.y -= 1;
                EvaluateAlgorithmRespectOnPositionChanged(position);
            }
            else if(action == GameAction.MoveRight)
            {
                position.x += 1;
                EvaluateAlgorithmRespectOnPositionChanged(position);
            }
            else if (action == GameAction.MoveDown)
            {
                position.y += 1;
                EvaluateAlgorithmRespectOnPositionChanged(position);
            }
            else if (action == GameAction.MoveLeft)
            {
                position.x -= 1;
                EvaluateAlgorithmRespectOnPositionChanged(position);
            }
            else if (action == GameAction.PaintFloorYellow)
            {
                TileColor previousColor = tiles[position.x, position.y];
                tiles[position.x, position.y] = TileColor.Yellow;

                EvaluateAlgorithmRespectOnPaintTile(position, position.x, position.y, TileColor.Yellow, previousColor);
            }
            else if (action == GameAction.PaintFloorRed)
            {
                TileColor previousColor = tiles[position.x, position.y];
                tiles[position.x, position.y] = TileColor.Red;

                EvaluateAlgorithmRespectOnPaintTile(position, position.x, position.y, TileColor.Red, previousColor);
            }
            else if (action == GameAction.UnpaintFloor)
            {
                TileColor previousColor = tiles[position.x, position.y];
                tiles[position.x, position.y] = TileColor.Grey;

                EvaluateAlgorithmRespectOnPaintTile(position, position.x, position.y, TileColor.Grey, previousColor);
            }
            else if (action == GameAction.TurnRight)
            {
                forwardDirection = (forwardDirection + 1) % 4;
            }
            else if (action == GameAction.TurnLeft)
            {
                forwardDirection = (forwardDirection - 1) < 0 ? 3 : (forwardDirection - 1);
            }
        }

        Queue<Tile> paintedTile = new Queue<Tile>();

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if(tiles[x, y] != TileColor.Grey)
                {
                    paintedTile.Enqueue(new Tile(x, y, tiles[x, y]));
                }
            }
        }

        Tile[] tilesToPaint = paintedTile.ToArray();

        Quaternion rotation = new Quaternion(0, 0, 0, 0);

        if (forwardDirection == 1)
        {
            rotation.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (forwardDirection == 2)
        {
            rotation.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (forwardDirection == 3)
        {
            rotation.eulerAngles = new Vector3(0, 270, 0);
        }

        playerPositionRotationAndTiles.SetPositionRotationAndTiles(labyrinth.GetLabyrinthPositionInWorldPosition(position) + new Vector3(0, cameraTransform.position.y, 0), rotation, tilesToPaint);
    }
}
