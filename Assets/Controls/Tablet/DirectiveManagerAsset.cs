using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.ScriptableObjects
{
    /// <summary>
    /// Holds directive values and dispatch directive related event
    /// </summary>
    public class DirectiveManagerAsset : ScriptableObject
    {
        [SerializeField]
        private GameActionManagerAsset gameActionManager;

        [SerializeField]
        public Cirrus.ObservableValue<Directive> CurrentDirective = new Cirrus.ObservableValue<Directive>();

        public void OnEnable()
        {
            CurrentDirective.OnValueChangedHandler += OnDirective;
        }

        public void OnDisable()
        {
            CurrentDirective.OnValueChangedHandler -= OnDirective;
        }

        public void Set(Directive directive)
        {
            CurrentDirective.Set(directive, notify: true);
        }

        void OnDirective(Directive directive)
        {
            switch (directive)
            {
                case Directive.MoveForward:
                    gameActionManager.SetAction(GameAction.ReceivedDirectiveMoveForward);
                    break;

                case Directive.Stop:
                    gameActionManager.SetAction(GameAction.ReceivedDirectiveStop);
                    break;

                case Directive.TurnLeft:
                    gameActionManager.SetAction(GameAction.ReceivedDirectiveTurnLeft);
                    break;

                case Directive.TurnRight:
                    gameActionManager.SetAction(GameAction.ReceivedDirectiveTurnRight);
                    break;

                case Directive.UTurn:
                    gameActionManager.SetAction(GameAction.ReceivedDirectiveUturn);
                    break;
            }
        }
    }
}

