using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2 axis controls : Use the primary touch par as 4 directions arrows
//Pros: Very simple and fast control
//Cons: Not very immersive controls and ambiguity caused by the touch pad keeping the last area touched for a short laps and cause you to do what you wanted to do

//free movement : Free 2 axis movement limited by collider of the walls and obtacles
//Pros: 
//Cons: 
public class FreeMovement2AxisControls : MonoBehaviour
{
    bool isMovementEnabled = true;

    public int direction = 0;//0 == 0 degree, 1 == 90 degree, 2 == 180 degree, 3 == 270 degree

    [SerializeField]
    ScriptableLabyrinth labyrinth;

    [SerializeField]
    LabyrinthVisual labyrinthVisual;

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

                float x = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x;
                float y = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y;

                float touchPadY = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y;
                float rotationY = Mathf.Abs(centerEyeAnchor.rotation.eulerAngles.y) % 360;
                int direction;

                if (rotationY < 45 && rotationY >= 0 || rotationY <= 360 && rotationY > 315)
                {
                    direction = touchPadY > 0 ? 0 : 2;
                }
                else if (rotationY > 45 && rotationY < 135)
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

                if (y > 0 && y >= Mathf.Abs(x))
                {
                    
                }
                else if (y < 0 && Mathf.Abs(y) >= Mathf.Abs(x))
                {
                    
                }
                else if (x > 0 && x >= Mathf.Abs(y))
                {
                   
                }
                else if (x < 0 && Mathf.Abs(x) >= Mathf.Abs(y))
                {
                    
                }
            }
        }
    }

    void Move(float xAxis, float yAxis)
    {
        /*
        float mouvement = front ? Constants.tileSize : -Constants.tileSize;

        if (direction == 0)
        {
            if (CheckIfMovementIsValid((front ? 0 : 2)))
            {
                cameraTransform.position += (new Vector3(0, 0, mouvement));
            }
        }
        else if (direction == 1)
        {
            if (CheckIfMovementIsValid((front ? 1 : 3)))
            {
                cameraTransform.position += (new Vector3(mouvement, 0, 0));
            }
        }
        else if (direction == 2)
        {
            if (CheckIfMovementIsValid((front ? 2 : 0)))
            {
                cameraTransform.position += (new Vector3(0, 0, -mouvement));
            }
        }
        else if (direction == 3)
        {
            if (CheckIfMovementIsValid((front ? 3 : 1)))
            {
                cameraTransform.position += (new Vector3(-mouvement, 0, 0));
            }
        }*/
    }
}
