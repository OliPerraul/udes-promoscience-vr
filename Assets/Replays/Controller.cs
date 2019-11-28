using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Replays
{
    public class Controller : MonoBehaviour
    {
        [SerializeField]
        private GameObject sidebar;

        [SerializeField]
        private GameObject display;

        [SerializeField]
        private UnityEngine.UI.Button infoButton;

        [SerializeField]
        private UnityEngine.UI.Button exitButton;

        [SerializeField]
        private UnityEngine.UI.RawImage viewRawImage;

        public void Awake()
        {
            exitButton.onClick.AddListener(() => Server.Instance.EndRoundOrTutorial());
            infoButton.onClick.AddListener(() => sidebar.gameObject.SetActive(!sidebar.gameObject.activeSelf));
            Server.Instance.gameStateChangedEvent += OnGameStateValueChanged;
        }

        public void OnGameStateValueChanged()
        {
            switch (Server.Instance.GameState)
            {
                case ServerGameState.AdvancedReplay:
                case ServerGameState.InstantReplay:
                    display.SetActive(true);
                    viewRawImage.texture = Server.Instance.Labyrinths.CurrentLabyrinth.Camera.RenderTexture;
                    break;

                default:
                    display.SetActive(false);
                    break;
            }

        }

        public void Update()
        {

        }
    }
}