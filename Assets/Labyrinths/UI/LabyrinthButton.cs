using UnityEngine;
using System.Collections;
using Cirrus.Extensions;
using Cirrus;
using UnityEngine.EventSystems;

namespace UdeS.Promoscience.Labyrinths.UI
{
    public class LabyrinthButton : MonoBehaviour
    {
        public OnLabyrinthEvent OnReplayClickedHandler;

        public OnLabyrinthEvent OnPlayClickedHandler;

        private bool ReplayLocked
        {
            set
            {
                if (value)
                {
                    replayBackground.color = replayBackground.color.SetA(0.5f);
                    replayIcon.color = replayIcon.color.SetA(0.5f);
                    lockIcon.color = lockIcon.color.SetA(196f / 255f);
                    replayButton.interactable = false;
                }
                else
                {
                    replayBackground.color = replayBackground.color.SetA(196f/255f);
                    replayIcon.color = replayIcon.color.SetA(196f / 255f);
                    lockIcon.color = lockIcon.color.SetA(0f);
                    replayButton.interactable = true;
                }
            }
        }

        public Labyrinth Labyrinth;

        [SerializeField]
        private UnityEngine.UI.Button playButton;

        [SerializeField]
        private UnityEngine.UI.Button replayButton;

        [SerializeField]
        private UnityEngine.UI.Image replayBackground;

        [SerializeField]
        private UnityEngine.UI.Image replayIcon;

        [SerializeField]
        private UnityEngine.UI.Image lockIcon;

        private bool init = false;

        public LabyrinthButton Create(
            Transform parent, 
            Labyrinth labyrinth,
            bool replayLocked=true)
        {
            var l = this.Create(parent);
            l.Labyrinth = labyrinth;
            l.playButton.onClick.AddListener(l.OnPlayClicked);
            l.replayButton.onClick.AddListener(l.OnReplayClicked);
            l.ReplayLocked = replayLocked;
            return l;
        }

        public void OnReplayClicked()
        {            
            if(OnReplayClickedHandler != null) OnReplayClickedHandler.Invoke(Labyrinth);
        }

        public void OnPlayClicked()
        {
            if (OnPlayClickedHandler != null) OnPlayClickedHandler.Invoke(Labyrinth);
        }

    }
}
