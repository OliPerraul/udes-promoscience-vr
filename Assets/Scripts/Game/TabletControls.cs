using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletControls : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger action;

    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableInteger forwardDirection;

    [SerializeField]
    Transform cameraTransform;

    List<int> actionsList = new List<int>();

    bool isMoving = false;
    bool isTurning = false;

    float lerpValue = 0;

    Vector3 targetPosition;
    Vector3 fromPosition;

    Quaternion fromRotation;
    Quaternion targetRotation;

    void Start ()
    {
        controls.stopAllMovementEvent += StopAllMovement;
        controls.stopAllMovementEvent += ResetPositionAndRotation;
        action.valueChangedEvent += AddAction;
	}
	
	
	void Update ()
    {
        if (controls.isControlsEnabled)
        {
            if (isMoving)
            {
                lerpValue += Time.deltaTime * Constants.MOVEMENT_SPEED;

                if (lerpValue >= 1)
                {
                    cameraTransform.position = targetPosition;
                    lerpValue = 0;
                    isMoving = false;
                }
                else
                {
                    cameraTransform.position = Vector3.Lerp(fromPosition, targetPosition, lerpValue);
                }
            }
            else if (isTurning)
            {
                lerpValue += Time.deltaTime * Constants.TURNING_SPEED;

                if (lerpValue >= 1)
                {
                    cameraTransform.rotation = targetRotation;
                    lerpValue = 0;
                    isTurning = false;
                }
                else
                {
                    cameraTransform.rotation = Quaternion.Lerp(fromRotation, targetRotation, lerpValue);
                }
            }
            else if (actionsList.Count > 0)
            {
                if (actionsList[0] == Constants.ACTION_TURN_LEFT)
                {
                    Quaternion trajectory = new Quaternion();
                    trajectory.eulerAngles += new Vector3(0, -90, 0);
                    fromRotation = cameraTransform.rotation;
                    targetRotation = fromRotation * trajectory;
                    forwardDirection.value = (forwardDirection.value - 1) < 0 ? 3 : (forwardDirection.value - 1);
                    isTurning = true;
                }
                else if (actionsList[0] == Constants.ACTION_TURN_RIGHT)
                {
                    Quaternion trajectory = new Quaternion();
                    trajectory.eulerAngles += new Vector3(0, 90, 0);
                    fromRotation = cameraTransform.rotation;
                    targetRotation = fromRotation * trajectory;
                    forwardDirection.value = (forwardDirection.value + 1) % 4;
                    isTurning = true;
                }
                else if (actionsList[0] == Constants.ACTION_MOVE_UP)
                {
                    fromPosition = cameraTransform.position;
                    targetPosition = fromPosition + (new Vector3(0, 0, Constants.TILE_SIZE));
                    isMoving = true;
                }
                else if (actionsList[0] == Constants.ACTION_MOVE_RIGHT)
                {
                    fromPosition = cameraTransform.position;
                    targetPosition = fromPosition + (new Vector3(Constants.TILE_SIZE, 0, 0));
                    isMoving = true;
                }
                else if (actionsList[0] == Constants.ACTION_MOVE_DOWN)
                {
                    fromPosition = cameraTransform.position;
                    targetPosition = fromPosition + (new Vector3(0, 0, -Constants.TILE_SIZE));
                    isMoving = true;
                }
                else if (actionsList[0] == Constants.ACTION_MOVE_LEFT)
                {
                    fromPosition = cameraTransform.position;
                    targetPosition = fromPosition + (new Vector3(-Constants.TILE_SIZE, 0, 0));
                    isMoving = true;
                }

                actionsList.RemoveAt(0);
            }
        }
    }

    void AddAction()
    {
        actionsList.Add(action.value);
    }

    void StopAllMovement()
    {
        actionsList.Clear();
        isMoving = false;
        isTurning = false;
        lerpValue = 0;
    }

    void ResetPositionAndRotation()
    {
        cameraTransform.position = new Vector3(0, cameraTransform.position.y, 0);
        cameraTransform.rotation = new Quaternion(0, 0, 0, 0);
    }
}