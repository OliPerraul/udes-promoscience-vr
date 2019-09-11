using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{
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
        Sprite forwardImage;

        [SerializeField]
        Sprite stopImage;

        [SerializeField]
        Sprite turnLeftImage;

        [SerializeField]
        Sprite turnRightImage;

        [SerializeField]
        Sprite uTurnImage;

        const float hideTime = 3.0f;

        float hideTimer;

        void OnEnable()
        {
            directive.valueChangedEvent += OnNewDirective;
        }

        void Update()
        {
            if (directiveDisplayer.activeSelf)
            {
                hideTimer += Time.deltaTime;

                float scale = hideTimer < hideTime / 2 ? 1.0f + (hideTimer / (hideTime / 2)) / 2 : 1.5f - ((hideTimer - (hideTime / 2)) / (hideTime / 2)) / 2;
                gameObject.transform.localScale += new Vector3(scale - gameObject.transform.localScale.x, scale - gameObject.transform.localScale.y, 0f);

                if (hideTimer >= hideTime)
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
            if (dir == Directive.MoveForward)
            {
                directiveImage.sprite = forwardImage;
            }
            else if (dir == Directive.Stop)
            {
                directiveImage.sprite = stopImage;
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
        }
    }
}
