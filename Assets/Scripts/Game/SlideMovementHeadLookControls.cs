using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Head look controls : Use the primary touch pad and the direction you are looking at
//Pros: more emersive
//Cons: need to turn ofthen, need to be played while standing or on a computer weeled chair

//Teleportation movement : Teleport movement in front or back direction.
//Pros: Very simple and fast movement
//Cons: Not very immersive, for now somtime confusing 
public class SlideMovementHeadLookControls : MonoBehaviour
{
    bool isMovementEnabled = false;

    bool isMoving = false;

    float movementSpeed = 0.5f;

    float clippingRadius = 0.01f;

    Vector3 targetPosition;
    Vector3 trajectoryToTargetPosition;

    [SerializeField]
    ScriptableVector3 action;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    Transform centerEyeAnchor;


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
                int direction = -1;
                float touchPadY = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y;
                float rotationY = Mathf.Abs(centerEyeAnchor.rotation.eulerAngles.y) % 360;

                if(rotationY < 45 && rotationY >= 0 || rotationY <= 360 && rotationY > 315)
                {
                    direction = touchPadY > 0 ? 0 : 2;
                }
                else if (rotationY > 45  && rotationY < 135)
                {
                    direction = touchPadY > 0 ? 1 : 3;
                }
                else if (rotationY > 135 && rotationY < 225)
                {
                    direction = touchPadY > 0 ? 2 : 0;
                }
                else if (rotationY > 225 && rotationY < 315)
                {
                    direction = touchPadY > 0 ? 3 : 1;
                } 

                if(CheckIfMovementIsValid(direction))
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
        }
    }



    bool CheckIfMovementIsValid(int d)
    {
        int posX = Mathf.RoundToInt((cameraTransform.position.x / 5)) + labyrinth.GetLabyrithStartPosition().x;
        int posY = Mathf.RoundToInt((-cameraTransform.position.z / 5)) + labyrinth.GetLabyrithStartPosition().y;
        Debug.Log("Lab pos x : " + posX + "  Lab pos y : " + posY);


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

        return labyrinth.GetIsTileWalkable(posX, posY);
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
