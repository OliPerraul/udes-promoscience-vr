using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using UdeS.Promoscience.ScriptableObjects;
using Cirrus.Extensions;

namespace UdeS.Promoscience.UI.Menu
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField]
        private Image headsetImage;

        [SerializeField]
        private Image tabletImage;

        [SerializeField]
        private float disconnectedAlpha = 0.4f;

        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private Image buttonImage;

        private UnityEngine.Color color;

        public UnityEngine.Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
                backgroundImage.color = color;
                buttonImage.color = color;
            }
        }

        [SerializeField]
        private Sprite circleIconTemplate;

        [SerializeField]
        private List<ScriptableTeam> teams;

        [SerializeField]
        private Dropdown dropdown;

        private Button button;

        [SerializeField]
        ScriptableBoolean isConnectedToPair;

        [SerializeField]
        ScriptableBoolean isConnectedToServer;

        [SerializeField]
        GameObject pairPanel;

        [SerializeField]
        GameObject serverPanel;

        void OnEnable()
        {
            isConnectedToPair.valueChangedEvent += OnIsConnectedToPairValueChanged;
            isConnectedToServer.valueChangedEvent += OnIsConnectedToServerValueChanged;
        }

        void Awake()
        {
            //OnNotReady();
            headsetImage.color = headsetImage.color.SetA(disconnectedAlpha);
            tabletImage.color = tabletImage.color.SetA(disconnectedAlpha);
        }

        void OnIsConnectedToPairValueChanged()
        {
            if (isConnectedToPair.Value)
            {
                if (pairPanel.activeSelf)
                {
                    pairPanel.SetActive(false);
                }
            }
            else
            {
                if (isConnectedToServer.Value)
                {
                    pairPanel.SetActive(true);
                }
            }
        }

        void OnIsConnectedToServerValueChanged()
        {
            if (isConnectedToServer.Value)
            {
                if (!isConnectedToPair.Value)
                {
                    pairPanel.SetActive(true);
                }

                if (serverPanel.activeSelf)
                {
                    serverPanel.SetActive(false);
                }
            }
            else
            {
                if (pairPanel.activeSelf)
                {
                    pairPanel.SetActive(false);
                }

                serverPanel.SetActive(true);
            }
        }



        public void OnValidate()
        {
            if (dropdown != null)
            {
                dropdown.ClearOptions();
                
                foreach (var tm in teams)
                {
                    //AddDropdownOption(tm);
                }
            }
        }

        public void OnDropdownItemSelected(int idx)//.OptionData data)
        {
            Color = teams[idx].TeamColor;
        }

        public void OnReady()
        {
            headsetImage.color.SetA(1);
        }

        public void OnNotReady()
        {
            headsetImage.color = headsetImage.color.SetA(disconnectedAlpha);
            tabletImage.color = tabletImage.color.SetA(disconnectedAlpha);
        }



    }
}
