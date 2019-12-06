﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdeS.Promoscience.UI;

namespace UdeS.Promoscience.Replays.UI
{
    public class ReplaySidebar : MonoBehaviour
    {
        [SerializeField]
        protected UnityEngine.UI.Button overlayButton;

        [SerializeField]
        protected UnityEngine.UI.Button greyboxButton;

        [SerializeField]
        protected UnityEngine.UI.Button algorithmButton;

        [SerializeField]
        private UnityEngine.UI.Button openSidebarButton;

        [SerializeField]
        private UnityEngine.UI.Button closeSidebarButton;

        [SerializeField]
        private GameObject content;

        private LabyrinthReplay replay;


        public virtual void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnGameStateChanged;

            closeSidebarButton.onClick.AddListener(() =>
            {
                Enabled = false;
                openSidebarButton.gameObject.SetActive(true);
            });

            openSidebarButton.onClick.AddListener(() =>
            {
                Enabled = true;
                openSidebarButton.gameObject.SetActive(false);
            });

            Enabled = false;
        }

        public virtual void OnDestroy()
        {
            if (Server.Instance != null)
            {
                Server.Instance.State.OnValueChangedHandler -= OnGameStateChanged;
            }

            if (replay != null)
            {
                replay.OnMoveIndexChangedHandler -= OnMoveIndexChanged;
            }
        }

        public void OnMoveIndexChanged(int idx)
        {
            //if(course != null)
            //algorithmStepsText.text = course.Cu.ToString();
        }



        public void OnGameStateChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.LabyrinthReplay:
                    Enabled = true;
                    break;

                default:
                    Enabled = false;
                    break;
            }
        }

        private bool _enabled = false;

        public virtual bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                if (Server.Instance.State.Value != ServerState.LabyrinthReplay)
                    return;

                _enabled = value;
                ButtonManager.Instance.Flags.Value =
                    _enabled ?
                    ButtonCanvasFlag.None :
                    ButtonCanvasFlag.Exit;                        

                content.SetActive(_enabled);
            }
        }


    }
}