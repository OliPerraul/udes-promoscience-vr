using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
//using UdeS.Promoscience.Game;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience
{
    public class AlgorithmRespect : MonoBehaviour
    {
        [SerializeField]
        ScriptableAlgorithm algorithm;

        [SerializeField]
        ScriptableFloat algorithmRespect;

        [SerializeField]
        ScriptableGameAction gameAction;

        [SerializeField]
        ScriptableClientGameState gameState;

        [SerializeField]
        ScriptableBoolean isDiverging;

        [SerializeField]
        ScriptableBoolean isRoundCompleted;

        [SerializeField]
        ScriptableAction labyrinthPositionChanged;

        [SerializeField]
        ScriptableTile playerPaintTile;

        [SerializeField]
        ScriptablePositionRotationAndTile playerPositionRotationAndTiles;

        [SerializeField]
        ScriptableAction playerReachedTheEnd;

        [SerializeField]
        ScriptableIntegerArray recordedSteps;

        [SerializeField]
        ScriptableBoolean returnToDivergencePointAnswer;

        [SerializeField]
        Labyrinth labyrinth;

        [SerializeField]
        RightHandAlgorithm rightHandAlgorithm;

        [SerializeField]
        LongestStraightAlgorithm longestStraightAlgorithm;

        [SerializeField]
        ShortestFlightDistanceAlgorithm shortestFlightDistanceAlgorithm;

        [SerializeField]
        StandardAlgorithm standardAlgorithm;

        [SerializeField]
        Controls.CameraRigWrapper cameraRig;


        bool isAlgorithmRespectActive = false;

        int errorCounter;

        const float E = 2.71828f;

        readonly float[] rotationByDirection = { 0, 90, 180, 270 };

        Vector2Int currentLabyrinthPosition;

        List<Tile> algorithmSteps = new List<Tile>();
        List<Tile> playerSteps = new List<Tile>();
        List<Tile> wrongColorTilesWhenDiverging = new List<Tile>();

        Stack<GameAction> stack = new Stack<GameAction>();

        Quaternion rotationAtDivergence;

        private int backtrack = 0;

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
                EvaluateAlgorithmRespectOnPositionChanged(labyrinth.GetWorldPositionInLabyrinthPosition(cameraRig.Transform.position.x, cameraRig.Transform.position.z), labyrinth.GetTileColor(currentLabyrinthPosition), cameraRig.Transform.rotation);
            }
        }

        void OnGameStateChanged()
        {
            if (gameState.Value == ClientGameState.PlayingTutorial || gameState.Value == ClientGameState.Playing)
            {
                ResetAlgorithmRespect();

                if (gameState.Value == ClientGameState.PlayingTutorial)
                {
                    SetAlgorithmStepsWithId(Utils.Algorithm.Tutorial);
                }
                else
                {
                    SetAlgorithmStepsWithId(algorithm.Value);

                    if (recordedSteps.Value.Length > 0)
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
            EvaluateAlgorithmRespectOnPaintTile(labyrinth.GetWorldPositionInLabyrinthPosition(cameraRig.Transform.position.x, cameraRig.Transform.position.z), playerPaintTile.TilePosition.x, playerPaintTile.TilePosition.y, playerPaintTile.TileColor, playerPaintTile.TilePreviousColor);
        }

        void EvaluateAlgorithmRespectOnPositionChanged(Vector2Int labyrinthPosition, TileColor previousTileColor, Quaternion rotation)
        {
            if (labyrinthPosition != currentLabyrinthPosition)
            {
                if (labyrinthPosition == labyrinth.GetLabyrithEndPosition() && !(algorithmRespect.Value < 1.0f))
                {
                    gameAction.SetAction(GameAction.CompletedRound);

                    if (gameState.Value == ClientGameState.Playing)
                    {
                        isRoundCompleted.Value = true;
                    }

                    playerReachedTheEnd.FireAction();

                    return;
                }
                else if (!isDiverging.Value)
                {
                    if (labyrinthPosition.x != algorithmSteps[playerSteps.Count].x
                        || labyrinthPosition.y != algorithmSteps[playerSteps.Count].y
                        || (previousTileColor != algorithmSteps[playerSteps.Count - 1].color))
                    {
                        isDiverging.Value = true;
                        algorithmRespect.Value = RespectValueComputation((new Vector2Int(
                            playerSteps[playerSteps.Count - 1].x, 
                            playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);

                        rotationAtDivergence = rotation;
                    }
                    else
                    {
                        playerSteps[playerSteps.Count - 1] = new Tile(
                            playerSteps[playerSteps.Count - 1].x, 
                            playerSteps[playerSteps.Count - 1].y, 
                            previousTileColor);

                        playerSteps.Add(new Tile(
                            labyrinthPosition.x, 
                            labyrinthPosition.y, 
                            TileColor.NoColor));
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
                            algorithmRespect.Value = RespectValueComputation((
                                new Vector2Int(
                                    playerSteps[playerSteps.Count - 1].x, 
                                    playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
                        }
                    }
                    else
                    {
                        algorithmRespect.Value = RespectValueComputation((
                            new Vector2Int(
                                playerSteps[playerSteps.Count - 1].x, 
                                playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
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

                                algorithmRespect.Value = 
                                    RespectValueComputation(
                                        (new Vector2Int(
                                            playerSteps[playerSteps.Count - 1].x, 
                                            playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
                            }

                            return;
                        }
                    }

                    wrongColorTilesWhenDiverging.Add(new Tile(x, y, previousColor));

                    algorithmRespect.Value = 
                        RespectValueComputation(
                            (new Vector2Int(
                                playerSteps[playerSteps.Count - 1].x,
                                playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + wrongColorTilesWhenDiverging.Count);
                }
            }
        }

        void SetAlgorithmStepsWithId(Utils.Algorithm algo)
        {
            if (algo == Utils.Algorithm.RightHand)
            {
                algorithmSteps = rightHandAlgorithm.GetAlgorithmSteps();
            }
            else if (algo == Utils.Algorithm.LongestStraight)
            {
                algorithmSteps = longestStraightAlgorithm.GetAlgorithmSteps();
            }
            else if (algo == Utils.Algorithm.ShortestFlightDistance)
            {
                algorithmSteps = shortestFlightDistanceAlgorithm.GetAlgorithmSteps();
            }
            else if (algo == Utils.Algorithm.Standard)
            {
                algorithmSteps = standardAlgorithm.GetAlgorithmSteps();
            }
        }

        float RespectValueComputation(float x)
        {
            return Mathf.Pow(E, -x / Constants.TILE_SIZE);
        }

        void ResetAlgorithmRespect()
        {
            playerSteps.Clear();
            wrongColorTilesWhenDiverging.Clear();
            backtrack = 0;
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

                Tile[] tiles = wrongColorTilesWhenDiverging.ToArray(); 

                Vector2Int gridPos = 
                    new Vector2Int(
                        playerSteps[playerSteps.Count - 1].x, 
                        playerSteps[playerSteps.Count - 1].y);

                Vector3 position = labyrinth.GetLabyrinthPositionInWorldPosition(
                    gridPos.x,
                    gridPos.y) + 
                    new Vector3(0, cameraRig.Transform.position.y, 0);                

                playerPositionRotationAndTiles.SetPositionRotationAndTiles(
                    position,
                    rotationAtDivergence, 
                    tiles);

                gameAction.SetAction(
                    GameAction.ReturnToDivergencePoint, 
                    gridPos, 
                    rotationAtDivergence, 
                    tiles,
                    playerSteps.ToArray());
            }
        }

        void StartWithSteps()
        {
            int[] steps = recordedSteps.Value;
            int forwardDirection = labyrinth.GetStartDirection();
            TileColor[,] tiles = new TileColor[labyrinth.GetLabyrithXLenght(), labyrinth.GetLabyrithYLenght()];
            Vector2Int position = labyrinth.GetLabyrithStartPosition();

            tiles[position.x, position.y] = TileColor.Yellow;

            for (int i = 0; i < steps.Length; i++)
            {
                GameAction gameAction = (GameAction)steps[i];

                if (gameAction == GameAction.MoveUp)
                {
                    position.y -= 1;
                    EvaluateAlgorithmRespectOnPositionChanged(position, tiles[currentLabyrinthPosition.x, currentLabyrinthPosition.y], GetRotationWithForwardDirection(forwardDirection));
                }
                else if (gameAction == GameAction.MoveRight)
                {
                    position.x += 1;
                    EvaluateAlgorithmRespectOnPositionChanged(position, tiles[currentLabyrinthPosition.x, currentLabyrinthPosition.y], GetRotationWithForwardDirection(forwardDirection));
                }
                else if (gameAction == GameAction.MoveDown)
                {
                    position.y += 1;
                    EvaluateAlgorithmRespectOnPositionChanged(position, tiles[currentLabyrinthPosition.x, currentLabyrinthPosition.y], GetRotationWithForwardDirection(forwardDirection));
                }
                else if (gameAction == GameAction.MoveLeft)
                {
                    position.x -= 1;
                    EvaluateAlgorithmRespectOnPositionChanged(position, tiles[currentLabyrinthPosition.x, currentLabyrinthPosition.y], GetRotationWithForwardDirection(forwardDirection));
                }
                else if (gameAction == GameAction.TurnRight)
                {
                    forwardDirection = (forwardDirection + 1) % 4;
                }
                else if (gameAction == GameAction.TurnLeft)
                {
                    forwardDirection = (forwardDirection - 1) < 0 ? 3 : (forwardDirection - 1);
                }
                else if (gameAction == GameAction.PaintFloorYellow)
                {
                    TileColor previousColor = tiles[position.x, position.y];
                    tiles[position.x, position.y] = TileColor.Yellow;

                    EvaluateAlgorithmRespectOnPaintTile(position, position.x, position.y, TileColor.Yellow, previousColor);
                }
                else if (gameAction == GameAction.PaintFloorRed)
                {
                    TileColor previousColor = tiles[position.x, position.y];
                    tiles[position.x, position.y] = TileColor.Red;

                    EvaluateAlgorithmRespectOnPaintTile(position, position.x, position.y, TileColor.Red, previousColor);
                }
                else if (gameAction == GameAction.UnpaintFloor)
                {
                    TileColor previousColor = tiles[position.x, position.y];
                    tiles[position.x, position.y] = TileColor.Grey;

                    EvaluateAlgorithmRespectOnPaintTile(position, position.x, position.y, TileColor.Grey, previousColor);
                }
                else if (gameAction == GameAction.ReturnToDivergencePoint)
                {
                    errorCounter += 5;

                    position = new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y);
                    forwardDirection = GetForwardDirectionWithRotation(rotationAtDivergence);

                    TileColor previousColor;

                    for (int j = wrongColorTilesWhenDiverging.Count - 1; j >= 0; j--)
                    {
                        Tile tile = wrongColorTilesWhenDiverging[j];

                        previousColor = tiles[tile.x, tile.y];
                        tiles[tile.x, tile.y] = tile.color;

                        EvaluateAlgorithmRespectOnPaintTile(position, tile.x, tile.y, tile.color, previousColor);
                    }

                    EvaluateAlgorithmRespectOnPositionChanged(
                        position, 
                        tiles[currentLabyrinthPosition.x, currentLabyrinthPosition.y], 
                        GetRotationWithForwardDirection(forwardDirection));
                }
            }

            Queue<Tile> paintedTile = new Queue<Tile>();

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[x, y] != TileColor.Grey)
                    {
                        paintedTile.Enqueue(new Tile(x, y, tiles[x, y]));
                    }
                }
            }

            Tile[] tilesToPaint = paintedTile.ToArray();

            Quaternion rotation = new Quaternion(0, 0, 0, 0);

            if (forwardDirection == 1)
            {
                rotation.eulerAngles = new Vector3(0, rotationByDirection[1], 0);
            }
            else if (forwardDirection == 2)
            {
                rotation.eulerAngles = new Vector3(0, rotationByDirection[2], 0);
            }
            else if (forwardDirection == 3)
            {
                rotation.eulerAngles = new Vector3(0, rotationByDirection[3], 0);
            }

            playerPositionRotationAndTiles.SetPositionRotationAndTiles(
                labyrinth.GetLabyrinthPositionInWorldPosition(position) + new Vector3(0, cameraRig.Transform.position.y, 0), 
                rotation, 
                tilesToPaint);
        }

        int GetForwardDirectionWithRotation(Quaternion rotation)
        {
            float y = rotation.eulerAngles.y;
            float epsilon = 1;

            if (y < rotationByDirection[1] + epsilon && y > rotationByDirection[1] - epsilon)
            {
                return 1;
            }
            else if (y < rotationByDirection[2] + epsilon && y > rotationByDirection[2] - epsilon)
            {
                return 2;
            }
            else if (y < rotationByDirection[3] + epsilon && y > rotationByDirection[3] - epsilon)
            {
                return 3;
            }

            return 0;
        }

        Quaternion GetRotationWithForwardDirection(int direction)
        {
            Quaternion rotation = new Quaternion(0, 0, 0, 0);
            rotation.eulerAngles = new Vector3(0, rotationByDirection[direction], 0);

            return rotation;
        }
    }
}
