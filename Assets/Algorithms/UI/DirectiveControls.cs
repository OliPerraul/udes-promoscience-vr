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
        private UnityEngine.UI.Button[] buttons;

        private void Start()
        {
            Client.Instance.clientStateChangedEvent += OnClientStateChanged;

            for(int i = 0; i < buttons.Length; i++)
            {
                int index = i;
                buttons[i].onClick.AddListener(() => directiveManager.Directive.Set((Directive)index));
            }
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
