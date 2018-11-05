using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectiveDisplay : MonoBehaviour
{
    [SerializeField]
    ScriptableDirective directive;

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

    void SetDirectiveImage(Directive dir)
    {
        if(dir == Directive.MoveFoward)
        {
            directiveImage.sprite = forwardImage;
        }
        else if (dir == Directive.TurnLeft)
        {
            directiveImage.sprite = turnLeftImage;
        }
        else if (dir == Directive.TurnRight)
        {
            directiveImage.sprite = turnRightImage;
        }
        else if (dir == Directive.UTurn)
        {
            directiveImage.sprite = uTurnImage;
        }
        else if (dir == Directive.Stop)
        {
            directiveImage.sprite = stopImage;
        }
    }
}
