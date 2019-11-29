using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Labyrinths.UI;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Labyrinths.Editor.UI
{
    public class LabyrinthEditorSelect : BaseSelectionInterface
    {
        [SerializeField]
        private ControllerAsset controller;

        [SerializeField]
        private LabyrinthEditorSection sectionTemplate;

        public override BaseSection SectionTemplate => sectionTemplate;

        private List<LabyrinthEditorSection> sections = new List<LabyrinthEditorSection>();

        private LabyrinthEditorSection currentSection;

        public override BaseSection CurrentSection => currentSection;

        [SerializeField]
        private UnityEngine.UI.Button exitButton;

        public override int NumSections => sections.Count;

        public override void Awake()
        {
            base.Awake();

            controller.State.OnValueChangedHandler += OnStateChanged;
            exitButton.onClick.AddListener(OnExitClicked);
        }


        public void OnDestroy()
        {
            controller.State.OnValueChangedHandler -= OnStateChanged;
        }

        public void OnExitClicked()
        {

        }

        public virtual void OnStateChanged(State state)
        {
            switch (state)
            {
                case State.Select:

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

                case State.Editor:

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

        public override void AddLabyrinth(int i)
        {
            var data = Labyrinths.Resources.Instance.Labyrinths[i];

            Labyrinth labyrinth = Labyrinths.Resources.Instance
                  .GetLabyrinthTemplate(data)
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

            sections.Add(currentSection);

            currentSection.gameObject.SetActive(true);

            //currentSection.OnButtonRemovedHandler += OnButtonRemoved;

            //currentSection.OnRemovedHandler += OnContainerRemoved;

            return currentSection;
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

            //Server.Instance.ClearLabyrinths();
        }

    }
}
