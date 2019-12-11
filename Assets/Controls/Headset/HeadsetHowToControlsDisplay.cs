using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;

namespace UdeS.Promoscience.Controls
{
    public class HeadsetHowToControlsDisplay : MonoBehaviour
    {
        [SerializeField]
        private ControlsAsset controller;

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
            //// TODO handle somewhere else?
            //// QUestion directive brings up the help
            //if (directive == Directive.Question)
            //{
            //    gameObject.SetActive(true);
            //}
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