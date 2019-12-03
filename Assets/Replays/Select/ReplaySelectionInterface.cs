using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;
using UdeS.Promoscience.Labyrinths;
using UdeS.Promoscience.Labyrinths.UI;

namespace UdeS.Promoscience.Replays.UI
{
    public class ReplaySelectionInterface : BaseSelectionInterface
    {
        [SerializeField]
        private ReplayManagerAsset replayController;

        protected ReplayManagerAsset ReplayController => replayController;

        [SerializeField]
        private ReplaySection sectionTemplate;

        public override BaseSection SectionTemplate => sectionTemplate;

        [SerializeField]
        private UnityEngine.UI.Button buttonAdd;


        private ReplaySection currentSection = null;

        public override BaseSection CurrentSection => currentSection;

        public List<ReplaySection> sections = new List<ReplaySection>();

        public override int NumSections => sections.Count;


        public override void Awake()
        {
            base.Awake();

            Server.Instance.State.OnValueChangedHandler += OnServerGameStateChanged;

            replayController.OnActionHandler += OnReplayAction;

            buttonAdd.onClick.AddListener(OnAddedBottomClicked);
        }

        public override void OnDestroy()
        {
            Server.Instance.State.OnValueChangedHandler -= OnServerGameStateChanged;

            replayController.OnActionHandler -= OnReplayAction;
        }

        public virtual void OnServerGameStateChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.ReplaySelect:

                    Enabled = true;

                    if (labyrinths.Count != 0)
                    {
                        foreach (var lab in labyrinths)
                        {
                            lab.gameObject.SetActive(true);
                            lab.Camera.OutputToTexture = true;
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

                case ServerState.LabyrinthReplay:

                    Enabled = false;
                    foreach (var lab in labyrinths)
                    {
                        lab.gameObject.SetActive(false);
                        lab.Camera.OutputToTexture = false;
                    }
                    break;

                case ServerState.Menu:
                    break;

                default:
                    Enabled = false;
                    break;
            }
        }

        public void OnExitClicked()
        {
            //Server.Instance.EndRoundOrTutorial();
        }


        public override bool Enabled
        {
            set => gameObject.SetActive(value);
        }


        public virtual void OnReplayAction(ReplayControlAction action, params object[] args)
        {
            //switch (action)
            //{
            //    case ReplayAction.ExitReplay:

            //        Enabled = true;
            //        Server.Instance.State.Value = ServerState.LevelSelect;

            //        break;
            //}
        }

        public override void Clear()
        {
            foreach (Transform children in buttonsParent)
            {
                if (children.gameObject.activeSelf) Destroy(children.gameObject);
            }

            //if (Server.Instance.Labyrinths.Labyrinths.Count != 0)
            //{
            //    //int i = 0;
            //    foreach (var l in Server.Instance.Labyrinths.Labyrinths)
            //    {
            //        if (l == null)
            //            continue;

            //        Destroy(l.gameObject);
            //    }
            //}

            //buttons.Clear();

            //Server.Instance.ClearLabyrinths();
        }


        public override void OnAddedBottomClicked()//Transform parent)
        {
            AddSection().AddButton(CreateNextLabyrinth());

            OnContentChangedHandler?.Invoke();
        }


        public override void AddLabyrinth(int i)
        {
            var data = GameManager.Instance.CurrentGame.Labyrinths[i];

            Labyrinth labyrinth = Labyrinths.Resources.Instance
                  .GetLabyrinthTemplate(data)
                  .Create(data);

            labyrinths.Add(labyrinth);

            labyrinth.GenerateLabyrinthVisual();

            labyrinth.Init(enableCamera: true);

            labyrinth.Camera.OutputToTexture = true;

            labyrinth.transform.position = Vector3.right * Labyrinths.Utils.SelectionOffset * (labyrinths.Count - 1);

            if (i % Labyrinths.Utils.SelectMaxHorizontal == 0)
            {
                AddSection();
            }

            sections[sections.Count - 1].AddButton(labyrinth);

            OnContentChangedHandler?.Invoke();
        }

        public override BaseSection AddSection()
        {
            if (sections.Count == 1)
            {
                sections[0].RespectLayout();
            }

            currentSection = sectionTemplate.Create(buttonsParent);

            sections.Add(currentSection);

            currentSection.gameObject.SetActive(true);

            currentSection.OnButtonRemovedHandler += OnButtonRemoved;

            currentSection.OnRemovedHandler += OnSectionRemoved;

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

        public void OnSectionRemoved(ReplaySection container)
        {
            sections.Remove(container);

            AdjustContent();

            OnContentChangedHandler?.Invoke();
        }
    }
}
