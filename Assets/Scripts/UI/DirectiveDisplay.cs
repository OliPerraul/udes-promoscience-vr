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
    Sprite forwardImage;//temp might change to a ressource manager

    [SerializeField]
    Sprite turnLeftImage;//temp might change to a ressource manager

    [SerializeField]
    Sprite turnRightImage;//temp might change to a ressource manager

    [SerializeField]
    Sprite uTurnImage;//temp might change to a ressource manager

    [SerializeField]
    Sprite stopImage;//temp might change to a ressource manager

    float hideTime = 3.0f;
    float hideTimer;

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
        }
	}

    void OnNewDirective()
    {
        SetDirectiveImage(directive.Value);
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
    }
}
