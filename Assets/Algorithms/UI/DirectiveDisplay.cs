using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{
    public class DirectiveDisplay : MonoBehaviour
    {
        [SerializeField]
        DirectiveManagerAsset directive;

        private Image directiveImage;

        [SerializeField]
        private Image[] images;

        const float hideTime = 3.0f;

        float hideTimer;

        private bool init = false;

        void OnEnable()
        {
            if (init) return;

            init = true;

            directive.Directive.OnValueChangedHandler += OnNewDirective;
            Client.Instance.clientStateChangedEvent += OnClientStateChanged;

            foreach (var img in images)
            {
                img.gameObject.SetActive(false);
            }

            if(images.Length < 0)
            directiveImage = images[0];

        }

        public void OnValidate()
        {
            if (images == null || images.Length == 0)
            {
                images = GetComponentsInChildren<Image>();
            }
        }


        void OnClientStateChanged()
        {
            switch (Client.Instance.State)
            {
                case ClientGameState.Playing:
                case ClientGameState.PlayingTutorial:
                    if (directiveImage != null)
                        directiveImage.gameObject.SetActive(false);
                    break;
            }
        }

        void Update()
        {
            if (directiveImage == null)
                return;

            if (directiveImage.gameObject.activeSelf)
            {
                hideTimer += Time.deltaTime;

                float scale = hideTimer < hideTime / 2 ? 1.0f + (hideTimer / (hideTime / 2)) / 2 : 1.5f - ((hideTimer - (hideTime / 2)) / (hideTime / 2)) / 2;
                gameObject.transform.localScale += new Vector3(scale - gameObject.transform.localScale.x, scale - gameObject.transform.localScale.y, 0f);

                if (hideTimer >= hideTime)
                {
                    directiveImage.gameObject.SetActive(false);
                }
            }
        }

        void OnNewDirective(Directive directive)
        {
            if (directiveImage != null)
                directiveImage.gameObject.SetActive(false);

            directiveImage = images[(int)directive];
 
            hideTimer = 0;
            directiveImage.gameObject.SetActive(true);
        }

    }
}
