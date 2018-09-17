using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Arrows controls: Use the primary touch par as 4 directions arrows
//Pros: Very simple and fast control
//Cons: Not very immersive controls and ambiguity caused by the touch pad keeping the last area touched for a short laps and cause you to do what you wanted to do

//Teleportation movement : Teleport movement in front or back direction. Right and left turn the camera in this case
//Pros: Very simple and fast movement
//Cons: Not very immersive, and the combinaison movement/control causes weard feeling when looking on the side and moving in the front direction
public class TeleportationMovementArrowsControls : MonoBehaviour
{
    bool isMovementEnabled = true;

    public int direction = 0;//0 == 0 degree, 1 == 90 degree, 2 == 180 degree, 3 == 270 degree

    [SerializeField]
    ScriptableLabyrinth labyrinth;

    [SerializeField]
    LabyrinthVisual labyrinthVisual;

    [SerializeField]
    Transform cameraTransform;

	void Update ()
    {
        if (isMovementEnabled)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
            {

                float x = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x;
                float y = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y;

                if(y > 0 && y >= Mathf.Abs(x))
                {
                    RequestMove(true);
                }
                else if (y < 0 && Mathf.Abs(y) >= Mathf.Abs(x))
                {
                    RequestMove(false);
                }
                else if (x > 0 && x >= Mathf.Abs(y))
                {
                    TurnCamera(true);
                }
                else if (x < 0 && Mathf.Abs(x) >= Mathf.Abs(y))
                {
                    TurnCamera(false);
                }
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                RequestMove(true);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                TurnCamera(false);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                RequestMove(false);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                TurnCamera(true);
            }
        }
    }

    void RequestMove(bool front)
    {
        float movement = front ? Constants.tileSize : -Constants.tileSize;

        if (direction == 0)
        {
            if (CheckIfMovementIsValid((front ? 0 : 2)))
            {
                cameraTransform.position += (new Vector3(0, 0, movement));
            }
        }
        else if (direction == 1)
        {
            if (CheckIfMovementIsValid((front ? 1 : 3)))
            {
                cameraTransform.position += (new Vector3(movement, 0, 0));
            }
        }
        else if (direction == 2)
        {
            if (CheckIfMovementIsValid((front ? 2 : 0)))
            {
                cameraTransform.position += (new Vector3(0, 0, -movement));
            }
        }
        else if (direction == 3)
        {
            if (CheckIfMovementIsValid((front ? 3 : 1)))
            {
                cameraTransform.position += (new Vector3(-movement, 0, 0));
            }
        }
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

    void TurnCamera(bool right)
    {
        if(right)
        {
            cameraTransform.Rotate(new Vector3(0, 90, 0));
            direction = (direction + 1) % 4;
        }
        else
        {
            cameraTransform.Rotate(new Vector3(0, -90, 0));
            direction = (direction - 1) < 0 ? 3 : (direction - 1);
        }
    }
}
