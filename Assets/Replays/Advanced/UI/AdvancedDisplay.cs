﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays.Advanced.UI
{
    public class AdvancedDisplay : Replays.UI.MainDisplay
    {
        [SerializeField]
        public Resource resource;

        private List<LabyrinthButton> labyrinthButtons;

        protected override Replays.ScriptableController ReplayController
        {
            get
            {
                return replayController;
            }
        }

        [SerializeField]
        private ScriptableController replayController;

        [SerializeField]
        private Transform buttonsParent;

        [SerializeField]
        private LabyrinthButton labyrinthButtonTemplate;

        [SerializeField]
        private GameObject buttonsHorizontalTemplate;

        //private Replays.UI.LabyrinthDisplay labyrinthDisplay;


        public override void OnEnable()
        {
            if (init) return;

            base.OnEnable();

            labyrinthButtons = new List<LabyrinthButton>();

            replayController.OnAdvancedReplayHandler += OnAdvancedReplay;

            replayController.OnActionHandler += OnAction;

        }

        public void OnAction(ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case ReplayAction.ExitReplay:
                    Enabled = true;
                    break;
            }
        }

        public void OnClicked(Labyrinths.Labyrinth labyrinth)
        {
            labyrinthDisplay.Enabled = true;
            Enabled = false;

            replayController.SendAction(ReplayAction.SelectLabyrinth, labyrinth);
        }

        public void Clear()
        {
            foreach (Transform children in buttonsParent)
            {
                if (children.gameObject.activeSelf) Destroy(children.gameObject);
            }
        }

        public override void OnServerChangedState()
        {
            if (Server.Instance.GameState ==
                ServerGameState.AdvancedReplay)
            {

            }
        }

        public void OnAdvancedReplay()
        {
            Clear();

            GameObject horizontal = null;

            int i = 0;
            Labyrinths.Labyrinth l;
            foreach (var data in replayController.LabyrinthsData)
            {
                if (replayController.IdLabyrinthPairs.TryGetValue(data.currentId, out l))
                {
                    if (i % resource.MaxHorizontal == 0)
                    {
                        horizontal = buttonsHorizontalTemplate.Create(buttonsParent);
                        horizontal.gameObject.SetActive(true);
                    }

                    var button = labyrinthButtonTemplate.Create(horizontal.transform, l);
                    button.name = "btn " + i;
                    button.gameObject.SetActive(true);
                    labyrinthButtons.Add(button);
                    button.OnClickedHandler += OnClicked;

                    i++;
                }
            }
        }
    }
}