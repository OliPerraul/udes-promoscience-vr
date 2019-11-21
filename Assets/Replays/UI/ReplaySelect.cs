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

        public Cirrus.Event OnContentChangedHandler;

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


        private ButtonContainer container = null;

        public List<ButtonContainer> containers = new List<ButtonContainer>();

        public int NumContainers => containers.Count;


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


        public void OnAddedBottomClicked()//Transform parent)
        {
            AddContainer().AddButton(CreateNextLabyrinth());

            OnContentChangedHandler?.Invoke();
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

            labyrinth.Camera.OutputToTexture = true;

            labyrinth.transform.position = Vector3.right * Labyrinths.Utils.SelectionOffset * (labyrinths.Count - 1);

            if (i % Labyrinths.Utils.SelectMaxHorizontal == 0)
            {
                AddContainer();
            }

            containers[containers.Count - 1].AddButton(labyrinth);

            OnContentChangedHandler?.Invoke();
        }

        public ButtonContainer AddContainer()
        {
            if (containers.Count == 1)
            {
                containers[0].GetComponent<ButtonContainer>()?.RespectLayout();
            }

            container = containerTemplate.Create(buttonsParent);

            containers.Add(container);

            container.gameObject.SetActive(true);

            container.OnButtonRemovedHandler += OnButtonRemoved;

            container.OnRemovedHandler += OnContainerRemoved;

            AdjustContent();

            OnContentChangedHandler?.Invoke();

            return container;
        }

        public void AdjustContent()
        {
            if (containers.Count == 1) containers[0].Fit();

            else containers[0].RespectLayout();
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

        public void OnContainerRemoved(ButtonContainer container)
        {
            containers.Remove(container);

            AdjustContent();

            OnContentChangedHandler?.Invoke();
        }
    }
}
