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

        [SerializeField]
        AlgorithmRespectAsset algorithmRespect;


        [SerializeField]
        private UnityEngine.UI.Button[] buttons;

        private void Awake()
        {
            Client.Instance.clientStateChangedEvent += OnClientStateChanged;

            algorithmRespect.IsCorrectingEnabled.OnValueChangedHandler += OnCorrectingEnabled;

            for(int i = 0; i < buttons.Length; i++)
            {
                int index = i;

                buttons[i].onClick.AddListener(() => {
                    directiveManager.Set((Directive)index);
                }               
                );
            }
        }

        public void OnCorrectingEnabled(bool enabled)
        {
            buttons[(int)Directive.Stop].image.sprite =
                enabled ?
                    directiveManager.GoDirectiveSprite :
                    directiveManager.StopDirectiveSprite;
        }

        public void OnValidate()
        {
            if (buttons == null || buttons.Length == 0)
            {
                buttons = GetComponentsInChildren<UnityEngine.UI.Button>();
            }
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
    }
}
