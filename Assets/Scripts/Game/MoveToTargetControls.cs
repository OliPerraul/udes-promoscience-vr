using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetControls : MonoBehaviour
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableVector3 action;

    [SerializeField]
    Transform cameraTransform;

    bool isMoving = false;

    float movementSpeed = 0.5f;

    Vector3 trajectoryToTargetPosition;

    List<Vector3> targetPositions = new List<Vector3>();



    void Start ()
    {
        controls.stopAllMovementEvent += StopAllMovement;
        controls.stopAllMovementEvent += ResetPosition;
        action.valueChangedEvent += AddTargetPosition;
	}
	
	
	void Update ()
    {
        if (controls.IsControlsEnabled)
        {
            if (targetPositions.Count > 0 && !isMoving)
            {
                trajectoryToTargetPosition = (targetPositions[0] - cameraTransform.position);

                isMoving = true;
            }

            if (isMoving)
            {
                Vector3 movementPosition = cameraTransform.position + (trajectoryToTargetPosition * Time.deltaTime * movementSpeed);

                if ((trajectoryToTargetPosition.x == 0
                    || movementPosition.x >= targetPositions[0].x && trajectoryToTargetPosition.x > 0
                    || movementPosition.x <= targetPositions[0].x && trajectoryToTargetPosition.x < 0)
                   && (trajectoryToTargetPosition.z == 0
                    || movementPosition.z >= targetPositions[0].z && trajectoryToTargetPosition.z > 0
                    || movementPosition.z <= targetPositions[0].z && trajectoryToTargetPosition.z < 0))
                {
                    cameraTransform.position = targetPositions[0];
                    targetPositions.RemoveAt(0);
                    isMoving = false;
                }
                else
                {
                    cameraTransform.position = movementPosition;
                }
            }
        }
    }

    void AddTargetPosition()
    {
        targetPositions.Add(action.Value);
    }

    void StopAllMovement()
    {
        targetPositions.Clear();
        isMoving = false;
    }

    void ResetPosition()
    {
        cameraTransform.position = new Vector3(0, cameraTransform.position.y, 0);
    }
}
