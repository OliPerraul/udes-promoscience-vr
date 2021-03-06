﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;

using Cirrus.Extensions;

namespace UdeS.Promoscience.Controls
{
    /// <summary>
    /// Polls and handles input from the headset user 
    /// </summary>
    public class HeadsetControlsManager : MonoBehaviour
    {
        [SerializeField]
        private float angleLookatTurnThreshold = 65;

        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("transitionTime")]
        private float transitionTimeLimit = 2f;

        [SerializeField]
        private float primaryIndexTriggerHeldTimeLimit = 1f;

        [SerializeField]
        private Algorithms.AlgorithmRespectAsset algorithmRespect;

        [SerializeField]
        private GameActionManagerAsset gameAction;

        // TODO replace by network manager asset
        [SerializeField]
        private ScriptableBoolean isConnectedToPair;

        // TODO replace by network manager asset
        [SerializeField]
        private ScriptableBoolean isConnectedToServer;

        [SerializeField]
        private Characters.AvatarCharacter avatar;

        [SerializeField]
        private HeadsetCameraRig cameraRig;

        [SerializeField]
        private HeadsetInputSchemeAsset inputScheme;

        [SerializeField]
        private HeadsetControlsAsset controls;

        [SerializeField]
        private Characters.AvatarCharacter character;

        [SerializeField]
        private Transform AvatarTransform => transform;


        private bool isChainingMovement = false;

        private bool isMoving = false;

        private bool isPrimaryTouchpadHeld = false;

        private bool isPrimaryIndexTriggerHeld = false;

        private bool isTurningLeft = false;

        private bool isTurningRight = false;

        private readonly int[] xByDirection = { 0, 1, 0, -1 };

        private readonly int[] yByDirection = { -1, 0, 1, 0 };

        private readonly float[] rotationByDirection = { 0f, 90f, 180f, 270f };

        [SerializeField]
        private float primaryIndexTriggerHeldTime = 0;

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

        [SerializeField]
        private bool isThirdPersonEnabled = false;

        public Transform Transform => transform;

        [SerializeField]
        private Transform directionTransform;

        public Transform DirectionArrowTransform => directionTransform;

        private void Awake()
        {
            Client.Instance.State.OnValueChangedHandler += OnClientChangedState;

            controls.stopAllMovementEvent += OnStopAllMovement;

            controls.resetPositionAndRotation += OnResetPositionAndRotation;

            isConnectedToPair.valueChangedEvent += OnConnectOrDisconnect;

            isConnectedToServer.valueChangedEvent += OnConnectOrDisconnect;

            controls.PositionRotationAndTiles.OnValueChangedHandler += OnPlayerPositionRotationAndTiles;

            controls.IsCursorLocked.OnValueChangedHandler += OnCursorLocked;
        }

        private Timer transitionTimer;

        public void Start()
        {
            //controls.IsThirdPersonEnabled.Set(false);
            controls.IsControlsEnabled.Set(true);
            controls.IsPlayerControlsEnabled.Set(true);
            controls.IsCursorLocked.Set(true);
            controls.IsThirdPersonEnabled.Set(isThirdPersonEnabled);

            // TODO put in client State event
            controls.IsTransitionCameraEnabled.Set(false);
            transitionTimer = new Timer(transitionTimeLimit, start: false);
            transitionTimer.OnTimeLimitHandler += OnTransitionTimeout;

            StartCoroutine(DelayedStart());
        }

        public IEnumerator DelayedStart()
        {
            yield return new WaitForSeconds(1f);

            Client.Instance.Settings.IsLeftHanded.Set(
                OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote));

            yield return null;
        }


        public void Update()
        {
            if (inputScheme.IsPrimaryTouchPadDown)
            {
                isPrimaryTouchpadHeld = true;
            }

            if (inputScheme.IsPrimaryTouchPadUp)
            {
                isPrimaryTouchpadHeld = false;
            }

            if (controls.IsControlsEnabled.Value && controls.IsPlayerControlsEnabled.Value)
            {
                if (isPrimaryTouchpadHeld)
                {
                    isPrimaryTouchpadHeld = true;

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
                else if (inputScheme.IsLeftPressed)
                {
                    RequestTurnLeft(turnAvatar: true);
                }
                else if (inputScheme.IsRightPressed)
                {
                    RequestTurnRight(turnAvatar: true);
                }

                if (inputScheme.IsPrimaryIndexTriggerDown)
                {
                    primaryIndexTriggerHeldTime = 0;
                    isPrimaryIndexTriggerHeld = false;
                }

                if (inputScheme.IsPrimaryIndexTriggerHeld)
                {
                    primaryIndexTriggerHeldTime += Time.deltaTime;

                    if (!isPrimaryIndexTriggerHeld &&
                        primaryIndexTriggerHeldTime >= primaryIndexTriggerHeldTimeLimit)
                    {
                        primaryIndexTriggerHeldTime = 0;
                        //controls.IsThirdPersonEnabled.Value = !controls.IsThirdPersonEnabled.Value;
                        controls.PaintingColor.Value = (TileColor.Grey);// ((int)controls.PaintingColor.Value + 1).Mod(Utils.NumColors);
                        isPrimaryIndexTriggerHeld = true;
                    }
                }

                if (inputScheme.IsPrimaryIndexTriggerUp && !isPrimaryIndexTriggerHeld)
                {
                    primaryIndexTriggerHeldTime = 0;

                    controls.PaintingColor.Value =
                        controls.PaintingColor.Value == TileColor.Red ?
                        TileColor.Yellow :
                        TileColor.Red;// (TileColor)((int)controls.PaintingColor.Value + 1).Mod(Utils.NumColors);

                    PaintCurrentPositionTile(true);
                    isPrimaryIndexTriggerHeld = false;
                }
                else if (inputScheme.IsPrimaryIndexTriggerUp)
                {
                    primaryIndexTriggerHeldTime = 0;
                    isPrimaryIndexTriggerHeld = false;
                }

                // Back key to toggle third person camera
                if (inputScheme.IsCameraKeyPressed)
                {
                    controls.IsThirdPersonEnabled.Value = !controls.IsThirdPersonEnabled.Value;
                }
            }
        }


        private void FixedUpdate()
        {
            controls.CameraRotation.Value = cameraRig.CameraRotation;

            if (controls.IsControlsEnabled.Value && controls.IsPlayerControlsEnabled.Value)
            {
                // MOVING
                if (isMoving)
                {
                    controls.Animation.Value = Characters.AvatarAnimation.Jumping;

                    float xi = ((moveSpeed * moveSpeed) / (-2 * Utils.MovementAcceleration)) + 1;

                    if (isChainingMovement)
                    {
                        xi += 1;
                    }

                    moveSpeed =
                        xi < lerpValue ?
                            moveSpeed - (Time.deltaTime * Utils.MovementAcceleration) :
                            moveSpeed + (Time.deltaTime * Utils.MovementAcceleration);

                    lerpValue += Time.deltaTime * moveSpeed * Utils.MovementSpeed;

                    if (lerpValue >= 1)
                    {
                        if (isChainingMovement)
                        {
                            MovementInDirectionAction(controls.ForwardDirection.Value);

                            isChainingMovement = false;
                            lerpValue = lerpValue - 1;

                            RequestMoveForward();

                            Transform.position = Vector3.Lerp(fromPosition, targetPosition, lerpValue);
                        }
                        else
                        {
                            MovementInDirectionAction(controls.ForwardDirection.Value);

                            Transform.position = targetPosition;
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
                        Transform.position = Vector3.Lerp(fromPosition, targetPosition, lerpValue);
                    }
                }
                // TURNING
                else if (isTurningLeft || isTurningRight)
                {
                    controls.Animation.Value = Characters.AvatarAnimation.Jumping;

                    float xi = ((turnSpeed * turnSpeed) / (-2 * Utils.TurningAcceleration)) + 1;

                    if (isChainingMovement)
                    {
                        xi++;
                    }

                    turnSpeed = xi < lerpValue ? turnSpeed - (Time.deltaTime * Utils.TurningAcceleration) : turnSpeed + (Time.deltaTime * Utils.TurningAcceleration);
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
                                character.RootTransform.rotation = targetRotation;
                                DirectionArrowTransform.rotation = Quaternion.LookRotation(Utils.GetDirectionVector((Direction)controls.ForwardDirection.Value));
                            }
                            else
                            {
                                DirectionArrowTransform.rotation = targetRotation;
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
                // IDLE
                else
                {
                    controls.Animation.Value = Characters.AvatarAnimation.Idle;
                }

                Vector2Int labyrinthPosition =
                    Client.Instance.Labyrinth.Value == null ?
                        Vector2Int.zero :
                        Client.Instance.Labyrinth.Value.GetWorldPositionInLabyrinthPosition(
                            Transform.position.x,
                            Transform.position.z);

                if (labyrinthPosition != this.labyrinthPosition)
                {
                    if (controls.PaintingColor.Value == TileColor.Red)
                    {
                        PaintTile(this.labyrinthPosition, TileColor.Red, true);
                    }

                    this.lastLabyrinthPosition = this.labyrinthPosition;
                    this.labyrinthPosition = labyrinthPosition;

                    controls.OnLabyrinthPositionChangedHandler?.Invoke();
                }

                if (character.RootTransform.position != lastPosition)
                {
                    controls.PlayerPosition.Value = Transform.position;
                    lastPosition = character.RootTransform.position;
                }

                if (directionTransform.rotation != lastRotation)
                {
                    controls.BroadcastPlayerRotation.Value = directionTransform.rotation;//.CameraRotation;
                    lastRotation = cameraRig.CameraRotation;
                }
            }
        }



        public void OnClientChangedState(ClientGameState state)
        {
            switch(Client.Instance.State.Value)
            {
                case ClientGameState.Playing:
                case ClientGameState.PlayingTutorial:
                    controls.IsCursorLocked.Set(true);
                    controls.IsTransitionCameraEnabled.Set(true);
                    cameraRig.TransitionCameraAnimator?.Play(TransitionCameraAnimation.Transition_In);
                    transitionTimer?.Start();

                    // Paint the first tile
                    PaintTile(
                        Client.Instance.Labyrinth.Value.GetLabyrithStartPosition(),
                        TileColor.Yellow,
                        saveAction:true);

                    break;

                case ClientGameState.Finished:
                    cameraRig?.TransitionCameraAnimator?.Play(TransitionCameraAnimation.Transition_Out);

                    break;

                default:
                    controls.IsTransitionCameraEnabled.Set(false);
                    break;

            }
        }

        public void OnTransitionTimeout()
        {
            controls.IsTransitionCameraEnabled.Set(false);
            controls.IsThirdPersonEnabled.Set(false);
        }


        public void OnCursorLocked(bool cursorLocked)
        {
            Cursor.visible = !cursorLocked;
            Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
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
            //Vector3 currentDirection = Utils.GetDirectionVector((Direction)controls.ForwardDirection.Value);

            //if (Utils.IsSameDirection(
            //        cameraRig.CameraForward,
            //        currentDirection,
            //        angleLookatTurnThreshold))
            {
                RequestMovementInDirection(controls.ForwardDirection.Value);
            }
            // TODO turn where you look
            //else
            //{
            //    if (Utils.AngleDir(currentDirection, cameraRig.CameraForward, Vector3.up) < 0)
            //    {
            //        RequestTurnLeft(turnAvatar: false);
            //    }
            //    else
            //    {
            //        RequestTurnRight(turnAvatar: false);
            //    }
            //}
        }


        public void DoTurn()
        {
            if (isAvatarTurn)
            {
                character.RootTransform.rotation = Quaternion.Lerp(
                    fromRotation,
                    targetRotation,
                    lerpValue);

                DirectionArrowTransform.rotation = Quaternion.Lerp(
                    fromRotation,
                    Quaternion.LookRotation(
                        Utils.GetDirectionVector(
                            (Direction)controls.ForwardDirection.Value)),
                    lerpValue);
            }
            else
            {
                DirectionArrowTransform.rotation = Quaternion.Lerp(
                    fromRotation,
                    targetRotation,
                    lerpValue);
            }
        }

        void RequestMovementInDirection(int direction)
        {
            if (Client.Instance.Labyrinth.Value == null)
                return;

            if (CheckIfMovementIsValidInDirectionFromPosition(direction, Transform.position))
            {
                fromPosition = Transform.position;

                Vector2Int lpos = Utils.GetMoveDestination(
                    labyrinthPosition,
                    (Direction)controls.ForwardDirection.Value);

                Vector3 pos = Client.Instance.Labyrinth.Value.GetLabyrinthPositionInWorldPosition(lpos);

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

            fromRotation = DirectionArrowTransform.rotation;

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

            fromRotation = DirectionArrowTransform.rotation;

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
            if (Client.Instance.Labyrinth.Value == null)
                return false;

            Vector2Int labyrinthPosition = Client.Instance.Labyrinth.Value.GetWorldPositionInLabyrinthPosition(position.x, position.z);

            if (CheckIfMovementIsValidWhileCorrecting(direction, labyrinthPosition))
            {
                labyrinthPosition = Utils.GetMoveDestination(labyrinthPosition, direction);
                return Client.Instance.Labyrinth.Value.GetIsTileWalkable(labyrinthPosition);
            }

            return false;
        }

        void PaintTile(Vector2Int position, TileColor color, bool saveAction)
        {
            TileColor tileColor = Client.Instance.Labyrinth.Value.GetTileColor(position);

            if (color != tileColor)
            {
                GameObject tile = Client.Instance.Labyrinth.Value.GetTile(position);

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
            if (Client.Instance.Labyrinth.Value == null)
                return;

            Vector2Int position = Client.Instance.Labyrinth.Value
                .GetWorldPositionInLabyrinthPosition(
                Transform.position.x,
                Transform.position.z);

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
            isPrimaryTouchpadHeld = false;
            lerpValue = 0;
            moveSpeed = 0;
            turnSpeed = 0;
        }

        void OnResetPositionAndRotation()
        {
            Transform.position = new Vector3(0, Transform.position.y, 0);

            if (Client.Instance.State.Value == ClientGameState.WaitingForNextLevel)
            {
                controls.ForwardDirection.Value = 0;
            }
            else
            {
                controls.ForwardDirection.Value = Client.Instance.Labyrinth.Value.GetStartDirection();
            }

            if (Client.Instance.Labyrinth.Value != null)
                labyrinthPosition = Client.Instance.Labyrinth.Value.GetWorldPositionInLabyrinthPosition(0, 0);

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
            character.RootTransform.rotation = rotation;

            DirectionArrowTransform.rotation = rotation;

            controls.PaintingColor.Value = TileColor.Yellow;
        }


        void OnPlayerPositionRotationAndTiles(PositionRotationAndTile stuff)
        {
            OnStopAllMovement();

            Transform.position = controls.PositionRotationAndTiles.Value.Position;

            character.RootTransform.rotation = controls.PositionRotationAndTiles.Value.Rotation;

            DirectionArrowTransform.rotation = controls.PositionRotationAndTiles.Value.Rotation;

            SetForwardDirectionWithRotation(
                controls.PositionRotationAndTiles.Value.Rotation);

            Tile[] tiles = controls.PositionRotationAndTiles.Value.Tiles;

            for (int i = 0; i < tiles.Length; i++)
            {
                PaintTile(tiles[i].x, tiles[i].y, tiles[i].Color, false);
            }

            labyrinthPosition = Client.Instance.Labyrinth.Value.GetWorldPositionInLabyrinthPosition(Transform.position.x, Transform.position.z);

            controls.OnLabyrinthPositionChangedHandler?.Invoke();

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