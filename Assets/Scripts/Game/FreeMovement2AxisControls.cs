using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Free controls : Use the primary touch pad and it's directions
//Pros: Very simple control
//Cons: Give alot of liberty to the user and need to send more information to save?

//Free movement : Free movement limited by collider of the walls and obtacles
//Pros: More immersive and free control
//Cons: Give alot of liberty to the user and need to send more information to save
public class FreeMovement2AxisControls : MonoBehaviour
{
    bool isMovementEnabled = true;
    bool isMoving = false;

    float _moveSpeed = 10f;

    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    Transform centerEyeAnchor;

    void Update ()
    {
        if (isMovementEnabled)
        {
            if(OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
            {
                isMoving = true;
            }
            else if(OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
            {
                isMoving = false;
            }

            if (isMoving)//check if hold work if not, add bool that change on hold and on release
            {

                //float x = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x;
                float y = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y;

                float movementSpeed = (Time.deltaTime) * _moveSpeed;
                cameraTransform.position += new Vector3(movementSpeed * y * Mathf.Sin(Mathf.Deg2Rad * centerEyeAnchor.transform.rotation.eulerAngles.y), 0.0f, movementSpeed * y * Mathf.Cos(Mathf.Deg2Rad * centerEyeAnchor.transform.rotation.eulerAngles.y));
            }
        }
    }
}
