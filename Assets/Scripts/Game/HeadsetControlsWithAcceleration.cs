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
    GameLabyrinth labyrinth;

    [SerializeField]
    Transform cameraTransform;

    bool isChainingMovement = false;
    bool isMoving = false;
    bool isTurningLeft = false;
    bool isTurningRight = false;

    readonly int[] xByDirection = { 0, 1, 0, -1 };
    readonly int[] yByDirection = { -1, 0, 1, 0 };

    float lerpValue = 0;
    float moveSpeed = 0;
    float turnSpeed = 0;

    Vector3 fromPosition;
    Vector3 targetPosition;

    Quaternion fromRotation;
    Quaternion targetRotation;


    private void Start()
    {
        controls.stopAllMovementEvent += StopAllMovement;
        controls.resetPositionAndRotation += ResetPositionAndRotation;
    }

    void Update ()
    {
       if (controls.isControlsEnabled)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
            {
                if (!isTurningLeft && !isTurningRight)
                {
                    if (isMoving)
                    {
                        if( lerpValue >= 0.5f && CheckIfMovementIsValidInDirectionFromPosition(forwardDirection.value, targetPosition))//Added minimum lerp before chaning is available, need to be tested
                        {
                            isChainingMovement = true;
                        }
                    }
                    else
                    {
                        RequestMovementInDirection(forwardDirection.value);
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
                            forwardDirection.value = (forwardDirection.value - 1) < 0 ? 3 : (forwardDirection.value - 1);
                            action.value = Constants.ACTION_TURN_LEFT;
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
                            forwardDirection.value = (forwardDirection.value + 1) % 4;
                            action.value = Constants.ACTION_TURN_RIGHT;
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
                PaintCurrentPositionTile();
            }


            if (isMoving)
            {
                float xi = ((moveSpeed * moveSpeed)/(2 * -1 * Constants.MOVEMENT_ACCELERATION)) + 1;

                if (isChainingMovement)
                {
                    xi++;
                }

                moveSpeed = xi < lerpValue ? moveSpeed - (Time.deltaTime * Constants.MOVEMENT_ACCELERATION) : moveSpeed + (Time.deltaTime * Constants.MOVEMENT_ACCELERATION);
                lerpValue += Time.deltaTime * moveSpeed;

                if (lerpValue >= 1)
                {
                    if(isChainingMovement)
                    {
                        isChainingMovement = false;
                        lerpValue = lerpValue - 1;

                        RequestMovementInDirection(forwardDirection.value);

                        cameraTransform.position = Vector3.Lerp(fromPosition, targetPosition, lerpValue);
                    }
                    else
                    {
                        cameraTransform.position = targetPosition;
                        moveSpeed = 0;
                        lerpValue = 0;
                        isMoving = false;
                    }
                }
                else
                {
                    cameraTransform.position = Vector3.Lerp(fromPosition, targetPosition, lerpValue);
                }
            }
            else if (isTurningLeft || isTurningRight)
            {
                float xi = ((turnSpeed * turnSpeed) / (2 * -1 * Constants.TURNING_ACCELERATION)) + 1;

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

                            forwardDirection.value = (forwardDirection.value - 1) < 0 ? 3 : (forwardDirection.value - 1);
                            action.value = Constants.ACTION_TURN_LEFT;
                        }
                        else if (isTurningRight)
                        {
                            Quaternion trajectory = new Quaternion();
                            trajectory.eulerAngles += new Vector3(0, 90, 0);
                            fromRotation = targetRotation;
                            targetRotation = targetRotation * trajectory;

                            forwardDirection.value = (forwardDirection.value + 1) % 4;
                            action.value = Constants.ACTION_TURN_RIGHT;
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
            targetPosition = fromPosition + (new Vector3(xByDirection[direction] * Constants.TILE_SIZE, 0, yByDirection[direction] * Constants.TILE_SIZE));

            if (direction == 0)
            {
                action.value = Constants.ACTION_MOVE_UP;
            }
            else if (direction == 1)
            {
                action.value = Constants.ACTION_MOVE_RIGHT;
            }
            else if (direction == 2)
            {
                action.value = Constants.ACTION_MOVE_DOWN;
            }
            else if (direction == 3)
            {
                action.value = Constants.ACTION_MOVE_LEFT;
            }
            
            isMoving = true;
        }
    }

    void CameraTurnLeft()
    {
        Quaternion trajectory = new Quaternion();
        trajectory.eulerAngles += new Vector3(0, -90, 0);
        fromRotation = cameraTransform.rotation;
        targetRotation = fromRotation * trajectory;

        isTurningLeft = true;
        forwardDirection.value = (forwardDirection.value - 1) < 0 ? 3 : (forwardDirection.value - 1);
        action.value = Constants.ACTION_TURN_LEFT;
    }

    void CameraTurnRight()
    {
        Quaternion trajectory = new Quaternion();
        trajectory.eulerAngles += new Vector3(0, 90, 0);
        fromRotation = cameraTransform.rotation;
        targetRotation = fromRotation * trajectory;

        isTurningRight = true;
        forwardDirection.value = (forwardDirection.value + 1) % 4;
        action.value = Constants.ACTION_TURN_RIGHT;
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

        GameObject tile = labyrinth.GetTile(position.x, position.y);
        FloorPainter floorPainter = tile.GetComponentInChildren<FloorPainter>();

        if(floorPainter != null)
        {
            floorPainter.PaintFloor();
            action.value = Constants.ACTION_PAINT_FLOOR;
        }
    }

    void StopAllMovement()
    {
        isMoving = false;
        isTurningLeft = false;
        isTurningRight = false;
        lerpValue = 0;
    }

    void ResetPositionAndRotation()
    {
        cameraTransform.position = new Vector3(0, cameraTransform.position.y, 0);
        forwardDirection.value = labyrinth.GetStartDirection();

        Quaternion rotation = new Quaternion(0, 0, 0, 0);

        if (forwardDirection.value == 1)
        {
            rotation.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (forwardDirection.value == 2)
        {
            rotation.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (forwardDirection.value == 3)
        {
            rotation.eulerAngles = new Vector3(0, 270, 0);
        }

        cameraTransform.rotation = rotation;
    }
}
