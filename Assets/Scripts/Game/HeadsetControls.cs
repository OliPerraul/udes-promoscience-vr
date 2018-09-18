using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsetControls : MonoBehaviour
{
    bool isMovementEnabled = false;

    bool isMoving = false;

    float movementSpeed = 0.5f;

    float clippingRadius = 0.01f;

    Vector3 targetPosition;
    Vector3 trajectoryToTargetPosition;

    [SerializeField]
    ScriptableInteger forwardDirection;

    [SerializeField]
    ScriptableVector3 action;

    [SerializeField]
    ScriptableLabyrinth labyrinth;

    [SerializeField]
    LabyrinthVisual labyrinthVisual;

    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    Transform centerEyeAnchor;

    [SerializeField]
    GameObject leftController;

    [SerializeField]
    GameObject rightController;


    void Update ()
    {
       if (isMovementEnabled)
        {
            if (isMoving)
            {
                float tileSize = Constants.tileSize;
                Vector3 movementPosition = cameraTransform.position + (trajectoryToTargetPosition * Time.deltaTime * movementSpeed);

                if (movementPosition.x > targetPosition.x - clippingRadius * tileSize
                    && movementPosition.x < targetPosition.x + clippingRadius * tileSize
                    && movementPosition.z > targetPosition.z - clippingRadius * tileSize
                    && movementPosition.z < targetPosition.z + clippingRadius * tileSize)
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
                //could be changed for invisible target with animation wen targeted
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
            else if (OVRInput.GetDown(OVRInput.Button.Left))
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
            }
            else if (direction == 1)
            {
                targetPosition = cameraTransform.position + (new Vector3(tileSize, 0, 0));
            }
            else if (direction == 2)
            {
                targetPosition = cameraTransform.position + (new Vector3(0, 0, -tileSize));
            }
            else if (direction == 3)
            {
                targetPosition = cameraTransform.position + (new Vector3(-tileSize, 0, 0));
            }

            trajectoryToTargetPosition = (targetPosition - cameraTransform.position);

            isMoving = true;

            action.value = targetPosition;
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

        TileInformation tInfo = labyrinthVisual.GetLabyrinthTileInfomation(posX, posY);
        isValid = tInfo.isWalkable;

        return isValid;
    }

    public void SetMovementActive(bool b)
    {
        isMovementEnabled = b;
    }

    public void StopAllMovement()
    {
        isMoving = false;
    }
}
