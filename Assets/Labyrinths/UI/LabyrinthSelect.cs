using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Labyrinths.UI
{
    public class LabyrinthSelect : MonoBehaviour//.UI.MainDisplay
    {
        [SerializeField]
        public float SelectionOffset = 60;

        [SerializeField]
        public Resource resource;

        private List<LabyrinthButton> labyrinthButtons;

        protected Replays.ScriptableController ReplayController
        {
            get
            {
                return replayController;
            }
        }

        [SerializeField]
        private Replays.ScriptableController replayController;

        [SerializeField]
        private Transform buttonsParent;

        [SerializeField]
        private LabyrinthButton labyrinthButtonTemplate;

        [SerializeField]
        private GameObject buttonsHorizontalTemplate;


        private bool init = false;

        public virtual void OnEnable()
        {
            if (init) return;

            init = true;

            labyrinthButtons = new List<LabyrinthButton>();

            replayController.OnActionHandler += OnReplayAction;
            Server.Instance.gameStateChangedEvent += OnServerGameStateChanged;
        }


        public virtual bool Enabled
        {
            set
            {
                gameObject.SetActive(value);
            }
        }


        public virtual void OnReplayAction(Replays.ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case Replays.ReplayAction.ExitReplay:

                    Enabled = true;

                    int i = 0;
                    foreach (Labyrinth l in Server.Instance.Labyrinths)
                    {
                        l.gameObject.SetActive(true);
                        SetLabyrinthCamera(l, i);
                        i++;
                    }

                    break;

                case Replays.ReplayAction.SelectLabyrinth:

                    Labyrinth lab = (Labyrinth)args[0];

                    foreach (Labyrinth l in Server.Instance.Labyrinths)
                    {
                        l.gameObject.SetActive(false);
                    }

                    lab.gameObject.SetActive(true);

                    replayController.Courses = SQLiteUtilities.GetSessionCourses();// ForLabyrinth(lab.Id);

                    Server.Instance.CurrentReplay = new Replays.LabyrinthReplay(replayController, lab);

                    Server.Instance.CurrentReplay.Start();

                    break;
            }
        }


        public void OnReplayClicked(Labyrinth labyrinth)
        {
            Enabled = true;
            Enabled = false;

            replayController.SendAction(Replays.ReplayAction.SelectLabyrinth, labyrinth);
        }

        public void OnPlayClicked(Labyrinth labyrinth)
        {
            Enabled = true;
            Enabled = false;
        }

        public virtual void Clear()
        {
            foreach (Transform children in buttonsParent)
            {
                if (children.gameObject.activeSelf) Destroy(children.gameObject);
            }

            if (Server.Instance.Labyrinths.Count != 0)
            {
                //int i = 0;
                foreach (var l in Server.Instance.Labyrinths)
                {
                    if (l == null)
                        continue;

                    Destroy(l.gameObject);
                }
            }
        }


        public virtual void SetLabyrinthCamera(Labyrinths.Labyrinth labyrinth, int i)
        {
            labyrinth.SetCamera(
                Server.Instance.LabyrinthsData.Count,
                resource.MaxHorizontal,
                i);
        }


        public virtual void OnServerGameStateChanged()
        {
            if (Server.Instance.GameState ==
                ServerGameState.LabyrinthSelect)
            {
                Clear();

                int i = 0;
                foreach (var data in Server.Instance.LabyrinthsData)
                {
                    Labyrinth labyrinth = ScriptableResources.Instance
                        .GetLabyrinth(data)
                        .Create(data);

                    labyrinth.GenerateLabyrinthVisual();
                    labyrinth.transform.position = Vector3.down * SelectionOffset * i;
                    Server.Instance.IdLabyrinthPairs.Add(data.currentId, labyrinth);

                    SetLabyrinthCamera(labyrinth, i);

                    i++;
                }

                StartCoroutine(DelayedOnLabyrinthSelect());
            }
            else
            {
                if (Server.Instance.CurrentReplay != null)
                {
                    Server.Instance.CurrentReplay.Clear();
                }

                foreach (var l in Server.Instance.Labyrinths)
                {
                    l.gameObject.Destroy();
                }

                Clear();
            }
        }

        public IEnumerator DelayedOnLabyrinthSelect()
        {
            yield return new WaitForEndOfFrame();

            GameObject horizontal = null;

            int i = 0;
            Labyrinth l;
            foreach (var data in Server.Instance.LabyrinthsData)
            {
                if (Server.Instance.IdLabyrinthPairs.TryGetValue(data.currentId, out l))
                {
                    if (i % resource.MaxHorizontal == 0)
                    {
                        horizontal = buttonsHorizontalTemplate.Create(buttonsParent);
                        horizontal.gameObject.SetActive(true);
                    }

                    var courses = SQLiteUtilities.GetSessionCoursesForLabyrinth(data.currentId);

                    var button = labyrinthButtonTemplate.Create(horizontal.transform, l, courses.Count == 0);
                    button.name = "btn " + i;
                    button.gameObject.SetActive(true);
                    labyrinthButtons.Add(button);
                    button.OnReplayClickedHandler += OnReplayClicked;
                    button.OnPlayClickedHandler += OnPlayClicked;

                    i++;
                }
            }

            yield return null;
        }
    }
}
