using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Head look controls : Use the primary touch pad and the direction you are looking at
//Pros: more emersive
//Cons: need to turn ofthen, need to be played while standing or on a computer weeled chair

//Teleportation movement : Teleport movement in front or back direction.
//Pros: Very simple and fast movement
//Cons: Not very immersive, for now somtime confusing 
public class TeleportationMovementHeadLookControls : MonoBehaviour
{
    bool isMovementEnabled = true;

    //0 == 0 degree, 1 == 90 degree, 2 == 180 degree, 3 == 270 degree

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
            if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
            {

                float touchPadY = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y;
                float rotationY = Mathf.Abs(centerEyeAnchor.rotation.eulerAngles.y) % 360;
                int direction;

                if(rotationY < 45 && rotationY >= 0 || rotationY <= 360 && rotationY > 315)
                {
                    direction = touchPadY > 0 ? 0:2;
                    RequestMove(direction);
                }
                else if (rotationY > 45  && rotationY < 135)
                {
                    direction = touchPadY > 0 ? 1 : 3;
                    RequestMove(direction);
                }
                else if (rotationY > 135 && rotationY < 225)
                {
                    direction = touchPadY > 0 ? 2 : 0;
                    RequestMove(direction);
                }
                else if (rotationY > 225 && rotationY < 315)
                {
                    direction = touchPadY > 0 ? 3 : 1;
                    RequestMove(direction);
                }

                
            }
        }
    }

    void RequestMove(int direction)
    {
        float movement = Constants.tileSize;

        if (CheckIfMovementIsValid(direction))
        { 
            if (direction == 0)
            {
                cameraTransform.position += (new Vector3(0, 0, movement));
            }
            else if (direction == 1)
            {
                cameraTransform.position += (new Vector3(movement, 0, 0));
            }
            else if (direction == 2)
            {
                cameraTransform.position += (new Vector3(0, 0, -movement));
            }
            else if (direction == 3)
            {
                cameraTransform.position += (new Vector3(-movement, 0, 0));
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
}
