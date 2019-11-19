﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays.UI
{
    public class ReplaySelect : MonoBehaviour//.UI.MainDisplay
    {
        [SerializeField]
        private int maxHorizontal = 2;

        [SerializeField]
        public float SelectionOffset = 60;

        [SerializeField]
        public Labyrinths.UI.Resource resource;

        private List<LabyrinthPanel> labyrinthPanel;

        protected ControllerAsset ReplayController
        {
            get
            {
                return replayController;
            }
        }

        [SerializeField]
        private ControllerAsset replayController;

        [SerializeField]
        private Transform buttonsParent;

        [SerializeField]
        private LabyrinthPanel labyrinthButtonTemplate;

        [SerializeField]
        private GameObject buttonsHorizontalTemplate;

        [SerializeField]
        private UnityEngine.UI.Button buttonRandom;

        [SerializeField]
        private UnityEngine.UI.Button buttonExit;


        public virtual void Awake()
        {
            labyrinthPanel = new List<LabyrinthPanel>();

            replayController.OnActionHandler += OnReplayAction;

            Server.Instance.gameStateChangedEvent += OnServerGameStateChanged;

            buttonExit.onClick.AddListener(OnExitClicked);

            buttonRandom.onClick.AddListener(OnRandomClicked);
        }

        public void OnRandomClicked()
        {
            Server.Instance.StartGameWithLabyrinth(
                Random.Range(
                    1, 
                    Labyrinths.Utils.NumLabyrinth + 1));
        }

        public void OnExitClicked()
        {
            Server.Instance.EndRoundOrTutorial();
        }


        public virtual bool Enabled
        {
            set => gameObject.SetActive(value);
        }


        public virtual void OnReplayAction(Replays.ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case ReplayAction.ExitReplay:

                    Enabled = true;
                    Server.Instance.GameState = ServerGameState.LabyrinthSelect;

                    break;
            }
        }


        public void OnReplayClicked(Labyrinths.IData labyrinth)
        {
            Enabled = true;
            Enabled = false;

            Server.Instance.StartAdvancedReplay(labyrinth);
        }

        public void OnPlayClicked(Labyrinths.IData labyrinth)
        {
            Enabled = true;
            Enabled = false;
            Server.Instance.StartGameWithLabyrinth(labyrinth.Id);
        }

        public virtual void Clear()
        {
            foreach (Transform children in buttonsParent)
            {
                if (children.gameObject.activeSelf) Destroy(children.gameObject);
            }

            if (Server.Instance.Labyrinths.Labyrinths.Count != 0)
            {
                //int i = 0;
                foreach (var l in Server.Instance.Labyrinths.Labyrinths)
                {
                    if (l == null)
                        continue;

                    Destroy(l.gameObject);
                }
            }

            Server.Instance.ClearLabyrinths();
        }


        public virtual void SetLabyrinthCamera(Labyrinths.Labyrinth labyrinth, int i)
        {
            labyrinth.SetCamera(
                Server.Instance.Labyrinths.Data.Count,
                maxHorizontal,
                i);
        }


        public virtual void OnServerGameStateChanged()
        {
            if (
                Server.Instance.GameState ==
                ServerGameState.LabyrinthSelect)
            {
                Enabled = true;

                Clear();

                int i = 0;
                foreach (var data in Server.Instance.Labyrinths.Data)
                {
                    Labyrinths.Labyrinth labyrinth = Labyrinths.Resources.Instance
                        .GetLabyrinthTemplate(data)
                        .Create(data);

                    labyrinth.GenerateLabyrinthVisual();

                    labyrinth.Init();

                    labyrinth.transform.position = Vector3.down * SelectionOffset * i;

                    Server.Instance.Labyrinths.IdPairs.Add(data.Id, labyrinth);

                    SetLabyrinthCamera(labyrinth, i);

                    i++;
                }
            }
            else
            {
                Clear();
                Enabled = false;
            }
        }


        private int labyrinthIndex = 0;

        GameObject horizontal = null;

        public void AddLabyrinth()
        {
            var data = Server.Instance.Labyrinths.Data[labyrinthIndex];

            if (labyrinthIndex % maxHorizontal == 0)
            {
                horizontal = buttonsHorizontalTemplate.Create(buttonsParent);
                horizontal.gameObject.SetActive(true);
            }

            List<Course> courses = SQLiteUtilities.GetSessionCoursesForLabyrinth(data.Id);

            var button = labyrinthButtonTemplate.Create(horizontal.transform, data, courses.Count == 0);
            button.name = "btn " + labyrinthIndex;
            button.gameObject.SetActive(true);
            labyrinthPanel.Add(button);

            labyrinthIndex++;
        }
    }
}
