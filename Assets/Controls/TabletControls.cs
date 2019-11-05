﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience
{

    public class TabletControls : MonoBehaviour
    {
        //[SerializeField]
        //private ScriptableClientGameState client;

        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        ScriptableBoolean isConnectedToPair;

        [SerializeField]
        ScriptableBoolean isConnectedToServer;

        [SerializeField]
        ScriptableVector3 playerPosition;

        [SerializeField]
        ScriptableQuaternion playerRotation;

        [SerializeField]
        Labyrinths.ScriptableTile playerPaintTile;

        [SerializeField]
        Labyrinths.ScriptableTileArray playerTilesToPaint;

        [SerializeField]
        Transform cameraTransform;

        bool isMoving = false;
        bool isTurning = false;

        const float fixedTimestep = 0.03f;//Value of TimeManager Fixed Timestep
        const float maxMovementDistance = Labyrinths.Utils.TILE_SIZE;
        const float maxRotationAngle = 45;

        float movementLerpValue = 0;
        float rotationLerpValue = 0;

        Vector3 lastPosition;
        Vector3 targetPosition;
        Queue<Vector3> positionQueue = new Queue<Vector3>();

        Quaternion lastRotation;
        Quaternion targetRotation;
        Queue<Quaternion> rotationQueue = new Queue<Quaternion>();


        void Start()
        {
            playerPosition.valueChangedEvent += OnNewPlayerPosition;
            playerRotation.valueChangedEvent += OnNewPlayerRotation;
            playerPaintTile.valueChangedEvent += OnPlayerPaintTile;
            playerTilesToPaint.valueChangedEvent += OnPlayerTilesToPaint;
            controls.resetPositionAndRotation += StopAllMovement;
            controls.stopAllMovementEvent += ResetPositionAndRotation;
            isConnectedToPair.valueChangedEvent += OnConnectOrDisconnect;
            isConnectedToServer.valueChangedEvent += OnConnectOrDisconnect;
        }

        private void Update()
        {
            if (controls.IsControlsEnabled && controls.IsPlayerControlsEnabled)
            {
                if (isMoving)
                {
                    movementLerpValue += Time.deltaTime / fixedTimestep;

                    if (movementLerpValue >= 1)
                    {
                        movementLerpValue -= 1;
                        isMoving = false;
                        cameraTransform.position = targetPosition;
                    }
                    else
                    {
                        cameraTransform.position = Vector3.Lerp(lastPosition, targetPosition, movementLerpValue);
                    }
                }

                if (isTurning)
                {
                    rotationLerpValue += Time.deltaTime / fixedTimestep;

                    if (rotationLerpValue >= 1)
                    {
                        rotationLerpValue -= 1;
                        isTurning = false;
                        cameraTransform.rotation = targetRotation;
                    }
                    else
                    {
                        cameraTransform.rotation = Quaternion.Lerp(lastRotation, targetRotation, rotationLerpValue);
                    }
                }

                if (!isMoving && positionQueue.Count > 0)
                {
                    if ((cameraTransform.position - positionQueue.Peek()).magnitude > maxMovementDistance)
                    {
                        cameraTransform.position = positionQueue.Dequeue();
                    }
                    else
                    {
                        lastPosition = cameraTransform.position;
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
                    if (Quaternion.Angle(cameraTransform.rotation, rotationQueue.Peek()) > maxRotationAngle)
                    {
                        cameraTransform.rotation = rotationQueue.Dequeue();
                    }
                    else
                    {
                        lastRotation = cameraTransform.rotation;
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
                controls.IsControlsEnabled = true;
            }
            else
            {
                controls.IsControlsEnabled = false;
            }
        }

        void OnNewPlayerPosition()
        {
            positionQueue.Enqueue(playerPosition.Value);
        }

        void OnNewPlayerRotation()
        {
            rotationQueue.Enqueue(playerRotation.Value);
        }

        void OnPlayerPaintTile()
        {
            PaintTile(playerPaintTile.TilePosition, playerPaintTile.TileColor);
        }

        void OnPlayerTilesToPaint()
        {
            Tile[] tiles = playerTilesToPaint.Value;

            for (int i = 0; i < tiles.Length; i++)
            {
                Tile tile = tiles[i];
                PaintTile(new Vector2Int(tile.x, tile.y), tile.color);
            }
        }

        void PaintTile(Vector2Int position, TileColor color)
        {
            GameObject tile = Client.Instance.Labyrinth.GetTile(position);
            Algorithms.FloorPainter floorPainter = tile.GetComponentInChildren<Algorithms.FloorPainter>();

            if (floorPainter != null)
            {
                floorPainter.PaintFloorWithColor(color);
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

            cameraTransform.position = new Vector3(0, cameraTransform.position.y, 0);

            if (Client.Instance.Labyrinth != null)
            {
                direction = Client.Instance.Labyrinth.GetStartDirection();
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

            cameraTransform.rotation = rotation;
        }
    }
}