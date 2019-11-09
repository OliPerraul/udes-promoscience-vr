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

            Client.Instance.clientStateChangedEvent += OnClientStateChanged;
        }


        public void OnClientStateChanged()
        {
            switch (Client.Instance.State)
            {
                case ClientGameState.Playing:
                case ClientGameState.PlayingTutorial:                
                    Enabled = true;
                    break;

                default:
                    Enabled = false;
                    break;

            }
        }

        public bool Enabled
        {
            set
            {
                foreach (Transform child in transform)
                {
                    if (child != this.transform)
                    {
                        child.gameObject.SetActive(value);
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
