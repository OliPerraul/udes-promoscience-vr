using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Labyrinths.UI
{
    public abstract class BaseSelect : MonoBehaviour
    {
        protected List<Labyrinth> labyrinths = new List<Labyrinth>();

        [SerializeField]
        protected Transform buttonsParent;

        public Cirrus.Event OnContentChangedHandler;

        protected BaseSection currentSection;

        public abstract BaseSection SectionTemplate { get; }

        public abstract BaseButton ButtonTemplate { get; }


        public int labyrinthIndex = 0;

        public int LabyrinthIndexWrap => labyrinthIndex.Mod(Labyrinths.Resources.NumLabyrinths);

        //public abstract List<BaseSection> Sections { get; }


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


        //public void OnExitClicked()
        //{
        //    Server.Instance.EndRoundOrTutorial();
        //}


        public virtual bool Enabled
        {
            set => gameObject.SetActive(value);
        }


        //public virtual void OnReplayAction(ReplayAction action, params object[] args)
        //{
        //    switch (action)
        //    {
        //        case ReplayAction.ExitReplay:

        //            Enabled = true;
        //            Server.Instance.GameState = ServerGameState.LabyrinthSelect;

        //            break;
        //    }
        //}

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


        public virtual void OnAddedBottomClicked()//Transform parent)
        {
            AddContainer().AddButton(CreateNextLabyrinth());

            OnContentChangedHandler?.Invoke();
        }


        public virtual void AddLabyrinth(int i)
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

            if (i % Utils.SelectMaxHorizontal == 0)
            {
                AddContainer();
            }

            //Sections[Sections.Count - 1].AddButton(labyrinth);

            OnContentChangedHandler?.Invoke();
        }

        public virtual BaseSection AddContainer()
        {
            if (NumSections == 1)
            {
                CurrentSection.RespectLayout();
            }

            currentSection = SectionTemplate.Create(buttonsParent);

            //Sections.Add(currentSection);

            currentSection.gameObject.SetActive(true);

            //currentSection.OnButtonRemovedHandler += OnButtonRemoved;

            //currentSection.OnRemovedHandler += OnContainerRemoved;

            AdjustContent();

            OnContentChangedHandler?.Invoke();

            return currentSection;
        }

        public abstract int NumSections { get; }

        public virtual void AdjustContent()
        {
            if (NumSections == 1) CurrentSection.Fit();

            else CurrentSection.RespectLayout();
        }

        public void OnButtonRemoved(BaseButton button)
        {
            labyrinths.Remove(button.Labyrinth);

            for (int i = 0; i < labyrinths.Count; i++)
            {
                labyrinths[i].transform.position = Vector3.right * Labyrinths.Utils.SelectionOffset * i;
            }

            labyrinthIndex = labyrinths.Count;

            OnContentChangedHandler?.Invoke();
        }

        public void OnContainerRemoved(BaseSection container)
        {
            RemoveSection(container);

            AdjustContent();

            OnContentChangedHandler?.Invoke();
        }

        public abstract BaseSection CurrentSection { get; }

        public abstract void RemoveSection(BaseSection section);

    }
}
