using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.Algorithms
{
    public class DirectiveControls : MonoBehaviour
    {
        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        ScriptableDirective directive;

        private void Start()
        {
            controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
            controls.isPlayerControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
        }

        void OnControlsEnableValueChanged()
        {
            if (controls.IsControlsEnabled && controls.IsPlayerControlsEnabled)
            {
                foreach (Transform child in transform)
                {
                    if (child != this.transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                foreach (Transform child in transform)
                {
                    if (child != this.transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }

        public void SetDirectiveMoveFoward()
        {
            directive.Value = Directive.MoveForward;
        }

        public void SetDirectiveTurnLeft()
        {
            directive.Value = Directive.TurnLeft;
        }

        public void SetDirectiveTurnRight()
        {
            directive.Value = Directive.TurnRight;
        }

        public void SetDirectiveUTurn()
        {
            directive.Value = Directive.UTurn;
        }

        public void SetDirectiveStop()
        {
            directive.Value = Directive.Stop;
        }
    }
}
