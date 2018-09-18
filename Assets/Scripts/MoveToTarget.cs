using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    [SerializeField]
    ScriptableVector3 action;

    [SerializeField]
    Transform cameraTransform;

    bool isMoving = false;

    float clippingRadius = 0.01f;
    float movementSpeed = 0.5f;

    Vector3 trajectoryToTargetPosition;

    List<Vector3> targetPositions = new List<Vector3>();



    void Start ()
    {
        action.valueChangedEvent += AddTargetPosition;
	}
	
	
	void Update ()
    {
        if (targetPositions.Count > 0 && !isMoving)
        {
            trajectoryToTargetPosition = (targetPositions[0] - cameraTransform.position);

            isMoving = true;
        }


        if(isMoving)
        {
            float tileSize = Constants.tileSize;
            Vector3 movementPosition = cameraTransform.position + (trajectoryToTargetPosition * Time.deltaTime * movementSpeed);

            if (movementPosition.x > targetPositions[0].x - clippingRadius * tileSize
                && movementPosition.x < targetPositions[0].x + clippingRadius * tileSize
                && movementPosition.z > targetPositions[0].z - clippingRadius * tileSize
                && movementPosition.z < targetPositions[0].z + clippingRadius * tileSize)
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

    void AddTargetPosition()
    {
        targetPositions.Add(action.value);
    }

    public void StopAllMovement()
    {
        targetPositions.Clear();
        isMoving = false;
    }
}
