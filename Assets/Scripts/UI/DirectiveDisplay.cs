﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectiveDisplay : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger directive;

    [SerializeField]
    ScriptableInteger forwardDirection;

    [SerializeField]
    GameObject directiveDisplayer;

    [SerializeField]
    Image directiveImage;

    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    Sprite forwardImage;//temp might change to a ressource manager

    [SerializeField]
    Sprite turnLeftImage;//temp might change to a ressource manager

    [SerializeField]
    Sprite turnRightImage;//temp might change to a ressource manager

    [SerializeField]
    Sprite uTurnImage;//temp might change to a ressource manager

    [SerializeField]
    Sprite stopImage;//temp might change to a ressource manager

    readonly int[] xByDirection = { 0, 1, 0, -1 };
    readonly int[] yByDirection = { -1, 0, 1, 0 };

    readonly float[] rotationByDirection = { 0, 90, 180, 270 };
    float hideTime = 3.0f;
    float hideTimer;

    Vector3 positionFromCamera = new Vector3(2, 0, 2);
    Vector3 baseRotation = new Vector3(0, 0, 0);



    void Start ()
    {
        directive.valueChangedEvent += OnNewDirective;
    }
	
	void Update ()
    {
		if(directiveDisplayer.activeSelf)
        {
            hideTimer += Time.deltaTime;
            if(hideTimer >= hideTime)
            {
                directiveDisplayer.SetActive(false);
            }
            /*
            else
            {
                directiveDisplayer.transform.position = new Vector3(positionFromCamera.x * Mathf.Sin(Mathf.Deg2Rad * cameraTransform.rotation.eulerAngles.y), positionFromCamera.y, positionFromCamera.z * Mathf.Cos(Mathf.Deg2Rad * cameraTransform.rotation.eulerAngles.y)) + cameraTransform.position;
                Quaternion r = new Quaternion();
                r.eulerAngles = new Vector3(baseRotation.x , cameraTransform.rotation.eulerAngles.y, baseRotation.z);
                directiveDisplayer.transform.rotation = r;
            }*/
        }
	}

    void OnNewDirective()
    {
        SetDirectiveImage(directive.value);
        hideTimer = 0;
        directiveDisplayer.SetActive(true);
    }

    void SetDirectiveImage(int directiveId)
    {
        if(directiveId == Constants.DIRECTIVE_MOVE_FOWARD)
        {
            directiveImage.sprite = forwardImage;
        }
        else if (directiveId == Constants.DIRECTIVE_TURN_LEFT)
        {
            directiveImage.sprite = turnLeftImage;
        }
        else if (directiveId == Constants.DIRECTIVE_TURN_RIGHT)
        {
            directiveImage.sprite = turnRightImage;
        }
        else if (directiveId == Constants.DIRECTIVE_UTURN)
        {
            directiveImage.sprite = uTurnImage;
        }
        else if (directiveId == Constants.DIRECTIVE_STOP)
        {
            directiveImage.sprite = stopImage;
        }

        SetDisplayPosition();
    }

    void SetDisplayPosition()
    {
        directiveDisplayer.transform.position = new Vector3((xByDirection[forwardDirection.value] * positionFromCamera.x), positionFromCamera.y, (yByDirection[forwardDirection.value] * positionFromCamera.z));
        Quaternion r = new Quaternion();
        r.eulerAngles = new Vector3(baseRotation.x, rotationByDirection[forwardDirection.value], baseRotation.z);
        directiveDisplayer.transform.rotation = r;
    }
}
