using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Playbacks.UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField]
        private ScriptableObjects.ScriptableServerGameInformation server;

        [SerializeField]
        private UnityEngine.UI.Button openButton;

         [SerializeField]
        private UnityEngine.UI.Button exitButton;

        [SerializeField]
        private Panel panel;

        [SerializeField]
        private Panel panelExpanded;

        private bool isPanelExpanded = false;

        private Panel activePanel;

        public void Awake()
        {
            panel.gameObject.SetActive(true);
            panelExpanded.gameObject.SetActive(true);


            panel.OnExpandHandler += OnExpand;
            panelExpanded.OnExpandHandler += OnExpand;

            openButton.onClick.AddListener(OnOpenClicked);
            exitButton.onClick.AddListener(OnExitClicked);

            panelExpanded.gameObject.SetActive(false);
            activePanel = panel;
        }

        public void OnOpenClicked()
        {
            activePanel.gameObject.SetActive(true);
        }

        public void OnExpand()
        {
            if (isPanelExpanded)
            {
                isPanelExpanded = false;
                panel.gameObject.SetActive(true);
                panelExpanded.gameObject.SetActive(false);
                activePanel = panel;
            }
            else
            {
                isPanelExpanded = true;
                panel.gameObject.SetActive(false);
                panelExpanded.gameObject.SetActive(true);
                activePanel = panelExpanded;
            }
         
        }

        public void OnExitClicked()
        {
            server.EndRoundOrTutorial();
        }

    }
}