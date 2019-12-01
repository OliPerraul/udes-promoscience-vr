using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Replays.UI
{
    public class ReplayDisplay : Cirrus.BaseSingleton<ReplayDisplay>
    {
        [SerializeField]
        private UnityEngine.UI.RawImage viewRawImage;

        public UnityEngine.UI.RawImage ViewRawImage => viewRawImage;

        [SerializeField]
        private GameObject sidebar;

        [SerializeField]
        private GameObject display;


        public void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnGameStateValueChanged;
        }

        public void OnGameStateValueChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.LabyrinthReplay:
                    display.SetActive(true);
                    //viewRawImage.texture = Server.Instance.CurrentLabyrinth.Camera.RenderTexture;
                    break;

                default:
                    display.SetActive(false);
                    break;
            }

        }
    }
}