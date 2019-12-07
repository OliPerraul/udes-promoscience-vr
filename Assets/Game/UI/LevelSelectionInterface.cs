using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;
using UdeS.Promoscience.UI;
using UdeS.Promoscience.Replays;

namespace UdeS.Promoscience.Labyrinths.UI
{
    public class LevelSelectionInterface : BaseSelectionInterface//.UI.MainDisplay
    {
        private List<LevelButton> labyrinthButtons = new List<LevelButton>();

        [SerializeField]
        private Algorithms.UI.LevelAlgorithmSelectionInteface algorithmSelect;

        [SerializeField]
        private LevelSection sectionTemplate;

        public override BaseSection SectionTemplate => sectionTemplate;

        private List<LevelSection> sections = new List<LevelSection>();

        private LevelSection currentSection;

        public override void Awake()
        {
            base.Awake();

            Server.Instance.State.OnValueChangedHandler += OnServerGameStateChanged;

            ButtonManager.Instance.RandomButton.onClick.AddListener(OnRandomClicked);

        }

        public override void OnDestroy()
        {
            if(Server.Instance != null && Server.Instance.gameObject != null) Server.Instance.State.OnValueChangedHandler -= OnServerGameStateChanged;
        }

        public void OnRandomClicked()
        {
            if (Server.Instance.State.Value != ServerState.LevelSelect)
                return;

            GameManager.Instance.CurrentGame.StartNextRound(
                Random.Range(1, Utils.NumLabyrinth + 1), 
                (int)algorithmSelect.AlgorithmId);
        }

        public void OnButtonClicked(BaseButton button)
        {
            Debug.Log(algorithmSelect.AlgorithmId);

            GameManager.Instance.CurrentGame.StartNextRound(
                button.Labyrinth.Data.Id,
                (int)algorithmSelect.AlgorithmId);
        }



        // On exit click (cancel game if round not started, or do not finish the round)

        //public void OnExitClicked()
        //{
        //    Server.Instance.EndRoundOrTutorial();
        //}


        public override bool Enabled
        {
            set { if (gameObject != null) gameObject.SetActive(value); }
        }
         

        public override int NumSections => sections.Count;

        public override BaseSection CurrentSection => throw new System.NotImplementedException();

        public override void AddLabyrinth(int i)
        {
            var data = Resources.Instance.Labyrinths[i];

            LabyrinthObject labyrinth = Resources.Instance
                  .GetLabyrinthObject(data)
                  .Create(data);

            labyrinths.Add(labyrinth);

            labyrinth.GenerateLabyrinthVisual();

            labyrinth.Init(enableCamera: true);

            labyrinth.Camera.OutputToTexture = true;

            labyrinth.transform.position = Vector3.right * Utils.SelectionOffset * (labyrinths.Count - 1);

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

            currentSection.gameObject.SetActive(true);

            currentSection.OnButtonClickHandler += OnButtonClicked;

            sections.Add(currentSection);



            //currentSection.OnButtonRemovedHandler += OnButtonRemoved;

            //currentSection.OnRemovedHandler += OnContainerRemoved;

            return currentSection;
        }

        public void OnPlayClicked(ILabyrinth labyrinth)
        {
            Enabled = true;
            Enabled = false;
            //GameManager.Instance.CurrentGame.StartRoundWithLabyrinth(labyrinth.Id);
        }

        public override void Clear()
        {
            foreach (Transform children in buttonsParent)
            {
                if (children.gameObject.activeSelf) Destroy(children.gameObject);
            }
        }


        public virtual void OnServerGameStateChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.LevelSelect:

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


                default:
                    Enabled = false;
                    foreach (var lab in labyrinths)
                    {
                        lab.gameObject.Destroy();
                    }

                    labyrinths.Clear();


                    foreach (var sect in sections)
                    {
                        sect.gameObject.Destroy();
                    }

                    sections.Clear();

                    break;
            }
        }


    }
}
