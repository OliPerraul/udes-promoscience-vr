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

    bool isMoving = false;
    bool isTurningLeft = false;
    bool isTurningRight = false;

    float lerpValue = 0;
    float moveSpeed = 0;
    float turnSpeed = 0;

    Vector3 targetPosition;
    Vector3 fromPosition;

    Quaternion fromRotation;
    Quaternion targetRotation;
    bool isChainingMovement = false;

    private void Start()
    {
        controls.stopAllMovementEvent += StopAllMovement;
        controls.stopAllMovementEvent += ResetPositionAndRotation;
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
                        if(CheckIfMovementAttargetPositionIsValid(forwardDirection.value))
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
        if (CheckIfMovementIsValid(direction))
        {
            fromPosition = cameraTransform.position;

            if (direction == 0)
            {
                targetPosition = fromPosition + (new Vector3(0, 0, Constants.TILE_SIZE));
                action.value = Constants.ACTION_MOVE_UP;
            }
            else if (direction == 1)
            {
                targetPosition = fromPosition + (new Vector3(Constants.TILE_SIZE, 0, 0));
                action.value = Constants.ACTION_MOVE_RIGHT;
            }
            else if (direction == 2)
            {
                targetPosition = fromPosition + (new Vector3(0, 0, -Constants.TILE_SIZE));
                action.value = Constants.ACTION_MOVE_DOWN;
            }
            else if (direction == 3)
            {
                targetPosition = fromPosition + (new Vector3(-Constants.TILE_SIZE, 0, 0));
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


    bool CheckIfMovementIsValid(int direction)
    {
        int posX = Mathf.RoundToInt((cameraTransform.position.x / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().x;
        int posY = Mathf.RoundToInt((-cameraTransform.position.z / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().y;

        if (direction == 2)
        {
            if (posY + 1 < labyrinth.GetLabyrithYLenght())
            {
                posY += 1;
            }
        }
        else if (direction == 1)
        {
            if (posX + 1 < labyrinth.GetLabyrithXLenght())
            {
                posX += 1;
            }
        }
        else if (direction == 0)
        {
            if (posY - 1 > -1)
            {
                posY -= 1;
            }
        }
        else if (direction == 3)
        {
            if (posX - 1 > -1)
            {
                posX -= 1;
            }
        }

        return labyrinth.GetIsTileWalkable(posX,posY);
    }

    bool CheckIfMovementAttargetPositionIsValid(int direction)
    {
        int posX = Mathf.RoundToInt((targetPosition.x / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().x;
        int posY = Mathf.RoundToInt((-targetPosition.z / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().y;

        if (direction == 2)
        {
            if (posY + 1 < labyrinth.GetLabyrithYLenght())
            {
                posY += 1;
            }
        }
        else if (direction == 1)
        {
            if (posX + 1 < labyrinth.GetLabyrithXLenght())
            {
                posX += 1;
            }
        }
        else if (direction == 0)
        {
            if (posY - 1 > -1)
            {
                posY -= 1;
            }
        }
        else if (direction == 3)
        {
            if (posX - 1 > -1)
            {
                posX -= 1;
            }
        }

        return labyrinth.GetIsTileWalkable(posX, posY);
    }

    void PaintCurrentPositionTile()
    {
        int posX = Mathf.RoundToInt((cameraTransform.position.x / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().x;
        int posY = Mathf.RoundToInt((-cameraTransform.position.z / Constants.TILE_SIZE)) + labyrinth.GetLabyrithStartPosition().y;
        GameObject tile = labyrinth.GetTile(posX, posY);
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
        lerpValue = 0;
    }

    void ResetPositionAndRotation()
    {
        cameraTransform.position = new Vector3(0, cameraTransform.position.y, 0);
        cameraTransform.rotation = new Quaternion(0, 0, 0, 0);
        forwardDirection.value = 0;
    }
}
