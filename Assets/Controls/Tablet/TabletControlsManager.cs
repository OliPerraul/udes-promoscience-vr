using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

using UdeS.Promoscience.Characters;
using System;

using Cirrus.Extensions;

namespace UdeS.Promoscience.Controls
{
    /// <summary>
    /// Handles controls received from headset user
    /// </summary>
    public class TabletControlsManager : MonoBehaviour
    {
        [SerializeField]
        private DirectiveManagerAsset directiveManager;

        [SerializeField]
        // TODO remove
        HeadsetControlsAsset controls;

        [SerializeField]
        private TabletControlsAsset tabletControls;

        [SerializeField]
        ScriptableBoolean isConnectedToPair;

        [SerializeField]
        ScriptableBoolean isConnectedToServer;

        [SerializeField]
        private Transform directionArrowTransform;

        private Transform DirectionArrowTransform => directionArrowTransform;

        [SerializeField]
        AvatarCharacter avatar;

        bool isMoving = false;
        bool isTurning = false;

        const float fixedTimestep = 0.03f;//Value of TimeManager Fixed Timestep
        const float maxMovementDistance = Labyrinths.Utils.TileSize;
        const float maxRotationAngle = 45;

        float movementLerpValue = 0;
        float rotationLerpValue = 0;

        Vector3 lastPosition;
        Vector3 targetPosition;
        Queue<Vector3> positionQueue = new Queue<Vector3>();

        Quaternion lastRotation;
        Quaternion targetRotation;
        Queue<Quaternion> rotationQueue = new Queue<Quaternion>();

        public void Awake()
        {
            controls.PlayerPosition.OnValueChangedHandler += OnNewPlayerPosition;
            controls.BroadcastPlayerRotation.OnValueChangedHandler += OnNewPlayerRotation;

            controls.PlayerPaintTile.OnValueChangedHandler += OnPlayerPaintTile;
            controls.PlayerTilesToPaint.OnValueChangedHandler += OnPlayerTilesToPaint;
            controls.resetPositionAndRotation += StopAllMovement;
            controls.stopAllMovementEvent += ResetPositionAndRotation;
            isConnectedToPair.valueChangedEvent += OnConnectOrDisconnect;
            isConnectedToServer.valueChangedEvent += OnConnectOrDisconnect;
        }

        void Start()
        {
            controls.IsThirdPersonEnabled.Set(true);
            tabletControls.TabletCameraMode.Set(TabletCameraMode.ThirdPerson);
        }

        private void Update()
        {
            if (controls.IsControlsEnabled.Value && controls.IsPlayerControlsEnabled.Value)
            {
                if (isMoving)
                {
                    movementLerpValue += Time.deltaTime / fixedTimestep;

                    if (movementLerpValue >= 1)
                    {
                        movementLerpValue -= 1;
                        isMoving = false;
                        avatar.RootTransform.position = targetPosition;
                    }
                    else
                    {
                        avatar.RootTransform.position = Vector3.Lerp(lastPosition, targetPosition, movementLerpValue);
                    }
                }

                if (isTurning)
                {
                    rotationLerpValue += Time.deltaTime / fixedTimestep;

                    if (rotationLerpValue >= 1)
                    {
                        rotationLerpValue -= 1;
                        isTurning = false;
                        avatar.CharacterTransform.rotation = targetRotation;
                        DirectionArrowTransform.rotation = avatar.CharacterTransform.rotation;
                        tabletControls.TabletFirstPersonCameraRoation.Value = avatar.CharacterTransform.rotation;
                    }
                    else
                    {
                        avatar.CharacterTransform.rotation = Quaternion.Lerp(lastRotation, targetRotation, rotationLerpValue);
                        DirectionArrowTransform.rotation = avatar.CharacterTransform.rotation;
                        tabletControls.TabletFirstPersonCameraRoation.Value = avatar.CharacterTransform.rotation;


                    }
                }

                if (!isMoving && positionQueue.Count > 0)
                {
                    if ((avatar.RootTransform.position - positionQueue.Peek()).magnitude > maxMovementDistance)
                    {
                        avatar.RootTransform.position = positionQueue.Dequeue();
                    }
                    else
                    {
                        lastPosition = avatar.RootTransform.position;
                        targetPosition = positionQueue.Dequeue();
                        isMoving = true;
                    }
                }
                else if (!isMoving)
                {
                    movementLerpValue = 0;
                }

                if (!isTurning && rotationQueue.Count > 0)
                {
                    if (Quaternion.Angle(avatar.CharacterTransform.rotation, rotationQueue.Peek()) > maxRotationAngle)
                    {
                        avatar.CharacterTransform.rotation = rotationQueue.Dequeue();
                        DirectionArrowTransform.rotation = avatar.CharacterTransform.rotation;
                        tabletControls.TabletFirstPersonCameraRoation.Value = avatar.CharacterTransform.rotation;
                    }
                    else
                    {
                        lastRotation = avatar.CharacterTransform.rotation;
                        DirectionArrowTransform.rotation = avatar.CharacterTransform.rotation;
                        tabletControls.TabletFirstPersonCameraRoation.Value = avatar.CharacterTransform.rotation;

                        targetRotation = rotationQueue.Dequeue();
                        isTurning = true;
                    }
                }
                else if (!isTurning)
                {
                    rotationLerpValue = 0;
                }
            }
        }


        void OnConnectOrDisconnect()
        {
            if (isConnectedToPair.Value && isConnectedToServer.Value)
            {
                controls.IsControlsEnabled.Value = true;
            }
            else
            {
                controls.IsControlsEnabled.Value = false;
            }
        }

        void OnNewPlayerPosition(Vector3 position)
        {
            positionQueue.Enqueue(position);
        }

        void OnNewPlayerRotation(Quaternion rotation)
        {
            rotationQueue.Enqueue(rotation);
        }

        void OnPlayerPaintTile(Tile tile)
        {
            PaintTile(tile.Position, tile.Color);
        }

        void OnPlayerTilesToPaint(Tile[] tiles)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                Tile tile = tiles[i];
                PaintTile(new Vector2Int(tile.x, tile.y), tile.Color);
            }
        }

        void PaintTile(Vector2Int position, TileColor color)
        {
            if (Client.Instance.Labyrinth.Value == null)
                return;

            GameObject tile = Client.Instance.Labyrinth.Value.GetTile(position);

            if (tile != null)
            {
                Algorithms.FloorPainter floorPainter = tile.GetComponentInChildren<Algorithms.FloorPainter>();

                if (floorPainter != null)
                {
                    floorPainter.PaintFloorWithColor(color);
                }
            }
        }

        void StopAllMovement()
        {
            isMoving = false;
            isTurning = false;
            movementLerpValue = 0;
            rotationLerpValue = 0;
        }

        void ResetPositionAndRotation()
        {
            positionQueue.Clear();
            rotationQueue.Clear();

            int direction = 0;

            avatar.RootTransform.position = new Vector3(0, avatar.RootTransform.position.y, 0);

            if (Client.Instance.Labyrinth.Value != null)
            {
                direction = Client.Instance.Labyrinth.Value.GetStartDirection();
            }

            Quaternion rotation = new Quaternion(0, 0, 0, 0);

            if (direction == 1)
            {
                rotation.eulerAngles = new Vector3(0, 90, 0);
            }
            else if (direction == 2)
            {
                rotation.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (direction == 3)
            {
                rotation.eulerAngles = new Vector3(0, 270, 0);
            }

            // TODO encapsulate
            avatar.CharacterTransform.rotation = rotation;
            DirectionArrowTransform.rotation = avatar.CharacterTransform.rotation;
            tabletControls.TabletFirstPersonCameraRoation.Value = avatar.CharacterTransform.rotation;
        }
    }
}