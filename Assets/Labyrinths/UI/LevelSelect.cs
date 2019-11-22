using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Labyrinths.UI
{
    public class LevelSelect : BaseSelect//.UI.MainDisplay
    {

        private List<LevelButton> labyrinthButtons;

        [SerializeField]
        private Replays.ControllerAsset replayController;

        protected Replays.ControllerAsset ReplayController => replayController;

        [SerializeField]
        private LevelButton labyrinthButtonTemplate;

        public override BaseButton ButtonTemplate => labyrinthButtonTemplate;

        [SerializeField]
        private GameObject buttonsHorizontalTemplate;

        [SerializeField]
        private LevelSection sectionTemplate;

        public override BaseSection SectionTemplate => sectionTemplate;

        [SerializeField]
        private UnityEngine.UI.Button buttonRandom;

        [SerializeField]
        private UnityEngine.UI.Button buttonExit;


        
        public virtual void Awake()
        {

            labyrinthButtons = new List<LevelButton>();

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


        public virtual bool Enabled
        {
            set
            {
                gameObject.SetActive(value);
            }
        }

        public override int NumSections => throw new System.NotImplementedException();

        public override BaseSection CurrentSection => throw new System.NotImplementedException();

        public virtual void OnReplayAction(Replays.ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case Replays.ReplayAction.ExitReplay:

                    Enabled = true;
                    Server.Instance.GameState = ServerGameState.LabyrinthSelect;

                    break;
            }
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


        //public virtual void SetLabyrinthCamera(Labyrinth labyrinth, int i)
        //{
        //    labyrinth.SetCamera(
        //        Server.Instance.Labyrinths.Data.Count,
        //        Utils.SelectMaxHorizontal,
        //        i);
        //}


        public virtual void OnServerGameStateChanged()
        {
            if (
                Server.Instance.GameState ==
                ServerGameState.LabyrinthSelect)
            {
                Enabled = true;

                Clear();

                GameObject horizontal = null;

                for (int i = 0; i < Utils.NumLabyrinth; i++)
                {

                    var data = Server.Instance.Labyrinths.Data[i];

                    Labyrinth labyrinth = Resources.Instance
                          .GetLabyrinthTemplate(data)
                          .Create(data);

                    labyrinth.GenerateLabyrinthVisual();

                    labyrinth.Init();

                    labyrinth.transform.position = Vector3.right * Labyrinths.Utils.SelectionOffset * i;

                    Server.Instance.Labyrinths.IdPairs.Add(data.Id, labyrinth);

                    if (i % Utils.SelectMaxHorizontal == 0)
                    {
                        horizontal = buttonsHorizontalTemplate.Create(buttonsParent);
                        horizontal.gameObject.SetActive(true);
                    }

                    var button = labyrinthButtonTemplate.Create(
                        horizontal.transform,
                        labyrinth);


                    button.name = "btn " + i;

                    button.gameObject.SetActive(true);

                    //labyrinthButtons.Add(button);
                }

            }
            else
            {                
                Clear();
                Enabled = false;
            }
        }

        public override void RemoveSection(BaseSection section)
        {
            throw new System.NotImplementedException();
        }
    }
}
