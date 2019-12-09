using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;
using System;

namespace UdeS.Promoscience.UI
{
    public class DirectiveDisplay : MonoBehaviour
    {
        [SerializeField]
        private Controls.ControlsAsset controls;

        [SerializeField]
        private Algorithms.AlgorithmRespectAsset algorithmRespect;

        [SerializeField]
        private DirectiveManagerAsset directiveManager;

        
        private Image directiveImage;

        [SerializeField]
        private Image[] images;

        const float hideTime = 3.0f;

        float hideTimer;

        void Awake()
        {
            directiveManager.CurrentDirective.OnValueChangedHandler += OnNewDirective;
            algorithmRespect.IsCorrectingEnabled.OnValueChangedHandler += OnCorrectingEnabled;
            controls.IsThirdPersonEnabled.OnValueChangedHandler += OnThirdpersonENabled;
            Client.Instance.State.OnValueChangedHandler += OnClientStateChanged;

            foreach (var img in images)
            {
                img.gameObject.SetActive(false);
            }

            if (directiveImage == null)
                directiveImage = images[0];

            if (directiveImage != null)
                directiveImage.gameObject.SetActive(false);


        }

        private void OnThirdpersonENabled(bool value)
        {
            if (directiveImage != null)
                directiveImage.gameObject.SetActive(false);
        }

        public void OnValidate()
        {
            if (images == null || images.Length == 0)
            {
                images = GetComponentsInChildren<Image>();
            }
        }


        void OnClientStateChanged(ClientGameState state)
        {
            switch (state)
            {
                case ClientGameState.Connecting:
                case ClientGameState.WaitingForNextRound:
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

            switch (directive)
            {
                case Directive.Question:
                case Directive.Compass:
                    return;

                case Directive.Stop:
                    directiveImage = images[(int)directive-2];

                    directiveImage.sprite =
                        directiveImage.sprite == directiveManager.StopDirectiveSprite ?
                            directiveManager.GoDirectiveSprite :
                            directiveManager.StopDirectiveSprite;
                    break;

                default:
                    directiveImage = images[(int)directive];
                    break;
            }

            hideTimer = 0;
            directiveImage.gameObject.SetActive(true);
        }

        public void OnCorrectingEnabled(bool enabled)
        {
            //images[(int)Directive.Stop].sprite =
            //    enabled ?
            //        directiveManager.GoDirectiveSprite :
            //        directiveManager.StopDirectiveSprite;
        }
    }
}
