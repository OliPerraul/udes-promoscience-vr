using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Labyrinths.UI;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Labyrinths.Editor.UI
{
    public class EditorSelectionInterface : BaseSelectionInterface
    {
        //[SerializeField]
        //private ControllerAsset controller;

        [SerializeField]
        private EditorSelectSection sectionTemplate;

        public override BaseSection SectionTemplate => sectionTemplate;

        private List<EditorSelectSection> sections = new List<EditorSelectSection>();

        private EditorSelectSection currentSection;

        public override BaseSection CurrentSection => currentSection;

        [SerializeField]
        private UnityEngine.UI.Button exitButton;

        [SerializeField]
        private UnityEngine.UI.Button addButton;

        public override int NumSections => sections.Count;

        public override void Awake()
        {
            base.Awake();

            EditorController.Instance.State.OnValueChangedHandler += OnStateChanged;

            exitButton.onClick.AddListener(OnExitClicked);
            addButton.onClick.AddListener(AddLabyrinth);
            
        }


        public void OnDestroy()
        {
            if (EditorController.Instance != null)
            EditorController.Instance.State.OnValueChangedHandler -= OnStateChanged;
        }

        public void OnExitClicked()
        {

        }

        public virtual void OnStateChanged(EditorState state)
        {
            switch (state)
            {
                case EditorState.Select:

                    Enabled = true;

                    //if (labyrinths.Count != 0)
                    //{
                    //    foreach (var lab in labyrinths)
                    //    {
                    //        lab.gameObject.SetActive(true);
                    //    }
                    //}
                    //else
                    {
                        for (labyrinthIndex = 0; labyrinthIndex < Utils.NumLabyrinth; labyrinthIndex++)
                        {
                            AddLabyrinth(labyrinthIndex);
                        }
                    }

                    break;

                case EditorState.Editor:

                    foreach (var lab in labyrinths)
                    {
                        if (lab == null)
                            continue;

                        lab.gameObject.Destroy();
                    }

                    labyrinths.Clear();

                    foreach (var lab in sections)
                    {
                        if (lab == null)
                            continue;

                        lab.gameObject.Destroy();
                    }

                    sections.Clear();

                    Enabled = false;
                    break;

                default:
                    
                    Enabled = false;
                    break;
            }
        }

        public override void AddLabyrinth(int i)
        {
            var data = Server.Instance.Labyrinths[i];

            LabyrinthObject labyrinth = Labyrinths.Resources.Instance
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

        public void AddLabyrinth()
        {
            var data = SQLiteUtilities.CreateSampleLabyrinth();

            Server.Instance.Labyrinths.Add(data);

            LabyrinthObject labyrinth = Labyrinths.Resources.Instance
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
