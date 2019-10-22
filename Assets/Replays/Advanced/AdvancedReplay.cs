using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using System.Collections.Generic;
using System.Linq;

namespace UdeS.Promoscience.Replays.Advanced
{
    public class AdvancedReplay : Replay
    {
        [SerializeField]
        private Resource resource;
               
        [SerializeField]
        public ScriptableController controller;

        public override Replays.ScriptableController Controller
        {
            get
            {
                return AdvancedController;
            }
        }

        [SerializeField]
        public float SelectionOffset = 60;

        private LabyrinthReplay labyrinthReplay;

        protected ScriptableController AdvancedController
        {
            get
            {
                return controller;
            }
        }

        private IEnumerable<Course> courses;

        public override void Awake()
        {
            base.Awake();

            Controller.OnActionHandler += OnReplayAction;
        }

        public virtual void OnReplayAction(ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case ReplayAction.ToggleLabyrinth:

                    Labyrinths.Labyrinth lab = (Labyrinths.Labyrinth) args[0];

                    foreach (Labyrinths.Labyrinth l in AdvancedController.Labyrinths)
                    {
                        l.gameObject.SetActive(false);
                    }

                    lab.gameObject.SetActive(true);

                    courses =  SQLiteUtilities.GetSessionCoursesForLabyrinth(lab.Id);

                    labyrinthReplay = new LabyrinthReplay(this, lab, courses);

                    labyrinthReplay.Start();

                    break;
            }
        }

        public virtual void Clear()
        {
            if (AdvancedController.Labyrinths.Count != 0)
            {
                foreach (var l in AdvancedController.Labyrinths)
                {
                    Destroy(l.gameObject);
                }
            }

            AdvancedController.IdLabyrinthPairs.Clear();
        }


        public override void OnServerGameStateChanged()
        {
            if (Server.GameState ==
                ServerGameState.AdvancedReplay)
            {
                Clear();

                int i = 0;
                foreach(var data in AdvancedController.LabyrinthsData)
                {
                    Labyrinths.Labyrinth labyrinth = LabyrinthResources.Labyrinth.Create(data);
                    labyrinth.GenerateLabyrinthVisual();
                    labyrinth.transform.position = Vector3.down * SelectionOffset * i;
                    Vector3 offset = labyrinth.GetLabyrinthPositionInWorldPosition(0, 0);
                    labyrinth.transform.position -= offset;
                    AdvancedController.IdLabyrinthPairs.Add(data.currentId, labyrinth);

                    labyrinth.SetCamera(
                        AdvancedController.LabyrinthsData.Count, 
                        resource.MaxHorizontal, 
                        SelectionOffset, 
                        i);

                    i++;
                }

                StartCoroutine(DelayedOnAdvancedReplay());
            }
        }

        public IEnumerator DelayedOnAdvancedReplay()
        {
            yield return new WaitForEndOfFrame();

            if (AdvancedController.OnAdvancedReplayHandler != null)
            {
                AdvancedController.OnAdvancedReplayHandler.Invoke();
            }

            yield return null;
        }

    }
}
