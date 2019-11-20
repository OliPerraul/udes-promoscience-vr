using UnityEngine;
using System.Collections;
using Cirrus.Extensions;
using Cirrus;
using UnityEngine.EventSystems;

namespace UdeS.Promoscience.Labyrinths.UI
{
    public abstract class BaseLabyrinthButton : MonoBehaviour
    {
        public OnDataEvent OnReplayClickedHandler;

        public OnDataEvent OnPlayClickedHandler;

        [SerializeField]
        protected Labyrinth labyrinth;

        public Labyrinth Labyrinth => labyrinth;

        [SerializeField]
        protected UnityEngine.UI.Button button;

        [SerializeField]
        protected UnityEngine.UI.RawImage rawImage;

        private bool init = false;

        public virtual void Awake()
        {
            button.onClick.AddListener(OnClicked);
        }



        public virtual void OnClicked()
        {
            if (OnPlayClickedHandler != null) OnPlayClickedHandler.Invoke(Labyrinth.Data);
        }


    }
}
