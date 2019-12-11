using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Controls;
//using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.Algorithms
{
    public class TabletDirectiveInterface : MonoBehaviour
    {
        [SerializeField]
        ControlsAsset controls;

        [SerializeField]
        DirectiveManagerAsset directiveManager;

        [SerializeField]
        AlgorithmRespectAsset algorithmRespect;


        [SerializeField]
        private UnityEngine.UI.Button[] buttons;

        private void Awake()
        {
            Client.Instance.State.OnValueChangedHandler += OnClientStateChanged;

            for(int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] == null)
                    continue;

                int index = i;

                buttons[i].onClick.AddListener(() => {
                    Debug.Log((Directive)index);
                    directiveManager.Set((Directive)index);
                }               
                );
            }
        }

        //public void OnCorrectingEnabled(bool enabled)
        //{
        //    buttons[(int)Directive.Stop].image.sprite =
        //        enabled ?
        //            directiveManager.GoDirectiveSprite :
        //            directiveManager.StopDirectiveSprite;
        //}

        public void OnValidate()
        {
            if (buttons == null || buttons.Length == 0)
            {
                buttons = GetComponentsInChildren<UnityEngine.UI.Button>();
            }
        }

        public void OnClientStateChanged(ClientGameState state)
        {
            switch (Client.Instance.State.Value)
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
