using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionDisplay : MonoBehaviour
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableInteger fowardDirection;

    [SerializeField]
    GameObject directionDisplayer;

    [SerializeField]
    Transform cameraTransform;

    void Start()
    {
        fowardDirection.valueChangedEvent += OnNewFowardDirection;
        controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
    }
	

    void OnNewFowardDirection()
    {
        if(fowardDirection.value == 0)
        {
            directionDisplayer.transform.position = cameraTransform.position - new Vector3(0, -0.5f, 0.5f);
        }
        else if (fowardDirection.value == 1)
        {
            directionDisplayer.transform.position = cameraTransform.position - new Vector3(0.5f, -0.5f, 0);
        }
        else if (fowardDirection.value == 2)
        {
            directionDisplayer.transform.position = cameraTransform.position - new Vector3( 0, -0.5f, -0.5f);
        }
        else if (fowardDirection.value == 3)
        {
            directionDisplayer.transform.position = cameraTransform.position - new Vector3(-0.5f, -0.5f, 0);
        }
    }

    void OnControlsEnableValueChanged()
    {
        if(controls.isControlsEnabled)
        {
            directionDisplayer.SetActive(true);
        }
        else
        {
            directionDisplayer.SetActive(false);
        }
    }

}
