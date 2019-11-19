﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;

using Cirrus.Extensions;

namespace UdeS.Promoscience
{
    // FOR REAL WHY IS THIS CHARACTER CONTROLLER SO GODDAM COMPLICATED
    // Our guy is just moving on a grid...

    public class HeadsetAvatarController : MonoBehaviour
    {
        [SerializeField]
        private float angleLookatTurnThreshold = 65;

        [SerializeField]
        private Algorithms.AlgorithmRespectAsset algorithmRespect;

        [SerializeField]
        private GameActionManagerAsset gameAction;

        [SerializeField]
        private AvatarControllerAsset controls;

        // TODO replace by network manager asset
        [SerializeField]
        private ScriptableBoolean isConnectedToPair;

        // TODO replace by network manager asset
        [SerializeField]
        private ScriptableBoolean isConnectedToServer;

        [SerializeField]
        private Controls.CameraRigWrapper cameraRig;

        private bool isChainingMovement = false;

        private bool isMoving = false;

        private bool isPrimaryTouchpadHold = false;

        private bool isPrimaryIndexTriggerHold = false;

        private bool isTurningLeft = false;

        private bool isTurningRight = false;

        private readonly int[] xByDirection = { 0, 1, 0, -1 };

        private readonly int[] yByDirection = { -1, 0, 1, 0 };

        private readonly float[] rotationByDirection = { 0f, 90f, 180f, 270f };

        private float primaryIndexTriggerHoldTime = 0;

        private float lerpValue = 0;

        private float moveSpeed = 0;

        private float turnSpeed = 0;

        private Vector2Int lastLabyrinthPosition;

        private Vector2Int labyrinthPosition;

        private Vector3 fromPosition;

        private Vector3 targetPosition;

        private Vector3 lastPosition;

        private Quaternion fromRotation;

        private Quaternion targetRotation;

        private Quaternion lastRotation;

        private bool isTurningDirection = false;

        private void Awake()
        {
            controls.stopAllMovementEvent += OnStopAllMovement;

            controls.resetPositionAndRotation += OnResetPositionAndRotation;

            isConnectedToPair.valueChangedEvent += OnConnectOrDisconnect;

            isConnectedToServer.valueChangedEvent += OnConnectOrDisconnect;

            controls.PositionRotationAndTiles.OnValueChangedHandler += OnPlayerPositionRotationAndTiles;

            controls.IsMouseFocusGrabbed.OnValueChangedHandler += OnMouseFocusChanged;

            controls.IsThirdPersonEnabled.Value = false;
        }


        public void OnMouseFocusChanged(bool enabled)
        {
            if (enabled)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        public void RequestTurnLeft(bool turnAvatar)
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

                        controls.ForwardDirection.Value = 
                            (controls.ForwardDirection.Value - 1) < 0 ? 
                                3 : 
                                (controls.ForwardDirection.Value - 1);

                        gameAction.SetAction(GameAction.TurnLeft);
                        lerpValue = 1 - lerpValue;
                        isTurningLeft = true;
                        isTurningRight = false;
                    }
                }
                else
                {
                    TurnLeft(turnAvatar);
                }
            }
        }

        public void RequestTurnRight(bool turnAvatar)
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
                        controls.ForwardDirection.Value = (controls.ForwardDirection.Value + 1) % 4;
                        gameAction.SetAction(GameAction.TurnRight);
                        lerpValue = 1 - lerpValue;
                        isTurningLeft = false;
                        isTurningRight = true;
                    }
                }
                else
                {
                    TurnRight(turnAvatar);
                }
            }
        }

        public void RequestMoveForward()
        {
            Vector3 currentDirection = Utils.GetDirectionVector((Direction)controls.ForwardDirection.Value);

            if (Utils.IsSameDirection(
                    cameraRig.AvatarDirection,
                    currentDirection,
                    angleLookatTurnThreshold))
            {
                RequestMovementInDirection(controls.ForwardDirection.Value);
            }
            else
            {
                if (Utils.AngleDir(currentDirection, cameraRig.AvatarDirection, Vector3.up) < 0)
                {
                    RequestTurnLeft(turnAvatar: false);
                }
                else
                {
                    RequestTurnRight(turnAvatar: false);
                }
            }
        }

        public void Update()
        {
            if (cameraRig.IsPrimaryTouchPadDown)
            {
                isPrimaryTouchpadHold = true;
            }

            if (cameraRig.IsPrimaryTouchPadUp)
            {
                isPrimaryTouchpadHold = false;
            }

            if (controls.IsControlsEnabled.Value && controls.IsPlayerControlsEnabled.Value)
            {
                if (isPrimaryTouchpadHold)
                {
                    isPrimaryTouchpadHold = true;

                    if (!isTurningLeft && !isTurningRight)
                    {
                        if (isMoving)
                        {
                            if (lerpValue >= 0.5f && CheckIfMovementIsValidInDirectionFromPosition(controls.ForwardDirection.Value, targetPosition))
                            {
                                isChainingMovement = true;
                            }
                        }
                        else
                        {
                            RequestMoveForward();
                        }
                    }
                }
                else if (cameraRig.IsLeftPressed)
                {
                    RequestTurnLeft(turnAvatar: true);
                }
                else if (cameraRig.IsRightPressed)
                {
                    RequestTurnRight(turnAvatar: true);
                }

                if (cameraRig.IsPrimaryIndexTriggerDown)
                {
                    isPrimaryIndexTriggerHold = true;
                    primaryIndexTriggerHoldTime = 0;
                }

                if (isPrimaryIndexTriggerHold)
                {
                    primaryIndexTriggerHoldTime += Time.deltaTime;

                    if (primaryIndexTriggerHoldTime >= 1)
                    {
                        controls.IsThirdPersonEnabled.Value = !controls.IsThirdPersonEnabled.Value;
                        isPrimaryIndexTriggerHold = false;
                    }
                }

                if (cameraRig.IsPrimaryIndexTriggerUp)
                {
                    controls.PaintingColor.Value = (TileColor)((int)controls.PaintingColor.Value + 1).Mod(Utils.NumColors);
                    PaintCurrentPositionTile(true);
                    isPrimaryIndexTriggerHold = false;
                }
            }
        }

        private void FixedUpdate()
        {
            if (controls.IsControlsEnabled.Value && controls.IsPlayerControlsEnabled.Value)
            {
                if (isMoving)
                {
                    float xi = ((moveSpeed * moveSpeed) / (-2 * Utils.MOVEMENT_ACCELERATION)) + 1;

                    if (isChainingMovement)
                    {
                        xi += 1;
                    }

                    moveSpeed =
                        xi < lerpValue ?
                            moveSpeed - (Time.deltaTime * Utils.MOVEMENT_ACCELERATION) :
                            moveSpeed + (Time.deltaTime * Utils.MOVEMENT_ACCELERATION);

                    lerpValue += Time.deltaTime * moveSpeed * Utils.MOVEMENT_SPEED;

                    if (lerpValue >= 1)
                    {
                        if (isChainingMovement)
                        {
                            MovementInDirectionAction(controls.ForwardDirection.Value);

                            isChainingMovement = false;
                            lerpValue = lerpValue - 1;

                            RequestMoveForward();

                            cameraRig.Transform.position = Vector3.Lerp(fromPosition, targetPosition, lerpValue);
                        }
                        else
                        {
                            MovementInDirectionAction(controls.ForwardDirection.Value);

                            cameraRig.Transform.position = targetPosition;
                            moveSpeed = 0;
                            lerpValue = 0;
                            isMoving = false;
                        }

                        if (controls.PaintingColor.Value != TileColor.Red)
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
                    float xi = ((turnSpeed * turnSpeed) / (-2 * Utils.TURNING_ACCELERATION)) + 1;

                    if (isChainingMovement)
                    {
                        xi++;
                    }

                    turnSpeed = xi < lerpValue ? turnSpeed - (Time.deltaTime * Utils.TURNING_ACCELERATION) : turnSpeed + (Time.deltaTime * Utils.TURNING_ACCELERATION);
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

                                controls.ForwardDirection.Value = (controls.ForwardDirection.Value - 1) < 0 ? 3 : (controls.ForwardDirection.Value - 1);
                                gameAction.SetAction(GameAction.TurnLeft);
                            }
                            else if (isTurningRight)
                            {
                                Quaternion trajectory = new Quaternion();
                                trajectory.eulerAngles += new Vector3(0, 90, 0);
                                fromRotation = targetRotation;
                                targetRotation = targetRotation * trajectory;

                                controls.ForwardDirection.Value = (controls.ForwardDirection.Value + 1) % 4;
                                gameAction.SetAction(GameAction.TurnRight);
                            }

                            DoTurn();
                        }
                        else
                        {
                            // TODO Fix this crap
                            if (isAvatarTurn)
                            {
                                cameraRig.AvatarTransform.rotation = targetRotation;
                                cameraRig.DirectionTransform.rotation = Quaternion.LookRotation(Utils.GetDirectionVector((Direction)controls.ForwardDirection.Value));
                            }
                            else
                            {
                                cameraRig.DirectionTransform.rotation = targetRotation;
                            }


                            turnSpeed = 0;
                            lerpValue = 0;
                            isTurningLeft = false;
                            isTurningRight = false;
                        }

                    }
                    else
                    {
                        DoTurn();
                    }
                }

                Vector2Int labyrinthPosition =
                    Client.Instance.Labyrinth.GetWorldPositionInLabyrinthPosition(
                        cameraRig.Transform.position.x,
                        cameraRig.Transform.position.z);

                if (labyrinthPosition != this.labyrinthPosition)
                {
                    if (controls.PaintingColor.Value == TileColor.Red)
                    {
                        PaintTile(this.labyrinthPosition, TileColor.Red, true);
                    }

                    this.lastLabyrinthPosition = this.labyrinthPosition;
                    this.labyrinthPosition = labyrinthPosition;

                    // TODO: encapsulate
                    if (controls.OnLabyrinthPositionChangedHandler != null)
                        controls.OnLabyrinthPositionChangedHandler.Invoke();
                }

                if (cameraRig.AvatarTransform.position != lastPosition)
                {
                    controls.PlayerPosition.Value = cameraRig.Transform.position;
                    lastPosition = cameraRig.AvatarTransform.position;
                }

                if (cameraRig.AvatarTransform.rotation != lastRotation)
                {
                    controls.PlayerRotation.Value = cameraRig.AvatarRotation;
                    lastRotation = cameraRig.AvatarRotation;
                }
            }
        }

        public void DoTurn()
        {
            if (isAvatarTurn)
            {
                cameraRig.AvatarTransform.rotation = Quaternion.Lerp(
                    fromRotation,
                    targetRotation,
                    lerpValue);

                cameraRig.DirectionTransform.rotation = Quaternion.Lerp(
                    fromRotation,
                    Quaternion.LookRotation(
                        Utils.GetDirectionVector(
                            (Direction)controls.ForwardDirection.Value)),
                    lerpValue);
            }
            else
            {
                cameraRig.DirectionTransform.rotation = Quaternion.Lerp(
                    fromRotation,
                    targetRotation,
                    lerpValue);
            }
        }

        void RequestMovementInDirection(int direction)
        {
            if (CheckIfMovementIsValidInDirectionFromPosition(direction, cameraRig.Transform.position))
            {
                fromPosition = cameraRig.Transform.position;

                Vector2Int lpos = Utils.GetMoveDestination(
                    labyrinthPosition,
                    (Direction)controls.ForwardDirection.Value);

                Vector3 pos = Client.Instance.Labyrinth.GetLabyrinthPositionInWorldPosition(lpos);

                targetPosition = new Vector3(pos.x, fromPosition.y, pos.z);

                isMoving = true;
            }
        }

        private bool isAvatarTurn = false;

        void TurnLeft(bool turnCamera)
        {
            isAvatarTurn = turnCamera;

            Quaternion trajectory = new Quaternion();

            trajectory.eulerAngles += new Vector3(0, -90, 0);

            fromRotation = isAvatarTurn ? 
                cameraRig.AvatarRotation : 
                cameraRig.DirectionTransform.rotation;

            targetRotation = fromRotation * trajectory;

            isTurningLeft = true;

            controls.ForwardDirection.Value = (controls.ForwardDirection.Value - 1) < 0 ? 3 : (controls.ForwardDirection.Value - 1);

            gameAction.SetAction(GameAction.TurnLeft);
        }

        void TurnRight(bool turnCamera)
        {
            isAvatarTurn = turnCamera;

            Quaternion trajectory = new Quaternion();

            trajectory.eulerAngles += new Vector3(0, 90, 0);

            fromRotation = isAvatarTurn ? 
                cameraRig.AvatarRotation : 
                cameraRig.DirectionTransform.rotation;

            targetRotation = fromRotation * trajectory;

            isTurningRight = true;

            controls.ForwardDirection.Value = (controls.ForwardDirection.Value + 1) % 4;

            gameAction.SetAction(GameAction.TurnRight);
        }

        bool CheckIfMovementIsValidWhileCorrecting(int direction, Vector2Int position)
        {
            // If correcting is enabled, only allow to move to correct mystakes
            if (algorithmRespect.IsCorrectingEnabled.Value)
            {
                if (algorithmRespect.IsDiverging.Value)
                {
                    if (algorithmRespect.WrongColorTilesWhenDiverging.Count == 0)
                    {
                        if (algorithmRespect.WrongTile.Value.Position == Utils.GetMoveDestination(position, direction) &&
                            algorithmRespect.WrongTile.Value.Color == controls.PaintingColor.Value)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        var returnPosition = algorithmRespect.WrongColorTilesWhenDiverging.Count < 2 ?
                            lastLabyrinthPosition :
                            algorithmRespect.WrongColorTilesWhenDiverging[algorithmRespect.WrongColorTilesWhenDiverging.Count - 2].Position;

                        if (returnPosition == Utils.GetMoveDestination(position, direction) &&
                            algorithmRespect.WrongColorTilesWhenDiverging[algorithmRespect.WrongColorTilesWhenDiverging.Count - 1].Color ==
                            controls.PaintingColor.Value)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        bool CheckIfMovementIsValidInDirectionFromPosition(int direction, Vector3 position)
        {
            Vector2Int labyrinthPosition = Client.Instance.Labyrinth.GetWorldPositionInLabyrinthPosition(position.x, position.z);

            if (CheckIfMovementIsValidWhileCorrecting(direction, labyrinthPosition))
            {
                labyrinthPosition = Utils.GetMoveDestination(labyrinthPosition, direction);
                return Client.Instance.Labyrinth.GetIsTileWalkable(labyrinthPosition);
            }

            return false;
        }

        void PaintTile(Vector2Int position, TileColor color, bool saveAction)
        {
            TileColor tileColor = Client.Instance.Labyrinth.GetTileColor(position);

            if (color != tileColor)
            {
                GameObject tile = Client.Instance.Labyrinth.GetTile(position);

                if (tile)
                {
                    Algorithms.FloorPainter floorPainter = tile.GetComponentInChildren<Algorithms.FloorPainter>();

                    if (floorPainter != null)
                    {
                        floorPainter.PaintFloorWithColor(color);

                        var prvtile = controls.PlayerPaintTile.Value;

                        // TODO fix this crap
                        controls.PlayerPaintTile.Value = new Tile
                        {
                            Position = position,
                            color = color,
                            previousColor = tileColor
                        };

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
        }

        void PaintTile(int positionX, int positionY, TileColor color, bool saveAction)
        {
            Vector2Int position = new Vector2Int(positionX, positionY);
            PaintTile(position, color, saveAction);
        }

        void PaintCurrentPositionTile(bool saveAction)
        {
            Vector2Int position = Client.Instance.Labyrinth
                .GetWorldPositionInLabyrinthPosition(
                cameraRig.Transform.position.x,
                cameraRig.Transform.position.z);

            PaintTile(
                position,
                controls.PaintingColor.Value,
                saveAction);
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
            controls.IsControlsEnabled.Value = isConnectedToPair.Value && isConnectedToServer.Value;
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

            if (Client.Instance.State == ClientGameState.WaitingForNextRound)
            {
                controls.ForwardDirection.Value = 0;
            }
            else
            {
                controls.ForwardDirection.Value = Client.Instance.Labyrinth.GetStartDirection();
            }

            if (Client.Instance.Labyrinth != null)
                labyrinthPosition = Client.Instance.Labyrinth.GetWorldPositionInLabyrinthPosition(0, 0);

            Quaternion rotation = new Quaternion(0, 0, 0, 0);

            if (controls.ForwardDirection.Value == 1)
            {
                rotation.eulerAngles = new Vector3(0, 90, 0);
            }
            else if (controls.ForwardDirection.Value == 2)
            {
                rotation.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (controls.ForwardDirection.Value == 3)
            {
                rotation.eulerAngles = new Vector3(0, 270, 0);
            }

            // TODO put somewhere else
            cameraRig.AvatarTransform.rotation = rotation;

            cameraRig.DirectionTransform.rotation = rotation;

            controls.PaintingColor.Value = TileColor.Yellow;
        }


        void OnPlayerPositionRotationAndTiles(PositionRotationAndTile stuff)
        {
            OnStopAllMovement();

            cameraRig.Transform.position = controls.PositionRotationAndTiles.Value.Position;

            cameraRig.AvatarTransform.rotation = controls.PositionRotationAndTiles.Value.Rotation;

            cameraRig.DirectionTransform.rotation = controls.PositionRotationAndTiles.Value.Rotation;

            SetForwardDirectionWithRotation(
                controls.PositionRotationAndTiles.Value.Rotation);

            Tile[] tiles = controls.PositionRotationAndTiles.Value.Tiles;

            for (int i = 0; i < tiles.Length; i++)
            {
                PaintTile(tiles[i].x, tiles[i].y, tiles[i].Color, false);
            }

            labyrinthPosition = Client.Instance.Labyrinth.GetWorldPositionInLabyrinthPosition(cameraRig.Transform.position.x, cameraRig.Transform.position.z);

            if (controls.OnLabyrinthPositionChangedHandler != null)
                controls.OnLabyrinthPositionChangedHandler.Invoke();

            controls.PaintingColor.Value = TileColor.Yellow;

            PaintCurrentPositionTile(true);
        }

        void SetForwardDirectionWithRotation(Quaternion rotation)
        {
            float y = rotation.eulerAngles.y;
            float epsilon = 1;

            if (y < rotationByDirection[0] + epsilon && y > rotationByDirection[0] - epsilon)
            {
                controls.ForwardDirection.Value = 0;
            }
            else if (y < rotationByDirection[1] + epsilon && y > rotationByDirection[1] - epsilon)
            {
                controls.ForwardDirection.Value = 1;
            }
            else if (y < rotationByDirection[2] + epsilon && y > rotationByDirection[2] - epsilon)
            {
                controls.ForwardDirection.Value = 2;
            }
            else if (y < rotationByDirection[3] + epsilon && y > rotationByDirection[3] - epsilon)
            {
                controls.ForwardDirection.Value = 3;
            }
        }
    }
}