using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletControls : MonoBehaviour
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableVector3 playerPosition;

    [SerializeField]
    ScriptableQuaternion playerRotation;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    Transform cameraTransform;

    void Start ()
    {
        playerPosition.valueChangedEvent += OnNewPlayerPosition;
        playerRotation.valueChangedEvent += OnNewPlayerRotation;
        controls.resetPositionAndRotation += ResetPositionAndRotation;
    }

    void OnNewPlayerPosition()
    {
        if(controls.IsControlsEnabled)
        {
            cameraTransform.position = playerPosition.Value;
        }
    }

    void OnNewPlayerRotation()
    {
        if (controls.IsControlsEnabled)
        {
            cameraTransform.rotation = playerRotation.Value;
        }
    }

    void ResetPositionAndRotation()
    {
        cameraTransform.position = new Vector3(0, cameraTransform.position.y, 0);
       int direction = labyrinth.GetStartDirection();

        Quaternion rotation = new Quaternion(0, 0, 0, 0);

        if (direction == 1)
        {
            rotation.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (direction == 2)
        {
            rotation.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (direction == 3)
        {
            rotation.eulerAngles = new Vector3(0, 270, 0);
        }

        cameraTransform.rotation = rotation;
    }
}