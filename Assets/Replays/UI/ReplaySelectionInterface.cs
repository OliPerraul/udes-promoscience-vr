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
        private ReplaySection sectionTemplate;

        public override BaseSection SectionTemplate => sectionTemplate;

        [SerializeField]
        private UnityEngine.UI.Button buttonAdd;

        private ReplaySection currentSection = null;

        public override BaseSection CurrentSection => currentSection;

        public List<ReplaySection> sections = new List<ReplaySection>();

        public override int NumSections => sections.Count;

        private GameReplay replay;

        public override void Awake()
        {
            base.Awake();

            Server.Instance.State.OnValueChangedHandler += OnServerGameStateChanged;

            buttonAdd.onClick.AddListener(() => AddBottomLabyrinth());
        }


        public override void OnDestroy()
        {
            if(
                Server.Instance != null && 
                Server.Instance.gameObject != null)
                Server.Instance.State.OnValueChangedHandler -= OnServerGameStateChanged;
        }

        public void AddInitialLabyrinths()
        {
            for (int i = 0; i < ReplayManager.Instance.GameReplay.Value.Levels.Count; i++)
            {
                if (i % Labyrinths.Utils.SelectMaxHorizontal == 0)
                {
                    AddInitialBottomLabyrinth(i);
                }
                else
                {
                    sections[sections.Count - 1].AddButton(ReplayManager.Instance.GameReplay.Value.Previews[i]);
                }
            }

            OnContentChangedHandler?.Invoke();
        }


        public virtual PreviewReplay CreateNextLabyrinth()
        {
            return ReplayManager.Instance.GameReplay.Value.CreatePreviewReplay();
        }

        public virtual void OnServerGameStateChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.GameReplay:

                    Enabled = true;

                    //ReplayManager.Instance.GameReplay.OnActionHandler += OnReplayAction;

                    AddInitialLabyrinths();

                    break;

                
                default:

                    Enabled = false;
                    

                    foreach (var sec in sections)
                    {
                        if (sec == null)
                            continue;

                        Destroy(sec.gameObject);

                    }

                    sections.Clear();

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

        public void AddInitialBottomLabyrinth(int i)
        {
            AddSection().AddButton(ReplayManager.Instance.GameReplay.Value.Previews[i]);

            OnContentChangedHandler?.Invoke();
        }


        public override void AddBottomLabyrinth()//Transform parent)
        {
            AddSection().AddButton(CreateNextLabyrinth());

            OnContentChangedHandler?.Invoke();
        }

        public virtual ReplaySection AddSection()
        {
            currentSection = sectionTemplate.Create(buttonsParent);

            sections.Add(currentSection);

            currentSection.gameObject.SetActive(true);

            currentSection.OnButtonRemovedHandler += OnButtonRemoved;

            currentSection.OnRemovedHandler += OnSectionRemoved;

            AdjustContent();

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
