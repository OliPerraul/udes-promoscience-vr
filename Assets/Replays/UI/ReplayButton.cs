using UnityEngine;
using System.Collections;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays.UI
{
    public class ReplayButton : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Button removeButton;

        [SerializeField]
        private UnityEngine.UI.Button addButton;

        [SerializeField]
        private UnityEngine.UI.Button button;

        private Labyrinths.IData labyrinth;

        public Cirrus.Event<Labyrinths.IData> OnRemovedHandler;

        public void Awake()
        {
            addButton.onClick.AddListener(OnAddedClicked);
            removeButton.onClick.AddListener(OnRemovedClicked);
            button.onClick.AddListener(OnLabyrinthClicked);
        }

        public void OnRemovedClicked()
        {

        }

        public void OnAddedClicked()
        {

        }

        public void OnLabyrinthClicked()
        {

        }

        public ReplayButton Create(
            Transform parent,
            Labyrinths.IData labyrinth,
            bool replayLocked = true)
        {
            var l = this.Create(parent);
            l.labyrinth = labyrinth;
            return l;
        }
    }


}