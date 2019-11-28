using UnityEngine;
using System.Collections;
using Cirrus.Extensions;
using System;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replays.UI
{
    public enum ButtonMode
    {
        Add,
        Remove,
        Both,
        None
    }

    public class ReplayButton : Labyrinths.UI.BaseButton
    {
        [SerializeField]
        private ReplayControllerAsset replayController;

        [SerializeField]
        private UnityEngine.UI.Button removeButton;

        [SerializeField]
        private UnityEngine.UI.Button addButton;

        public Cirrus.Event<Transform,ReplayButton> OnRemovedHandler;

        public Cirrus.Event OnAddedHandler;

        public Cirrus.Event<ReplayButton> OnReplayClickedHandler;

        private BaseReplay replay;

        private ButtonMode mode;

        public ButtonMode Mode
        {
            set
            {
                mode = value;

                switch (value)
                {
                    case ButtonMode.Add:
                        removeButton.gameObject.SetActive(false);
                        addButton.gameObject.SetActive(true);
                        break;

                    case ButtonMode.Remove:
                        removeButton.gameObject.SetActive(true);
                        addButton.gameObject.SetActive(false);
                        break;

                    case ButtonMode.Both:
                        removeButton.gameObject.SetActive(true);
                        addButton.gameObject.SetActive(true);
                        break;

                    case ButtonMode.None:
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

        public override void OnClick()
        {
            if (replay == null)
            { 
                replay = new SingleReplay(
                    replayController,
                    labyrinth);
            }            
            
            Server.Instance.StartAdvancedReplay(replay);
        }

        public void OnRemoved()
        {
            Labyrinth.gameObject?.Destroy();
            gameObject?.Destroy();
            OnRemovedHandler?.Invoke(transform.parent, this);
        }

        public override Labyrinths.UI.BaseButton Create(
            Transform parent,
            Labyrinths.Labyrinth labyrinth)
        {
            return CreateReplayButton(parent, labyrinth);
        }

        public ReplayButton CreateReplayButton(
            Transform parent,
            Labyrinths.Labyrinth labyrinth)
        {
            var l = this.Create(parent);
            l.labyrinth = labyrinth;
            l.rawImage.texture = labyrinth.Camera.RenderTexture;

            return l;
        }
    }


}