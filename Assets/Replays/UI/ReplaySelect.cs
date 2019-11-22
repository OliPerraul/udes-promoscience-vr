using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;
using UdeS.Promoscience.Labyrinths;
using UdeS.Promoscience.Labyrinths.UI;

namespace UdeS.Promoscience.Replays.UI
{
    public class ReplaySelect : Labyrinths.UI.BaseSelect//.UI.MainDisplay
    {
        [SerializeField]
        private ControllerAsset replayController;

        protected ControllerAsset ReplayController => replayController;

        [SerializeField]
        private ReplayButton buttonTemplate;

        public override BaseButton ButtonTemplate => buttonTemplate;

        [SerializeField]
        private ReplaySection containerTemplate;

        public override BaseSection SectionTemplate => containerTemplate;



        [SerializeField]
        private UnityEngine.UI.Button buttonExit;


        [SerializeField]
        private UnityEngine.UI.Button buttonAdd;


        private ReplaySection currentSection = null;

        public override BaseSection CurrentSection => currentSection;

        public List<ReplaySection> sections = new List<ReplaySection>();

        public override int NumSections => sections.Count;


        public virtual void Awake()
        {
            Server.Instance.gameStateChangedEvent += OnServerGameStateChanged;

            replayController.OnActionHandler += OnReplayAction;

            buttonAdd.onClick.AddListener(OnAddedBottomClicked);
        }


        public virtual void OnServerGameStateChanged()
        {
            switch (Server.Instance.GameState)
            {
                case ServerGameState.ReplaySelect:

                    Enabled = true;

                    if (labyrinths.Count != 0)
                    {
                        foreach (var lab in labyrinths)
                        {
                            lab.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        for (labyrinthIndex = 0; labyrinthIndex < Labyrinths.Utils.NumLabyrinth; labyrinthIndex++)
                        {
                            AddLabyrinth(labyrinthIndex);
                        }
                    }

                    break;

                case ServerGameState.AdvancedReplay:
                    Enabled = false;
                    foreach (var lab in labyrinths)
                    {
                        lab.gameObject.SetActive(false);
                    }
                    break;

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

            labyrinth.Init();

            labyrinth.Camera.OutputToTexture = true;

            labyrinth.transform.position = Vector3.right * Labyrinths.Utils.SelectionOffset * (labyrinths.Count - 1);

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

            //buttons.Clear();

            Server.Instance.ClearLabyrinths();
        }


        public override void OnAddedBottomClicked()//Transform parent)
        {
            AddContainer().AddButton(CreateNextLabyrinth());

            OnContentChangedHandler?.Invoke();
        }


        public override void AddLabyrinth(int i)
        {
            var data = Server.Instance.Labyrinths.Data[i];

            Labyrinth labyrinth = Labyrinths.Resources.Instance
                  .GetLabyrinthTemplate(data)
                  .Create(data);

            labyrinths.Add(labyrinth);

            labyrinth.GenerateLabyrinthVisual();

            labyrinth.Init();

            labyrinth.Camera.OutputToTexture = true;

            labyrinth.transform.position = Vector3.right * Labyrinths.Utils.SelectionOffset * (labyrinths.Count - 1);

            if (i % Labyrinths.Utils.SelectMaxHorizontal == 0)
            {
                AddContainer();
            }

            sections[sections.Count - 1].AddButton(labyrinth);

            OnContentChangedHandler?.Invoke();
        }

        public override Labyrinths.UI.BaseSection AddContainer()
        {
            if (sections.Count == 1)
            {
                sections[0].RespectLayout();
            }

            currentSection = containerTemplate.Create(buttonsParent);

            sections.Add(currentSection);

            currentSection.gameObject.SetActive(true);

            currentSection.OnButtonRemovedHandler += OnButtonRemoved;

            currentSection.OnRemovedHandler += OnContainerRemoved;

            return currentSection;
        }

        public override void AdjustContent()
        {
            base.AdjustContent();

            if (sections.Count == 1) sections[0].Fit();

            else sections[0].RespectLayout();
        }

        public void OnButtonRemoved(ReplayButton button)
        {
            labyrinths.Remove(button.Labyrinth);

            for (int i = 0; i < labyrinths.Count; i++)
            {
                labyrinths[i].transform.position = Vector3.right * Labyrinths.Utils.SelectionOffset * i;
            }

            labyrinthIndex = labyrinths.Count;

            OnContentChangedHandler?.Invoke();
        }

        public void OnContainerRemoved(ReplaySection container)
        {
            sections.Remove(container);

            AdjustContent();

            OnContentChangedHandler?.Invoke();
        }

        public override void RemoveSection(BaseSection section)
        {
            //sections.Remove(section);
        }
    }
}
