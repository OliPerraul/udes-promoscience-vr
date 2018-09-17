using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectiveDisplay : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger directive;

    [SerializeField]
    GameObject directiveDisplayer;

    [SerializeField]
    Image directiveImage;

    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    Image fowardImage;//temp might change to a ressource manager

    [SerializeField]
    Image turnLeftImage;//temp might change to a ressource manager

    [SerializeField]
    Image turnRightImage;//temp might change to a ressource manager

    [SerializeField]
    Image uTurnImage;//temp might change to a ressource manager

    [SerializeField]
    Image stopImage;//temp might change to a ressource manager

    float hideTime = 5.0f;
    float hideTimer;

    Vector3 positionFromCamera = new Vector3(0,2,2);

    

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
            else
            {
                directiveDisplayer.transform.position = new Vector3(positionFromCamera.x * Mathf.Sin(Mathf.Deg2Rad * cameraTransform.rotation.eulerAngles.y), 0, positionFromCamera.z * Mathf.Cos(Mathf.Deg2Rad * cameraTransform.rotation.eulerAngles.y)) + cameraTransform.position;
            }
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
            directiveImage = fowardImage;
        }
        else if (directiveId == Constants.DIRECTIVE_TURN_LEFT)
        {
            directiveImage = turnLeftImage;
        }
        else if (directiveId == Constants.DIRECTIVE_TURN_RIGHT)
        {
            directiveImage = turnRightImage;
        }
        else if (directiveId == Constants.DIRECTIVE_UTURN)
        {
            directiveImage = uTurnImage;
        }
        else if (directiveId == Constants.DIRECTIVE_STOP)
        {
            directiveImage = stopImage;
        }
    }
}
