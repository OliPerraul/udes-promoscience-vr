using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Labyrinths.UI
{
    public class LevelSelectionInterface : BaseSelectionInterface//.UI.MainDisplay
    {
        private List<LevelButton> labyrinthButtons = new List<LevelButton>();

        [SerializeField]
        private Replays.ReplayControllerAsset replayController;

        protected Replays.ReplayControllerAsset ReplayController => replayController;

        //[SerializeField]
        //private LevelButton buttonTemplate;

        //public override BaseButton ButtonTemplate => buttonTemplate;

        [SerializeField]
        private LevelSection sectionTemplate;

        public override BaseSection SectionTemplate => sectionTemplate;

        [SerializeField]
        private UnityEngine.UI.Button buttonRandom;

        [SerializeField]
        private UnityEngine.UI.Button buttonExit;

        private List<LevelSection> sections = new List<LevelSection>();

        private LevelSection currentSection;

        public override void Awake()
        {
            base.Awake();

            replayController.OnActionHandler += OnReplayAction;

            Server.Instance.gameStateChangedEvent += OnServerGameStateChanged;

            buttonExit.onClick.AddListener(OnExitClicked);

            buttonRandom.onClick.AddListener(OnRandomClicked);
        }

        public void OnRandomClicked()
        {
            Server.Instance.StartGameWithLabyrinth(Random.Range(1, Utils.NumLabyrinth+1));// labyrinth.Id);
        }

        public void OnExitClicked()
        {
            Server.Instance.EndRoundOrTutorial();
        }


        public override bool Enabled
        {
            set
            {
                gameObject.SetActive(value);
            }
        }

        public override int NumSections => sections.Count;

        public override BaseSection CurrentSection => throw new System.NotImplementedException();

        public virtual void OnReplayAction(Replays.ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case Replays.ReplayAction.ExitReplay:

                    Enabled = true;
                    Server.Instance.GameState = ServerGameState.LevelSelect;

                    break;
            }
        }

        public override void AddLabyrinth(int i)
        {
            var data = Server.Instance.Labyrinths.Data[i];

            Labyrinth labyrinth = Resources.Instance
                  .GetLabyrinthTemplate(data)
                  .Create(data);

            labyrinths.Add(labyrinth);

            labyrinth.GenerateLabyrinthVisual();

            labyrinth.Init(enableCamera: true);

            labyrinth.Camera.OutputToTexture = true;

            labyrinth.transform.position = Vector3.right * Labyrinths.Utils.SelectionOffset * (labyrinths.Count - 1);

            AddSection().AddButton(labyrinth);

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

            //currentSection.OnButtonRemovedHandler += OnButtonRemoved;

            //currentSection.OnRemovedHandler += OnContainerRemoved;

            return currentSection;
        }



        public void OnReplayClicked(IData labyrinth)
        {
            Enabled = true;
            Enabled = false;

            Server.Instance.StartAdvancedReplay(labyrinth);
        }

        public void OnPlayClicked(IData labyrinth)
        {
            Enabled = true;
            Enabled = false;
            Server.Instance.StartGameWithLabyrinth(labyrinth.Id);
        }

        public override void Clear()
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


        public virtual void OnServerGameStateChanged()
        {
            switch (Server.Instance.GameState)
            {
                case ServerGameState.LevelSelect:

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
                        for (labyrinthIndex = 0; labyrinthIndex < Utils.NumLabyrinth; labyrinthIndex++)
                        {
                            AddLabyrinth(labyrinthIndex);
                        }
                    }

                    break;

                case ServerGameState.GameRound:
                case ServerGameState.Tutorial:

                    Enabled = false;
                    foreach (var lab in labyrinths)
                    {
                        lab.gameObject.SetActive(false);
                    }
                    break;

                default:
                    Enabled = false;
                    break;
            }
        }


    }
}
