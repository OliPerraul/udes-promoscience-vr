using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;
using UdeS.Promoscience.Labyrinths;

namespace UdeS.Promoscience.Replays.UI
{
    public class ReplaySelect : Labyrinths.UI.BaseLabyrinthSelect//.UI.MainDisplay
    {
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
        private ReplayButton labyrinthButtonTemplate;

        //private List<ReplayButton> buttons = new List<ReplayButton>();

        [SerializeField]
        private ButtonContainer containerTemplate;


        [SerializeField]
        private UnityEngine.UI.Button buttonExit;


        [SerializeField]
        private UnityEngine.UI.Button buttonAdd;


        private int labyrinthIndex = 0;

        public int LabyrinthIndexWrap => labyrinthIndex.Mod(Labyrinths.Resources.NumLabyrinths);


        private ButtonContainer horizontal = null;

        public List<ButtonContainer> horizontals = new List<ButtonContainer>();


        public virtual void Awake()
        {
            Server.Instance.gameStateChangedEvent += OnServerGameStateChanged;

            replayController.OnActionHandler += OnReplayAction;

            buttonAdd.onClick.AddListener(AddBottom);
        }


        public virtual void OnServerGameStateChanged()
        {
            if (
                Server.Instance.GameState ==
                ServerGameState.ReplaySelect)
            {
                Enabled = true;

                Clear();

                for(labyrinthIndex = 0; labyrinthIndex < Labyrinths.Utils.NumLabyrinth; labyrinthIndex++)
                {
                    AddLabyrinth(labyrinthIndex);
                }

            }
            else
            {
                Clear();
                Enabled = false;
            }
        }

        public Labyrinth CreateNextLabyrinth()
        {
            var data = Server.Instance.Labyrinths.Data[LabyrinthIndexWrap];

            labyrinthIndex++;

            Labyrinth labyrinth = Labyrinths.Resources.Instance
              .GetLabyrinthTemplate(data)
              .Create(data);

            labyrinths.Add(labyrinth);

            labyrinth.GenerateLabyrinthVisual();

            labyrinth.transform.position = Vector3.right * Labyrinths.Utils.SelectionOffset * labyrinths.Count;

            return labyrinth;
        }


        public void OnExitClicked()
        {
            Server.Instance.EndRoundOrTutorial();
        }


        public virtual bool Enabled
        {
            set => gameObject.SetActive(value);
        }


        public virtual void OnReplayAction(ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case ReplayAction.ExitReplay:

                    Enabled = true;
                    Server.Instance.GameState = ServerGameState.LabyrinthSelect;

                    break;
            }
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

            buttons.Clear();

            Server.Instance.ClearLabyrinths();
        }

        public void AddBottom()
        {
            AddHorizontal();
            AddNextLabyrinth();// horizontal.transform);
        }

        public void AddNextLabyrinth()//Transform parent)
        {
            //if (parent == null)
            //{
            //    AddBottom();
            //    return;
            //}

            //var data = Server.Instance.Labyrinths.Data[LabyrinthIndexWrap];

            //Labyrinth labyrinth = Labyrinths.Resources.Instance
            //  .GetLabyrinthTemplate(data)
            //  .Create(data);

            //labyrinths.Add(labyrinth);

            //labyrinth.GenerateLabyrinthVisual();

            //labyrinth.transform.position = Vector3.right * Labyrinths.Utils.SelectionOffset * labyrinths.Count;

            //AddLabyrinthButton(parent, LabyrinthIndexWrap, labyrinth);

            //labyrinthIndex++;
        }


        public void AddLabyrinth(int i)
        {
            var data = Server.Instance.Labyrinths.Data[i];

            Labyrinth labyrinth = Labyrinths.Resources.Instance
                  .GetLabyrinthTemplate(data)
                  .Create(data);

            labyrinths.Add(labyrinth);

            labyrinth.GenerateLabyrinthVisual();

            labyrinth.Init();

            labyrinth.transform.position = Vector3.right * Labyrinths.Utils.SelectionOffset * labyrinths.Count;

            if (i % Labyrinths.Utils.SelectMaxHorizontal == 0)
            {
                AddHorizontal();
            }

            horizontals[horizontals.Count-1].AddLabyrinth(labyrinth);
        }

        public void AddHorizontal()
        {
            if (horizontals.Count == 1)
            {
                horizontals[0].GetComponent<ButtonContainer>()?.RespectLayout();
            }

            horizontal = containerTemplate.Create(buttonsParent);

            horizontals.Add(horizontal);

            horizontal.gameObject.SetActive(true);
        }

        public void RemoveHorizontal(GameObject horizontal)
        {
            if (horizontal.GetComponentsInChildren<ReplayButton>(false).Length == 1)
            {
                horizontal?.gameObject?.Destroy();
                horizontals.Remove(horizontal.gameObject);
            }

            if (horizontals.Count == 1)
            {
                horizontals[0].GetComponent<ButtonContainer>()?.Fit();
            }
        }

        public void AddLabyrinthButton(ButtonContainer parent, Labyrinth labyrinth)
        {
            parent.AddLabyrinth(labyrinth);
        }


        public void OnLabyrinthRemoved(Transform parent, ReplayButton button)
        {
            labyrinths.Remove(button.Labyrinth);

            for (int labyrinthIndex = 0; labyrinthIndex < labyrinths.Count; labyrinthIndex++)
            {
                labyrinths[labyrinthIndex].transform.position = Vector3.right * Labyrinths.Utils.SelectionOffset * labyrinthIndex;
            }

            buttons.Remove(button);

            RemoveHorizontal(parent.gameObject);

            if (buttons.Count == 1)
            {
                buttons[0].Mode = ReplayButtonMode.Add;
            }
        }

    }
}
