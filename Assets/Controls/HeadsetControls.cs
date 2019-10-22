using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience
{
    public class HeadsetControls : MonoBehaviour
    {
        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        ScriptableDirective directive;

        [SerializeField]
        ScriptableGameAction gameAction;

        [SerializeField]
        ScriptableClientGameState client;

        [SerializeField]
        ScriptableInteger forwardDirection;

        [SerializeField]
        ScriptableBoolean isConnectedToPair;

        [SerializeField]
        ScriptableBoolean isConnectedToServer;

        [SerializeField]
        ScriptableAction labyrinthPositionChanged;

        [SerializeField]
        ScriptableVector3 playerPosition;

        [SerializeField]
        ScriptableQuaternion playerRotation;

        [SerializeField]
        Labyrinths.ScriptableTile playerPaintTile;

        [SerializeField]
        ScriptablePositionRotationAndTile playerPositionRotationAndTiles;

        [SerializeField]
        Labyrinths.ScriptableTileColor paintingColor;

        [SerializeField]
        Labyrinths.Labyrinth labyrinth;

        [SerializeField]
        private Controls.CameraRigWrapper cameraRig;


        bool isChainingMovement = false;
        bool isMoving = false;
        bool isPrimaryTouchpadHold = false;
        bool isPrimaryIndexTriggerHold = false;
        bool isTurningLeft = false;
        bool isTurningRight = false;

        readonly int[] xByDirection = { 0, 1, 0, -1 };
        readonly int[] yByDirection = { -1, 0, 1, 0 };

        readonly float[] rotationByDirection = { 0, 90, 180, 270 };

        float primaryIndexTriggerHoldTime = 0;
        float lerpValue = 0;
        float moveSpeed = 0;
        float turnSpeed = 0;

        Vector2Int lastLabyrinthPosition;

        Vector3 fromPosition;
        Vector3 targetPosition;
        Vector3 lastPosition;

        Quaternion fromRotation;
        Quaternion targetRotation;
        Quaternion lastRotation;


        private void Start()
        {
            controls.stopAllMovementEvent += OnStopAllMovement;
            controls.resetPositionAndRotation += OnResetPositionAndRotation;
            directive.valueChangedEvent += OnDirective;
            isConnectedToPair.valueChangedEvent += OnConnectOrDisconnect;
            isConnectedToServer.valueChangedEvent += OnConnectOrDisconnect;
            playerPositionRotationAndTiles.valueChangedEvent += OnPlayerPositionRotationAndTiles;
        }

        void Update()
        {
            if (cameraRig.IsPrimaryTouchPadDown)
            {
                isPrimaryTouchpadHold = true;
            }

            if (cameraRig.IsPrimaryTouchPadUp)
            {
                isPrimaryTouchpadHold = false;
            }

            if (controls.IsControlsEnabled && controls.IsPlayerControlsEnabled)
            {
                if (isPrimaryTouchpadHold)
                {
                    isPrimaryTouchpadHold = true;

                    if (!isTurningLeft && !isTurningRight)
                    {
                        if (isMoving)
                        {
                            if (lerpValue >= 0.5f && CheckIfMovementIsValidInDirectionFromPosition(forwardDirection.Value, targetPosition))
                            {
                                isChainingMovement = true;
                            }
                        }
                        else
                        {
                            RequestMovementInDirection(forwardDirection.Value);
                        }
                    }
                }
                else if (cameraRig.IsLeft)
                {
                    if (!isMoving)
                    {
                        if (isTurningLeft)
                        {
                            isChainingMovement = true;
                        }
                        else if (isTurningRight)
                        {
                            if (isChainingMovement)
                            {
                                isChainingMovement = false;
                            }
                            else
                            {
                                Quaternion trajectory = new Quaternion();
                                trajectory.eulerAngles += new Vector3(0, -90, 0);
                                fromRotation = targetRotation;
                                targetRotation = fromRotation * trajectory;
                                forwardDirection.Value = (forwardDirection.Value - 1) < 0 ? 3 : (forwardDirection.Value - 1);
                                gameAction.SetAction(GameAction.TurnLeft);
                                lerpValue = 1 - lerpValue;
                                isTurningLeft = true;
                                isTurningRight = false;
                            }
                        }
                        else
                        {
                            CameraTurnLeft();
                        }
                    }
                }
                else if (cameraRig.IsRight)
                {
                    if (!isMoving)
                    {
                        if (isTurningRight)
                        {
                            isChainingMovement = true;
                        }
                        else if (isTurningLeft)
                        {
                            if (isChainingMovement)
                            {
                                isChainingMovement = false;
                            }
                            else
                            {
                                Quaternion trajectory = new Quaternion();
                                trajectory.eulerAngles += new Vector3(0, 90, 0);
                                fromRotation = targetRotation;
                                targetRotation = fromRotation * trajectory;
                                forwardDirection.Value = (forwardDirection.Value + 1) % 4;
                                gameAction.SetAction(GameAction.TurnRight);
                                lerpValue = 1 - lerpValue;
                                isTurningLeft = false;
                                isTurningRight = true;
                            }
                        }
                        else
                        {
                            CameraTurnRight();
                        }
                    }
                }

                if (cameraRig.IsPrimaryIndexTriggerDown)
                {
                    isPrimaryIndexTriggerHold = true;
                    primaryIndexTriggerHoldTime = 0;
                }

                if (cameraRig.PrimaryIndexTriggerUp)
                {
                    if (paintingColor.Value == TileColor.Yellow)
                    {
                        paintingColor.Value = TileColor.Red;
                    }
                    else if (primaryIndexTriggerHoldTime <= 1)
                    {
                        paintingColor.Value = TileColor.Yellow;
                        PaintCurrentPositionTile(true);
                    }

                    isPrimaryIndexTriggerHold = false;
                }

                if (isPrimaryIndexTriggerHold)
                {
                    primaryIndexTriggerHoldTime += Time.deltaTime;

                    if (primaryIndexTriggerHoldTime >= 1 && paintingColor.Value != TileColor.Grey)
                    {
                        paintingColor.Value = TileColor.Grey;
                        PaintCurrentPositionTile(true);
                    }
                }

                if (isMoving)
                {
                    float xi = ((moveSpeed * moveSpeed) / (-2 * Constants.MOVEMENT_ACCELERATION)) + 1;

                    if (isChainingMovement)
                    {
                        xi += 1;
                    }

                    moveSpeed = xi < lerpValue ? moveSpeed - (Time.deltaTime * Constants.MOVEMENT_ACCELERATION) : moveSpeed + (Time.deltaTime * Constants.MOVEMENT_ACCELERATION);
                    lerpValue += Time.deltaTime * moveSpeed * Constants.MOVEMENT_SPEED;

                    if (lerpValue >= 1)
                    {
                        if (isChainingMovement)
                        {
                            MovementInDirectionAction(forwardDirection.Value);

                            isChainingMovement = false;
                            lerpValue = lerpValue - 1;

                            RequestMovementInDirection(forwardDirection.Value);

                            cameraRig.Transform.position = Vector3.Lerp(fromPosition, targetPosition, lerpValue);
                        }
                        else
                        {
                            MovementInDirectionAction(forwardDirection.Value);

                            cameraRig.Transform.position = targetPosition;
                            moveSpeed = 0;
                            lerpValue = 0;
                            isMoving = false;
                        }

                        if (paintingColor.Value != TileColor.Red)
                        {
                            PaintCurrentPositionTile(true);
                        }
                    }
                    else
                    {
                        cameraRig.Transform.position = Vector3.Lerp(fromPosition, targetPosition, lerpValue);
                    }
                }
                else if (isTurningLeft || isTurningRight)
                {
                    float xi = ((turnSpeed * turnSpeed) / (-2 * Constants.TURNING_ACCELERATION)) + 1;

                    if (isChainingMovement)
                    {
                        xi++;
                    }

                    turnSpeed = xi < lerpValue ? turnSpeed - (Time.deltaTime * Constants.TURNING_ACCELERATION) : turnSpeed + (Time.deltaTime * Constants.TURNING_ACCELERATION);
                    lerpValue += Time.deltaTime * turnSpeed;

                    if (lerpValue >= 1)
                    {
                        if (isChainingMovement)
                        {
                            isChainingMovement = false;
                            lerpValue = lerpValue - 1;

                            if (isTurningLeft)
                            {
                                Quaternion trajectory = new Quaternion();
                                trajectory.eulerAngles += new Vector3(0, -90, 0);
                                fromRotation = targetRotation;
                                targetRotation = targetRotation * trajectory;

                                forwardDirection.Value = (forwardDirection.Value - 1) < 0 ? 3 : (forwardDirection.Value - 1);
                                gameAction.SetAction(GameAction.TurnLeft);
                            }
                            else if (isTurningRight)
                            {
                                Quaternion trajectory = new Quaternion();
                                trajectory.eulerAngles += new Vector3(0, 90, 0);
                                fromRotation = targetRotation;
                                targetRotation = targetRotation * trajectory;

                                forwardDirection.Value = (forwardDirection.Value + 1) % 4;
                                gameAction.SetAction(GameAction.TurnRight);
                            }

                            cameraRig.Transform.rotation = Quaternion.Lerp(fromRotation, targetRotation, lerpValue);
                        }
                        else
                        {
                            cameraRig.Transform.rotation = targetRotation;
                            turnSpeed = 0;
                            lerpValue = 0;
                            isTurningLeft = false;
                            isTurningRight = false;
                        }
                    }
                    else
                    {
                        cameraRig.Transform.rotation = Quaternion.Lerp(fromRotation, targetRotation, lerpValue);
                    }
                }

                Vector2Int labyrinthPosition = labyrinth.GetWorldPositionInLabyrinthPosition(cameraRig.Transform.position.x, cameraRig.Transform.position.z);

                if (labyrinthPosition != lastLabyrinthPosition)
                {
                    if (paintingColor.Value == TileColor.Red)
                    {
                        PaintTile(lastLabyrinthPosition, TileColor.Red, true);
                    }

                    lastLabyrinthPosition = labyrinthPosition;
                    labyrinthPositionChanged.FireAction();
                }
            }
        }

        private void FixedUpdate()
        {
            if (cameraRig.Transform.position != lastPosition)
            {
                playerPosition.Value = cameraRig.Transform.position;
                lastPosition = cameraRig.Transform.position;
            }

            if (cameraRig.Transform.rotation != lastRotation)
            {
                playerRotation.Value = cameraRig.Transform.rotation;
                lastRotation = cameraRig.Transform.rotation;
            }
        }

        void RequestMovementInDirection(int direction)
        {
            if (CheckIfMovementIsValidInDirectionFromPosition(direction, cameraRig.Transform.position))
            {
                fromPosition = cameraRig.Transform.position;
                targetPosition = fromPosition + (new Vector3(xByDirection[direction] * Constants.TILE_SIZE, 0, -yByDirection[direction] * Constants.TILE_SIZE));

                isMoving = true;
            }
        }

        void CameraTurnLeft()
        {
            Quaternion trajectory = new Quaternion();
            trajectory.eulerAngles += new Vector3(0, -90, 0);
            fromRotation = cameraRig.Transform.rotation;
            targetRotation = fromRotation * trajectory;

            isTurningLeft = true;
            forwardDirection.Value = (forwardDirection.Value - 1) < 0 ? 3 : (forwardDirection.Value - 1);
            gameAction.SetAction(GameAction.TurnLeft);
        }

        void CameraTurnRight()
        {
            Quaternion trajectory = new Quaternion();
            trajectory.eulerAngles += new Vector3(0, 90, 0);
            fromRotation = cameraRig.Transform.rotation;
            targetRotation = fromRotation * trajectory;

            isTurningRight = true;
            forwardDirection.Value = (forwardDirection.Value + 1) % 4;
            gameAction.SetAction(GameAction.TurnRight);
        }

        bool CheckIfMovementIsValidInDirectionFromPosition(int direction, Vector3 position)
        {
            Vector2Int labyrinthPosition = labyrinth.GetWorldPositionInLabyrinthPosition(position.x, position.z);

            labyrinthPosition.x += xByDirection[direction];
            labyrinthPosition.y += yByDirection[direction];

            return labyrinth.GetIsTileWalkable(labyrinthPosition);
        }

        void PaintTile(Vector2Int position, TileColor color, bool saveAction)
        {
            TileColor tileColor = labyrinth.GetTileColor(position);

            if (color != tileColor)
            {
                GameObject tile = labyrinth.GetTile(position);
                Algorithms.FloorPainter floorPainter = tile.GetComponentInChildren<Algorithms.FloorPainter>();

                if (floorPainter != null)
                {
                    floorPainter.PaintFloorWithColor(color);
                    playerPaintTile.SetTile(position, color, tileColor);

                    if (saveAction)
                    {
                        if (color == TileColor.Grey)
                        {
                            gameAction.SetAction(GameAction.UnpaintFloor);
                        }
                        else if (color == TileColor.Yellow)
                        {
                            gameAction.SetAction(GameAction.PaintFloorYellow);
                        }
                        else if (color == TileColor.Red)
                        {
                            gameAction.SetAction(GameAction.PaintFloorRed);
                        }
                    }
                }
            }
        }

        void PaintTile(int positionX, int positionY, TileColor color, bool saveAction)
        {
            Vector2Int position = new Vector2Int(positionX, positionY);

            PaintTile(position, color, saveAction);
        }

        void PaintCurrentPositionTile(bool saveAction)
        {
            Vector2Int position = labyrinth.GetWorldPositionInLabyrinthPosition(cameraRig.Transform.position.x, cameraRig.Transform.position.z);

            PaintTile(position, paintingColor.Value, saveAction);
        }

        void MovementInDirectionAction(int direction)
        {
            if (direction == 0)
            {
                gameAction.SetAction(GameAction.MoveUp);
            }
            else if (direction == 1)
            {
                gameAction.SetAction(GameAction.MoveRight);
            }
            else if (direction == 2)
            {
                gameAction.SetAction(GameAction.MoveDown);
            }
            else if (direction == 3)
            {
                gameAction.SetAction(GameAction.MoveLeft);
            }
        }

        void OnConnectOrDisconnect()
        {
            if (isConnectedToPair.Value && isConnectedToServer.Value)
            {
                controls.IsControlsEnabled = true;
            }
            else
            {
                controls.IsControlsEnabled = false;
            }
        }

        void OnDirective()
        {
            if (directive.Value == Directive.MoveForward)
            {
                gameAction.SetAction(GameAction.ReceivedDirectiveMoveForward);
            }
            else if (directive.Value == Directive.Stop)
            {
                gameAction.SetAction(GameAction.ReceivedDirectiveStop);
            }
            else if (directive.Value == Directive.TurnLeft)
            {
                gameAction.SetAction(GameAction.ReceivedDirectiveTurnLeft);
            }
            else if (directive.Value == Directive.TurnRight)
            {
                gameAction.SetAction(GameAction.ReceivedDirectiveTurnRight);
            }
            else if (directive.Value == Directive.UTurn)
            {
                gameAction.SetAction(GameAction.ReceivedDirectiveUturn);
            }
        }

        void OnStopAllMovement()
        {
            isMoving = false;
            isTurningLeft = false;
            isTurningRight = false;
            isChainingMovement = false;
            isPrimaryTouchpadHold = false;
            lerpValue = 0;
            moveSpeed = 0;
            turnSpeed = 0;
        }

        void OnResetPositionAndRotation()
        {
            cameraRig.Transform.position = new Vector3(0, cameraRig.Transform.position.y, 0);

            if (client.Value == ClientGameState.WaitingForNextRound)
            {
                forwardDirection.Value = 0;
            }
            else
            {
                forwardDirection.Value = labyrinth.GetStartDirection();
            }

            lastLabyrinthPosition = labyrinth.GetWorldPositionInLabyrinthPosition(0, 0);

            Quaternion rotation = new Quaternion(0, 0, 0, 0);

            if (forwardDirection.Value == 1)
            {
                rotation.eulerAngles = new Vector3(0, 90, 0);
            }
            else if (forwardDirection.Value == 2)
            {
                rotation.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (forwardDirection.Value == 3)
            {
                rotation.eulerAngles = new Vector3(0, 270, 0);
            }

            cameraRig.Transform.rotation = rotation;

            paintingColor.Value = TileColor.Yellow;
        }


        void OnPlayerPositionRotationAndTiles()
        {
            OnStopAllMovement();

            cameraRig.Transform.position = playerPositionRotationAndTiles.GetPosition();
            cameraRig.Transform.rotation = playerPositionRotationAndTiles.GetRotation();

            SetForwardDirectionWithRotation(playerPositionRotationAndTiles.GetRotation());

            Tile[] tiles = playerPositionRotationAndTiles.GetTiles();

            for (int i = 0; i < tiles.Length; i++)
            {
                PaintTile(tiles[i].x, tiles[i].y, tiles[i].color, false);
            }

            lastLabyrinthPosition = labyrinth.GetWorldPositionInLabyrinthPosition(cameraRig.Transform.position.x, cameraRig.Transform.position.z);
            labyrinthPositionChanged.FireAction();

            paintingColor.Value = TileColor.Yellow;

            PaintCurrentPositionTile(true);
        }

        void SetForwardDirectionWithRotation(Quaternion rotation)
        {
            float y = rotation.eulerAngles.y;
            float epsilon = 1;

            if (y < rotationByDirection[0] + epsilon && y > rotationByDirection[0] - epsilon)
            {
                forwardDirection.Value = 0;
            }
            else if (y < rotationByDirection[1] + epsilon && y > rotationByDirection[1] - epsilon)
            {
                forwardDirection.Value = 1;
            }
            else if (y < rotationByDirection[2] + epsilon && y > rotationByDirection[2] - epsilon)
            {
                forwardDirection.Value = 2;
            }
            else if (y < rotationByDirection[3] + epsilon && y > rotationByDirection[3] - epsilon)
            {
                forwardDirection.Value = 3;
            }
        }
    }
}