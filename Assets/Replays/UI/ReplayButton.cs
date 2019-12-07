﻿using UnityEngine;
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
        private PreviewReplay replay;

        [SerializeField]
        private AlgorithmSelectionInteface algorithmSelection;

        [SerializeField]
        private UnityEngine.UI.Button removeButton;

        [SerializeField]
        private UnityEngine.UI.Button addButton;

        [SerializeField]
        private UnityEngine.UI.Text roundNumberText;

        [SerializeField]
        private UnityEngine.UI.Text stepText;

        public Cirrus.Event<Transform,ReplayButton> OnRemovedHandler;

        public Cirrus.Event OnAddedHandler;

        public Cirrus.Event<ReplayButton> OnReplayClickedHandler;


        private ButtonMode mode;

        public ButtonMode Mode
        {
            set
            {
                if (gameObject == null)
                    return;

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

            algorithmSelection.Algorithm.OnValueChangedHandler += (x) => Debug.Log("DSAD");

            Mode = mode;
        }

        public override void OnClick()
        {
            replay?.OnRoundReplayStartedHandler(replay);
        }

        public void OnRemoved()
        {
            replay.Remove();
            gameObject?.Destroy();
            OnRemovedHandler?.Invoke(transform.parent, this);
        }

        public virtual Labyrinths.UI.BaseButton Create(
            Transform parent,
            PreviewReplay replay)
        {
            return CreateReplayButton(parent, replay);
        }

        public ReplayButton CreateReplayButton(
            Transform parent,
            PreviewReplay replay)
        {
            var l = this.Create(parent);
            l.replay = replay;
            l.rawImage.texture = replay.LabyrinthObject.Camera.RenderTexture;
            l.roundNumberText.text = (replay.RoundNumber + 1).ToString();
            return l;
        }
    }


}