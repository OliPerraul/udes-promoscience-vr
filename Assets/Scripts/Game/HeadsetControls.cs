using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsetControls : MonoBehaviour
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableInteger action;

    [SerializeField]
    ScriptableInteger forwardDirection;

    [SerializeField]
    ScriptableVector3 movementTargetPosition;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    Transform centerEyeAnchor;

    [SerializeField]
    GameObject leftController;

    [SerializeField]
    GameObject rightController;


    bool isMoving = false;

    float movementSpeed = 0.5f;

    float clippingRadius = 0.01f;

    Vector3 targetPosition;
    Vector3 trajectoryToTargetPosition;


    private void Start()
    {
        controls.stopAllMovementEvent += StopAllMovement;
        controls.stopAllMovementEvent += ResetPositionAndRotation;
    }

    void Update ()
    {
       if (controls.isControlsEnabled)
        {
            if (isMoving)
            {
                Vector3 movementPosition = cameraTransform.position + (trajectoryToTargetPosition * Time.deltaTime * movementSpeed);

                if ((trajectoryToTargetPosition.x == 0 
                    || movementPosition.x >= targetPosition.x && trajectoryToTargetPosition.x > 0
                    || movementPosition.x <= targetPosition.x && trajectoryToTargetPosition.x < 0)
                   && (trajectoryToTargetPosition.z == 0
                    || movementPosition.z >= targetPosition.z && trajectoryToTargetPosition.z > 0
                    || movementPosition.z <= targetPosition.z && trajectoryToTargetPosition.z < 0))
                {
                    cameraTransform.position = targetPosition;
                    isMoving = false;
                }
                else
                {
                    cameraTransform.position = movementPosition;
                }
            }
            else if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
            {
                RequestMovementInDirection(forwardDirection.value);
            }
            else if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                if (leftController.activeSelf)
                {
                    float rotationY = leftController.transform.rotation.eulerAngles.y;
                    int direction;

                    if (rotationY < 45 && rotationY >= 0 || rotationY <= 360 && rotationY > 315)
                    {
                        direction = 2;
                        RequestMovementInDirection(direction);
                    }
                    else if (rotationY > 45 && rotationY < 135)
                    {
                        direction = 3;
                        RequestMovementInDirection(direction);
                    }
                    else if (rotationY > 135 && rotationY < 225)
                    {
                        direction = 0;
                        RequestMovementInDirection(direction);
                    }
                    else if (rotationY > 225 && rotationY < 315)
                    {
                        direction = 1;
                        RequestMovementInDirection(direction);
                    }
                }
                else if (rightController.activeSelf)
                {
                    float rotationY = rightController.transform.rotation.eulerAngles.y;
                    int direction;

                    if (rotationY < 45 && rotationY >= 0 || rotationY <= 360 && rotationY > 315)
                    {
                        direction =  2;
                        RequestMovementInDirection(direction);
                    }
                    else if (rotationY > 45 && rotationY < 135)
                    {
                        direction = 3;
                        RequestMovementInDirection(direction);
                    }
                    else if (rotationY > 135 && rotationY < 225)
                    {
                        direction = 0;
                        RequestMovementInDirection(direction);
                    }
                    else if (rotationY > 225 && rotationY < 315)
                    {
                        direction = 1;
                        RequestMovementInDirection(direction);
                    }
                }
            }

            if (OVRInput.GetDown(OVRInput.Button.Left))
            {
                CameraTurnLeft();
            }
            else if (OVRInput.GetDown(OVRInput.Button.Right))
            {
                CameraTurnRight();
            }
        }
    }

    void RequestMovementInDirection(int direction)
    {
        if (CheckIfMovementIsValid(direction))
        {
            float tileSize = Constants.tileSize;

            if (direction == 0)
            {
                targetPosition = cameraTransform.position + (new Vector3(0, 0, tileSize));
                action.value = Constants.ACTION_MOVE_UP;
            }
            else if (direction == 1)
            {
                targetPosition = cameraTransform.position + (new Vector3(tileSize, 0, 0));
                action.value = Constants.ACTION_MOVE_RIGHT;
            }
            else if (direction == 2)
            {
                targetPosition = cameraTransform.position + (new Vector3(0, 0, -tileSize));
                action.value = Constants.ACTION_MOVE_DOWN;
            }
            else if (direction == 3)
            {
                targetPosition = cameraTransform.position + (new Vector3(-tileSize, 0, 0));
                action.value = Constants.ACTION_MOVE_LEFT;
            }

            trajectoryToTargetPosition = (targetPosition - cameraTransform.position);

            isMoving = true;

            movementTargetPosition.value = targetPosition;
        }
    }

    void CameraTurnLeft()
    {
        cameraTransform.Rotate(new Vector3(0, -90, 0));
        forwardDirection.value = (forwardDirection.value - 1) < 0 ? 3 : (forwardDirection.value - 1);
    }

    void CameraTurnRight()
    {
        cameraTransform.Rotate(new Vector3(0, 90, 0));
        forwardDirection.value = (forwardDirection.value + 1) % 4;
    }


    bool CheckIfMovementIsValid(int d)
    {
        bool isValid = false;
        int posX = Mathf.RoundToInt((cameraTransform.position.x / 5)) + labyrinth.GetLabyrithStartPosition().x;
        int posY = Mathf.RoundToInt((-cameraTransform.position.z / 5)) + labyrinth.GetLabyrithStartPosition().y;

        if (d == 2)
        {
            if (posY + 1 < labyrinth.GetLabyrithYLenght())
            {
                posY += 1;
            }
        }
        else if (d == 1)
        {
            if (posX + 1 < labyrinth.GetLabyrithXLenght())
            {
                posX += 1;
            }
        }
        else if (d == 0)
        {
            if (posY - 1 > -1)
            {
                posY -= 1;
            }
        }
        else if (d == 3)
        {
            if (posX - 1 > -1)
            {
                posX -= 1;
            }
        }

        TileInformation tInfo = labyrinth.GetLabyrinthTileInfomation(posX, posY);
        isValid = tInfo.isWalkable;

        return isValid;
    }

    void StopAllMovement()
    {
        isMoving = false;
    }

    void ResetPositionAndRotation()
    {
        cameraTransform.position = new Vector3(0, cameraTransform.position.y, 0);
        cameraTransform.rotation = new Quaternion(0, 0, 0, 0);
        forwardDirection.value = 0;
    }
}
