using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsetControlsWithAcceleration : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger action;

    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableInteger forwardDirection;

    [SerializeField]
    ScriptableVector3 playerPosition;

    [SerializeField]
    ScriptableQuaternion playerRotation;

    [SerializeField]
    ScriptableTile playerPaintTile;

    [SerializeField]
    ScriptableTileColor paintingColor;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    Transform cameraTransform;

    bool isChainingMovement = false;
    bool isMoving = false;
    bool isPrimaryTouchpadHold = false;
    bool isTurningLeft = false;
    bool isTurningRight = false;

    readonly int[] xByDirection = { 0, 1, 0, -1 };
    readonly int[] yByDirection = { -1, 0, 1, 0 };

    float lerpValue = 0;
    float moveSpeed = 0;
    float turnSpeed = 0;

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
    }

    void Update ()
    {
       if (controls.IsControlsEnabled)
        {
            if(OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
            {
                isPrimaryTouchpadHold = false;
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) || isPrimaryTouchpadHold)
            {
                isPrimaryTouchpadHold = true;

                if (!isTurningLeft && !isTurningRight)
                {
                    if (isMoving)
                    {
                        if( lerpValue >= 0.5f && CheckIfMovementIsValidInDirectionFromPosition(forwardDirection.Value, targetPosition))
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
            else if (OVRInput.GetDown(OVRInput.Button.Left))
            {
                if (!isMoving)
                {
                    if (isTurningLeft)
                    {
                        isChainingMovement = true;
                    }
                    else if (isTurningRight)
                    {
                        if(isChainingMovement)
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
                            action.Value = Constants.ACTION_TURN_LEFT;
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
            else if (OVRInput.GetDown(OVRInput.Button.Right))
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
                            action.Value = Constants.ACTION_TURN_RIGHT;
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
            else if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                int nextTileColorId = ((int)paintingColor.Value + 1) % 3;
                paintingColor.Value = (TileColor) nextTileColorId;
                PaintCurrentPositionTile();
            }


            if (isMoving)
            {
                float xi = ((moveSpeed * moveSpeed)/(-2 * Constants.MOVEMENT_ACCELERATION)) + 1;

                if (isChainingMovement)
                {
                    xi += 1;
                }

                moveSpeed = xi < lerpValue ? moveSpeed - (Time.deltaTime * Constants.MOVEMENT_ACCELERATION) : moveSpeed + (Time.deltaTime * Constants.MOVEMENT_ACCELERATION);
                lerpValue += Time.deltaTime * moveSpeed;

                if (lerpValue >= 1)
                {
                    if(isChainingMovement)
                    {
                        isChainingMovement = false;
                        lerpValue = lerpValue - 1;

                        RequestMovementInDirection(forwardDirection.Value);
                        PaintCurrentPositionTile();

                        cameraTransform.position = Vector3.Lerp(fromPosition, targetPosition, lerpValue);
                    }
                    else
                    {
                        cameraTransform.position = targetPosition;
                        moveSpeed = 0;
                        lerpValue = 0;
                        isMoving = false;
                        PaintCurrentPositionTile();
                    }
                }
                else
                {
                    cameraTransform.position = Vector3.Lerp(fromPosition, targetPosition, lerpValue);
                }
            }
            else if (isTurningLeft || isTurningRight)
            {
                float xi = ((turnSpeed * turnSpeed) / ( -2 * Constants.TURNING_ACCELERATION)) + 1;

                if(isChainingMovement)
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

                        if(isTurningLeft)
                        {
                            Quaternion trajectory = new Quaternion();
                            trajectory.eulerAngles += new Vector3(0, -90, 0);
                            fromRotation = targetRotation;
                            targetRotation = targetRotation * trajectory;

                            forwardDirection.Value = (forwardDirection.Value - 1) < 0 ? 3 : (forwardDirection.Value - 1);
                            action.Value = Constants.ACTION_TURN_LEFT;
                        }
                        else if (isTurningRight)
                        {
                            Quaternion trajectory = new Quaternion();
                            trajectory.eulerAngles += new Vector3(0, 90, 0);
                            fromRotation = targetRotation;
                            targetRotation = targetRotation * trajectory;

                            forwardDirection.Value = (forwardDirection.Value + 1) % 4;
                            action.Value = Constants.ACTION_TURN_RIGHT;
                        }

                        cameraTransform.rotation = Quaternion.Lerp(fromRotation, targetRotation, lerpValue);
                    }
                    else
                    {
                        cameraTransform.rotation = targetRotation;
                        turnSpeed = 0;
                        lerpValue = 0;
                        isTurningLeft = false;
                        isTurningRight = false;
                    }
                }
                else
                {
                    cameraTransform.rotation = Quaternion.Lerp(fromRotation, targetRotation, lerpValue);
                }
            }
        }
    }

    void RequestMovementInDirection(int direction)
    {
        if (CheckIfMovementIsValidInDirectionFromPosition(direction, cameraTransform.position))
        {
            fromPosition = cameraTransform.position;
            targetPosition = fromPosition + (new Vector3(xByDirection[direction] * Constants.TILE_SIZE, 0, -yByDirection[direction] * Constants.TILE_SIZE));

            if (direction == 0)
            {
                action.Value = Constants.ACTION_MOVE_UP;
            }
            else if (direction == 1)
            {
                action.Value = Constants.ACTION_MOVE_RIGHT;
            }
            else if (direction == 2)
            {
                action.Value = Constants.ACTION_MOVE_DOWN;
            }
            else if (direction == 3)
            {
                action.Value = Constants.ACTION_MOVE_LEFT;
            }
            
            isMoving = true;
        }
    }

    private void FixedUpdate()
    {
        if(cameraTransform.position != lastPosition)
        {
            playerPosition.Value = cameraTransform.position;
            lastPosition = cameraTransform.position;
        }

        if (cameraTransform.rotation != lastRotation)
        {
            playerRotation.Value = cameraTransform.rotation;
            lastRotation = cameraTransform.rotation;
        }
    }

    void CameraTurnLeft()
    {
        Quaternion trajectory = new Quaternion();
        trajectory.eulerAngles += new Vector3(0, -90, 0);
        fromRotation = cameraTransform.rotation;
        targetRotation = fromRotation * trajectory;

        isTurningLeft = true;
        forwardDirection.Value = (forwardDirection.Value - 1) < 0 ? 3 : (forwardDirection.Value - 1);
        action.Value = Constants.ACTION_TURN_LEFT;
    }

    void CameraTurnRight()
    {
        Quaternion trajectory = new Quaternion();
        trajectory.eulerAngles += new Vector3(0, 90, 0);
        fromRotation = cameraTransform.rotation;
        targetRotation = fromRotation * trajectory;

        isTurningRight = true;
        forwardDirection.Value = (forwardDirection.Value + 1) % 4;
        action.Value = Constants.ACTION_TURN_RIGHT;
    }

    bool CheckIfMovementIsValidInDirectionFromPosition(int direction, Vector3 position)
    {
        Vector2Int labyrinthPosition = labyrinth.GetWorldPositionInLabyrinthPosition(position.x, position.z);

        labyrinthPosition.x += xByDirection[direction];
        labyrinthPosition.y += yByDirection[direction];

        return labyrinth.GetIsTileWalkable(labyrinthPosition);
    }

    void PaintCurrentPositionTile()
    {
        Vector2Int position = labyrinth.GetWorldPositionInLabyrinthPosition(cameraTransform.position.x, cameraTransform.position.z);

        TileColor tileColor = labyrinth.GetTileColor(position);

        if (paintingColor.Value != tileColor)
        {
            GameObject tile = labyrinth.GetTile(position);
            FloorPainter floorPainter = tile.GetComponentInChildren<FloorPainter>();

            if (floorPainter != null)
            {
                floorPainter.PaintFloorWithColor(paintingColor.Value);
                action.Value = Constants.ACTION_PAINT_FLOOR;
                playerPaintTile.SetTile(position, paintingColor.Value);
            }
        }
    }

    void OnStopAllMovement()
    {
        isMoving = false;
        isTurningLeft = false;
        isTurningRight = false;
        lerpValue = 0;
    }

    void OnResetPositionAndRotation()
    {
        cameraTransform.position = new Vector3(0, cameraTransform.position.y, 0);
        forwardDirection.Value = labyrinth.GetStartDirection();

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

        cameraTransform.rotation = rotation;
    }
}
