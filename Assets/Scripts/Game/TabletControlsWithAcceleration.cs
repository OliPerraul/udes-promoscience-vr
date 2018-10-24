using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletControlsWithAcceleration : MonoBehaviour
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

    Queue<int> actionsList = new Queue<int>();

    bool isChainingMovement = false;
    bool isMoving = false;
    bool isTurningLeft = false;
    bool isTurningRight = false;

    readonly int[] xByDirection = { 0, 1, 0, -1 };
    readonly int[] yByDirection = { 1, 0, -1, 0 };

    float lerpValue = 0;
    float moveSpeed = 0;
    float turnSpeed = 0;

    Vector3 fromPosition;
    Vector3 targetPosition;

    Quaternion fromRotation;
    Quaternion targetRotation;

    void Start ()
    {
        controls.stopAllMovementEvent += StopAllMovement;
        controls.resetPositionAndRotation += ResetPositionAndRotation;
        action.valueChangedEvent += OnAction;
	}
	
	
	void Update ()
    {
        if (controls.IsControlsEnabled)
        {
            if (actionsList.Count > 0)
            {
                if (!isTurningLeft && !isTurningRight)
                {
                    if (actionsList.Peek() == Constants.ACTION_MOVE_UP)
                    {
                        if(isMoving)
                        {
                            if(forwardDirection.Value == 0)
                            {
                                isChainingMovement = true;
                                actionsList.Dequeue();
                            }
                        }
                        else
                        {
                            RequestMovementInDirection(0);
                        }


                    }
                    else if (actionsList.Peek() == Constants.ACTION_MOVE_RIGHT)
                    {
                        if (isMoving)
                        {
                            if (forwardDirection.Value == 1)
                            {
                                isChainingMovement = true;
                                actionsList.Dequeue();
                            }
                        }
                        else
                        {
                            RequestMovementInDirection(1);
                        }
                    }
                    else if (actionsList.Peek() == Constants.ACTION_MOVE_DOWN)
                    {
                        if (isMoving)
                        {
                            if (forwardDirection.Value == 2)
                            {
                                isChainingMovement = true;
                                actionsList.Dequeue();
                            }
                        }
                        else
                        {
                            RequestMovementInDirection(2);
                        }
                    }
                    else if (actionsList.Peek() == Constants.ACTION_MOVE_LEFT)
                    {
                        if (isMoving)
                        {
                            if (forwardDirection.Value == 3)
                            {
                                isChainingMovement = true;
                                actionsList.Dequeue();
                            }
                        }
                        else
                        {
                            RequestMovementInDirection(3);
                        }
                    }
                }
                if (!isMoving)
                {
                    if (actionsList.Peek() == Constants.ACTION_TURN_LEFT)
                    {
                        if (isTurningLeft)
                        {
                            isChainingMovement = true;
                        }
                        else if(isTurningRight)
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
                                lerpValue = 1 - lerpValue;
                                isTurningLeft = true;
                                isTurningRight = false;
                            }
                        }
                        else
                        {
                            Quaternion trajectory = new Quaternion();
                            trajectory.eulerAngles += new Vector3(0, -90, 0);
                            fromRotation = cameraTransform.rotation;
                            targetRotation = fromRotation * trajectory;

                            isTurningLeft = true;
                            forwardDirection.Value = (forwardDirection.Value - 1) < 0 ? 3 : (forwardDirection.Value - 1);
                        }

                        actionsList.Dequeue();
                    }
                    else if (actionsList.Peek() == Constants.ACTION_TURN_RIGHT)
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
                                lerpValue = 1 - lerpValue;
                                isTurningLeft = false;
                                isTurningRight = true;
                            }
                        }
                        else
                        {
                            Quaternion trajectory = new Quaternion();
                            trajectory.eulerAngles += new Vector3(0, 90, 0);
                            fromRotation = cameraTransform.rotation;
                            targetRotation = fromRotation * trajectory;

                            isTurningRight = true;
                            forwardDirection.Value = (forwardDirection.Value + 1) % 4;
                        }

                        actionsList.Dequeue();
                    }
                }
            }

            if (isMoving)
            {
                float xi = ((moveSpeed * moveSpeed) / (-2 * Constants.MOVEMENT_ACCELERATION)) + 1;

                if (isChainingMovement)
                {
                    xi++;
                }

                moveSpeed = xi < lerpValue ? moveSpeed - (Time.deltaTime * Constants.MOVEMENT_ACCELERATION) : moveSpeed + (Time.deltaTime * Constants.MOVEMENT_ACCELERATION);
                lerpValue += Time.deltaTime * moveSpeed;

                if (lerpValue >= 1)
                {
                    if (isChainingMovement)
                    {
                        isChainingMovement = false;
                        lerpValue = lerpValue - 1;

                        fromPosition = cameraTransform.position;
                        targetPosition = fromPosition + (new Vector3(xByDirection[forwardDirection.Value] * Constants.TILE_SIZE, 0, yByDirection[forwardDirection.Value] * Constants.TILE_SIZE));

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
                        }
                        else if (isTurningRight)
                        {
                            Quaternion trajectory = new Quaternion();
                            trajectory.eulerAngles += new Vector3(0, 90, 0);
                            fromRotation = targetRotation;
                            targetRotation = targetRotation * trajectory;

                            forwardDirection.Value = (forwardDirection.Value + 1) % 4;
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

    void OnAction()
    {
        if (action.Value == Constants.ACTION_PAINT_FLOOR)
        {
            //PaintCurrentPositionTile();//Moved to algorithm respect for now 
        }
        else
        {
            actionsList.Enqueue(action.Value);
        }
    }

    void RequestMovementInDirection(int direction)
    {
        fromPosition = cameraTransform.position;
        targetPosition = fromPosition + (new Vector3(xByDirection[direction] * Constants.TILE_SIZE, 0, yByDirection[direction] * Constants.TILE_SIZE));

        isMoving = true;
        actionsList.Dequeue();
    }



    void StopAllMovement()
    {
        actionsList.Clear();
        isMoving = false;
        isTurningLeft = false;
        isTurningRight = false;
        lerpValue = 0;
    }

    void ResetPositionAndRotation()
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