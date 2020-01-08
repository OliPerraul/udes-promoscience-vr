using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;

namespace UdeS.Promoscience.Controls
{
    /// <summary>
    /// Display showing headset inputs during the game
    /// </summary>
    public class HeadsetHowToControlsDisplay : MonoBehaviour
    {
        [SerializeField]
        private HeadsetControlsAsset controller;

        [SerializeField]
        private DirectiveManagerAsset directiveManager;

        [SerializeField]
        private HeadsetInputSchemeAsset inputScheme;


        public void Awake()
        {
            directiveManager.CurrentDirective.OnValueChangedHandler += OnNewDirective;
            controller.IsThirdPersonEnabled.OnValueChangedHandler += OnThirdPersonEnabled;
        }

        public void Start()
        {
            gameObject.SetActive(false);
        }

        public void OnThirdPersonEnabled(bool enabled)
        {
            gameObject.SetActive(false);
        }




        void OnNewDirective(Directive directive)
        {

        }

        public void Update()
        {
            if (inputScheme.IsAnyPressed)
            {
                gameObject.SetActive(false);
            }
        }

    }
}