using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;

namespace UdeS.Promoscience.Controls
{
    public class ControlsDisplay : MonoBehaviour
    {
        [SerializeField]
        private DirectiveManagerAsset directiveManager;

        [SerializeField]
        private HeadsetInputSchemeAsset inputScheme;


        public void Awake()
        {
            directiveManager.CurrentDirective.OnValueChangedHandler += OnNewDirective;
        }


        void OnNewDirective(Directive directive)
        {
            // TODO handle somewhere else?
            // QUestion directive brings up the help
            if (directive == Directive.Question)
            {
                gameObject.SetActive(true);
            }
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