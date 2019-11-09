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
        AvatarControllerAsset controls;

        [SerializeField]
        DirectiveManagerAsset directiveManager;

        private void Start()
        {
            controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
            controls.isPlayerControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
        }

        void OnControlsEnableValueChanged()
        {
            if (controls.IsControlsEnabled.Value && controls.IsPlayerControlsEnabled.Value)
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
            directiveManager.Value = Directive.MoveForward;
        }

        public void SetDirectiveTurnLeft()
        {
            directiveManager.Value = Directive.TurnLeft;
        }

        public void SetDirectiveTurnRight()
        {
            directiveManager.Value = Directive.TurnRight;
        }

        public void SetDirectiveUTurn()
        {
            directiveManager.Value = Directive.UTurn;
        }

        public void SetDirectiveStop()
        {
            directiveManager.Value = Directive.Stop;
        }
    }
}
