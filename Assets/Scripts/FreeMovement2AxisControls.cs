using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Free controls : Use the primary touch pad and it's directions
//Pros: Very simple control
//Cons: Give alot of liberty to the user and need to send more information to save?

//Free movement : Free 2 axis movement limited by collider of the walls and obtacles
//Pros: More immersive and free control
//Cons: Give alot of liberty to the user and need to send more information to save
public class FreeMovement2AxisControls : MonoBehaviour
{
    bool isMovementEnabled = true;

    float _moveSpeed = 1f;

    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    Transform centerEyeAnchor;

    void Update ()
    {
        if (isMovementEnabled)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))//check if hold work if not, add bool that change on hold and on release
            {

                float x = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).x;
                float y = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y;

                float touchPadY = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y;

                float movementSpeed = (Time.deltaTime) * _moveSpeed;
                Vector3 mouvement = new Vector3(movementSpeed * x * Mathf.Sin(Mathf.Deg2Rad * centerEyeAnchor.transform.rotation.eulerAngles.y), 0, movementSpeed * y * Mathf.Cos(Mathf.Deg2Rad * centerEyeAnchor.transform.rotation.eulerAngles.y));
                cameraTransform.position += mouvement;
            }
        }
    }

}
