using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationControls : MonoBehaviour
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableInteger action;

    [SerializeField]
    Transform cameraBaseTransform;

    List<int> actionsList = new List<int>();

    void Start ()
    {
        controls.stopAllMovementEvent += StopAllMovement;
        controls.stopAllMovementEvent += ResetRotation;
        action.valueChangedEvent += AddAction;
	}
	
	
	void Update ()
    {
        if (controls.isControlsEnabled)
        {
            if (actionsList.Count > 0)
            {
                if (actionsList[0] == Constants.ACTION_TURN_LEFT)
                {
                    cameraBaseTransform.Rotate(new Vector3(0, -90, 0));
                }
                else if (actionsList[0] == Constants.ACTION_TURN_RIGHT)
                {
                    cameraBaseTransform.Rotate(new Vector3(0, 90, 0));
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
        //isTurning = false;
    }

    void ResetRotation()
    {
        cameraBaseTransform.rotation = new Quaternion(0, 0, 0, 0);
    }
}
