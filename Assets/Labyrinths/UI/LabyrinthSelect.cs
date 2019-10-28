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

        [SerializeField]
        private UnityEngine.UI.Button buttonRandom;

        [SerializeField]
        private UnityEngine.UI.Button buttonExit;


        private bool init = false;

        public virtual void OnEnable()
        {
            if (init) return;

            init = true;

            labyrinthButtons = new List<LabyrinthButton>();

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
            }
        }


        public void OnReplayClicked(Labyrinth labyrinth)
        {
            Enabled = true;
            Enabled = false;

            Server.Instance.BeginAdvancedReplay(labyrinth);
        }

        public void OnPlayClicked(Labyrinth labyrinth)
        {
            Enabled = true;
            Enabled = false;
            Server.Instance.StartGameWithLabyrinth(labyrinth.Id);
        }

        public virtual void Clear()
        {
            if (Server.Instance.CurrentReplay != null)
            {
                Server.Instance.CurrentReplay.Clear();
            }

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

            Server.Instance.ClearLabyrinths();
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
                Enabled = true;

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
                //Clear();
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
