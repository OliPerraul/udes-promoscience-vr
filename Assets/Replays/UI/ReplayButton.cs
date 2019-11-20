using UnityEngine;
using System.Collections;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays.UI
{
    public enum ReplayButtonMode
    {
        Add,
        Remove,
        Both,
        None

    }


    public class ReplayButton : Labyrinths.UI.BaseLabyrinthButton
    {
        [SerializeField]
        private UnityEngine.UI.Button removeButton;

        [SerializeField]
        private UnityEngine.UI.Button addButton;

        public Cirrus.Event<Labyrinths.IData> OnRemovedHandler;

        public ReplayButtonMode Mode
        {
            set
            {
                switch (value)
                {
                    case ReplayButtonMode.Add:
                        removeButton.gameObject.SetActive(false);
                        addButton.gameObject.SetActive(true);
                        break;

                    case ReplayButtonMode.Remove:
                        removeButton.gameObject.SetActive(true);
                        addButton.gameObject.SetActive(false);
                        break;

                    case ReplayButtonMode.Both:
                        removeButton.gameObject.SetActive(true);
                        addButton.gameObject.SetActive(true);
                        break;

                    case ReplayButtonMode.None:
                        removeButton.gameObject.SetActive(false);
                        addButton.gameObject.SetActive(false);
                        break;

                }
            }
        }


        public override void Awake()
        {
            base.Awake();

            addButton.onClick.AddListener(OnAddedClicked);
            removeButton.onClick.AddListener(OnRemovedClicked);
        }

        public void OnRemovedClicked()
        {

        }

        public void OnAddedClicked()
        {

        }

        public ReplayButton Create(
            Transform parent,
            Labyrinths.Labyrinth labyrinth,
            ReplayButtonMode mode=ReplayButtonMode.Remove)
        {
            var l = this.Create(parent);
            l.labyrinth = labyrinth;
            l.rawImage.texture = labyrinth.Camera.RenderTexture;
            l.Mode = mode;

            return l;
        }
    }


}