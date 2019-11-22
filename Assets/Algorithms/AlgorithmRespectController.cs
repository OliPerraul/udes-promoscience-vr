using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
////using UdeS.Promoscience.Utils;
//using UdeS.Promoscience.Game;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.Controls;

namespace UdeS.Promoscience.Algorithms
{
    public class AlgorithmRespectController : MonoBehaviour
    {
        [SerializeField]
        AvatarControllerAsset controls;

        [SerializeField]
        AlgorithmRespectAsset algorithmRespect;

        [SerializeField]
        GameActionManagerAsset gameAction;

        [SerializeField]
        GameRoundManagerAsset gameRoundManager;

        [SerializeField]
        Controls.CameraRigWrapper cameraRig;

        bool isAlgorithmRespectActive = false;

        const float E = 2.71828f;

        readonly float[] rotationByDirection = { 0, 90, 180, 270 };

        Vector2Int currentLabyrinthPosition;

        List<Tile> algorithmSteps = new List<Tile>();

        List<Tile> playerSteps = new List<Tile>();

        //List<Tile> algorithmRespect.WrongColorTilesWhenDiverging = new List<Tile>();

        Stack<GameAction> stack = new Stack<GameAction>();

        Quaternion rotationAtDivergence;

        void Start()
        {
            Client.Instance.clientStateChangedEvent += OnGameStateChanged;
            controls.PlayerPaintTile.OnValueChangedHandler += OnPlayerPaintTile;
            controls.OnLabyrinthPositionChangedHandler += OnLabyrinthPositionChanged;
            algorithmRespect.OnReturnToDivergencePointRequestHandler += OnReturnToDivergencePointRequest;
        }


        public void OnReturnToDivergencePointRequest()
        {
            algorithmRespect.ErrorCount += 1;

            Tile[] tiles = algorithmRespect.WrongColorTilesWhenDiverging.ToArray();

            Vector2Int lpos =
                new Vector2Int(
                    playerSteps[playerSteps.Count - 1].x,
                    playerSteps[playerSteps.Count - 1].y);

            Vector3 position = Client.Instance.Labyrinth.GetLabyrinthPositionInWorldPosition(
                lpos.x,
                lpos.y) +
                new Vector3(0, cameraRig.Transform.position.y, 0);

            controls.PositionRotationAndTiles.Value =
                new PositionRotationAndTile
                {
                    Position = position,
                    Rotation = rotationAtDivergence,
                    Tiles = tiles
                };

            algorithmRespect.Respect =
                RespectValueComputation(
                    (new Vector2Int(
                        playerSteps[playerSteps.Count - 1].x,
                        playerSteps[playerSteps.Count - 1].y) - lpos).magnitude + algorithmRespect.WrongColorTilesWhenDiverging.Count);

            gameAction.SetAction(
                GameAction.ReturnToDivergencePoint,
                lpos,
                rotationAtDivergence,
                tiles,
                playerSteps.ToArray());
        }


        void OnLabyrinthPositionChanged()
        {
            if (isAlgorithmRespectActive)
            {
                EvaluateAlgorithmRespectOnPositionChanged(
                    Client.Instance.Labyrinth.GetWorldPositionInLabyrinthPosition(cameraRig.AvatarTransform.position.x, cameraRig.Transform.position.z),
                    Client.Instance.Labyrinth.GetTileColor(currentLabyrinthPosition),
                    cameraRig.Transform.rotation);
            }
        }
      
        void OnGameStateChanged()
        {
            if (Client.Instance.State == ClientGameState.PlayingTutorial || Client.Instance.State == ClientGameState.Playing)
            {
                ResetAlgorithmRespect();

                if (Client.Instance.State == ClientGameState.PlayingTutorial)
                {
                    SetAlgorithmSteps();
                }
                else
                {
                    SetAlgorithmSteps();

                    if (Client.Instance.ActionSteps.Length > 0)
                    {
                        StartWithSteps();
                    }
                }

                isAlgorithmRespectActive = true;
            }
            else if (Client.Instance.State == ClientGameState.WaitingForNextRound)
            {
                isAlgorithmRespectActive = false;
            }
        }

        void OnPlayerPaintTile(Tile tile)
        {
            EvaluateAlgorithmRespectOnPaintTile(
                Client.Instance.Labyrinth.GetWorldPositionInLabyrinthPosition(
                    cameraRig.Transform.position.x,
                    cameraRig.Transform.position.z),
                tile.Position.x,
                tile.Position.y,
                tile.Color,
                tile.PreviousColor);
        }

        void EvaluateAlgorithmRespectOnPositionChanged(Vector2Int labyrinthPosition, TileColor previousTileColor, Quaternion rotation)
        {
            if (labyrinthPosition != currentLabyrinthPosition)
            {
                if (labyrinthPosition == Client.Instance.Labyrinth.GetLabyrithEndPosition() && !(algorithmRespect.Respect < 1.0f))
                {
                    gameAction.SetAction(GameAction.CompletedRound);

                    if (Client.Instance.State == ClientGameState.Playing)
                    {
                        gameRoundManager.IsRoundCompleted.Value = true;
                    }

                    if (controls.OnPlayerReachedTheEndHandler != null)
                        controls.OnPlayerReachedTheEndHandler.Invoke();

                    return;
                }
                else if (!algorithmRespect.IsDiverging.Value)
                {
                    if (labyrinthPosition.x != algorithmSteps[playerSteps.Count].x
                        || labyrinthPosition.y != algorithmSteps[playerSteps.Count].y
                        || (previousTileColor != algorithmSteps[playerSteps.Count - 1].Color))
                    {
                        algorithmRespect.IsDiverging.Value = true;
                        algorithmRespect.Respect = RespectValueComputation((new Vector2Int(
                            playerSteps[playerSteps.Count - 1].x,
                            playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + algorithmRespect.WrongColorTilesWhenDiverging.Count);

                        // TODO: this is strange, why do we not add a tile here
                        // since we are diverging..??
                        algorithmRespect.WrongTile.Value =
                            new Tile(
                                 playerSteps[playerSteps.Count - 1].x,
                                 playerSteps[playerSteps.Count - 1].y,
                                 playerSteps[playerSteps.Count - 1].PreviousColor);

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
                        if (algorithmRespect.WrongColorTilesWhenDiverging.Count == 0)
                        {
                            algorithmRespect.IsDiverging.Value = false;
                            algorithmRespect.Respect = 1.0f;
                        }
                        else
                        {
                            algorithmRespect.Respect = RespectValueComputation((
                                new Vector2Int(
                                    playerSteps[playerSteps.Count - 1].x,
                                    playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + algorithmRespect.WrongColorTilesWhenDiverging.Count);
                        }
                    }
                    else
                    {
                        algorithmRespect.Respect = RespectValueComputation((
                            new Vector2Int(
                                playerSteps[playerSteps.Count - 1].x,
                                playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + algorithmRespect.WrongColorTilesWhenDiverging.Count);
                    }
                }

                currentLabyrinthPosition = labyrinthPosition;
            }
        }

        void EvaluateAlgorithmRespectOnPaintTile(Vector2Int labyrinthPosition, int x, int y, TileColor color, TileColor previousColor)
        {
            if (algorithmRespect.IsDiverging.Value)
            {
                if (x != playerSteps[playerSteps.Count - 1].x
                    || y != playerSteps[playerSteps.Count - 1].y)
                {
                    for (int i = 0; i < algorithmRespect.WrongColorTilesWhenDiverging.Count; i++)
                    {
                        if (x == algorithmRespect.WrongColorTilesWhenDiverging[i].x
                            && y == algorithmRespect.WrongColorTilesWhenDiverging[i].y)
                        {
                            if (color == algorithmRespect.WrongColorTilesWhenDiverging[i].Color)
                            {
                                algorithmRespect.WrongColorTilesWhenDiverging.RemoveAt(i);

                                algorithmRespect.Respect =
                                    RespectValueComputation(
                                        (new Vector2Int(
                                            playerSteps[playerSteps.Count - 1].x,
                                            playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + algorithmRespect.WrongColorTilesWhenDiverging.Count);
                            }

                            return;
                        }
                    }
                    
                    algorithmRespect.WrongColorTilesWhenDiverging.Add(new Tile(x, y, previousColor));

                    algorithmRespect.Respect =
                        RespectValueComputation(
                            (new Vector2Int(
                                playerSteps[playerSteps.Count - 1].x,
                                playerSteps[playerSteps.Count - 1].y) - labyrinthPosition).magnitude + algorithmRespect.WrongColorTilesWhenDiverging.Count);
                }
            }
        }

        void SetAlgorithmSteps()
        {
            algorithmSteps =
                Client.Instance.Algorithm.GetAlgorithmSteps(
                    Client.Instance.LabyrinthData);
        }

        float RespectValueComputation(float x)
        {
            return Mathf.Pow(E, -x / Labyrinths.Utils.TILE_SIZE);
        }

        void ResetAlgorithmRespect()
        {
            playerSteps.Clear();
            algorithmRespect.WrongColorTilesWhenDiverging.Clear();
            algorithmRespect.ErrorCount = 0;
            algorithmRespect.IsDiverging.Value = false;
            algorithmRespect.Respect = 1.0f;
            currentLabyrinthPosition = Client.Instance.Labyrinth.GetLabyrithStartPosition();
            playerSteps.Add(new Tile(currentLabyrinthPosition.x, currentLabyrinthPosition.y, Client.Instance.Labyrinth.GetTileColor(currentLabyrinthPosition)));
        }

        public void OnReturnToDivergencePointAnswer(bool doreturn)
        {
            if (doreturn)
            {
                algorithmRespect.ErrorCount += 1;

                Tile[] tiles = algorithmRespect.WrongColorTilesWhenDiverging.ToArray();

                Vector2Int lpos =
                    new Vector2Int(
                        playerSteps[playerSteps.Count - 1].x,
                        playerSteps[playerSteps.Count - 1].y);

                Vector3 position = Client.Instance.Labyrinth.GetLabyrinthPositionInWorldPosition(
                    lpos.x,
                    lpos.y) +
                    new Vector3(
                        0,
                        cameraRig.Transform.position.y,
                        0);

                controls.PositionRotationAndTiles.Value =
                    new PositionRotationAndTile
                    {
                        Position = position,
                        Rotation = rotationAtDivergence,
                        Tiles = tiles
                    };

                algorithmRespect.Respect =
                    RespectValueComputation(
                        (new Vector2Int(
                            playerSteps[playerSteps.Count - 1].x,
                            playerSteps[playerSteps.Count - 1].y) - lpos).magnitude + algorithmRespect.WrongColorTilesWhenDiverging.Count);

                gameAction.SetAction(
                    GameAction.ReturnToDivergencePoint,
                    lpos,
                    rotationAtDivergence,
                    tiles,
                    playerSteps.ToArray());
            }
        }

        void StartWithSteps()
        {
            int[] steps = Client.Instance.ActionSteps;
            int forwardDirection = Client.Instance.Labyrinth.GetStartDirection();
            TileColor[,] tiles = new TileColor[Client.Instance.Labyrinth.GetLabyrithXLenght(), Client.Instance.Labyrinth.GetLabyrithYLenght()];
            Vector2Int position = Client.Instance.Labyrinth.GetLabyrithStartPosition();

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
                    algorithmRespect.ErrorCount += 1;

                    position = new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y);
                    forwardDirection = GetForwardDirectionWithRotation(rotationAtDivergence);

                    TileColor previousColor;

                    for (int j = algorithmRespect.WrongColorTilesWhenDiverging.Count - 1; j >= 0; j--)
                    {
                        Tile tile = algorithmRespect.WrongColorTilesWhenDiverging[j];

                        previousColor = tiles[tile.x, tile.y];
                        tiles[tile.x, tile.y] = tile.Color;

                        EvaluateAlgorithmRespectOnPaintTile(position, tile.x, tile.y, tile.Color, previousColor);
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

            controls.PositionRotationAndTiles.Value =
                new PositionRotationAndTile
                {
                    Position = Client.Instance.Labyrinth.GetLabyrinthPositionInWorldPosition(position) + new Vector3(0, cameraRig.Transform.position.y, 0),
                    Rotation = rotation,
                    Tiles = tilesToPaint
                };
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
