using UnityEngine;
using System.Collections;
using Cirrus.Extensions;
using System;
using System.Collections.Generic;

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

        public Cirrus.Event<Transform,ReplayButton> OnRemovedHandler;

        public Cirrus.Event OnAddedHandler;

        private ReplayButtonMode mode;

        public ReplayButtonMode Mode
        {
            set
            {
                mode = value;

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

            addButton.onClick.AddListener(() => OnAddedHandler?.Invoke());

            removeButton.onClick.AddListener(OnRemoved);

            Mode = mode;
        }

        public void OnRemoved()
        {
            Labyrinth.gameObject?.Destroy();
            gameObject?.Destroy();
            OnRemovedHandler?.Invoke(transform.parent, this);
        }


        public ReplayButton Create(
            Transform parent,
            Labyrinths.Labyrinth labyrinth,
            ReplayButtonMode mode = ReplayButtonMode.Remove)
        {
            var l = this.Create(parent);
            l.labyrinth = labyrinth;
            l.rawImage.texture = labyrinth.Camera.RenderTexture;
            l.Mode = mode;

            Debug.Log(Enum.GetName(typeof(ReplayButtonMode), mode));

            return l;
        }
    }


}