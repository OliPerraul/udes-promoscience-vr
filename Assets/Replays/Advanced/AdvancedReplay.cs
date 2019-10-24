using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using System.Collections.Generic;
using System.Linq;
using Cirrus.Extensions;

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

        

        public override void Awake()
        {
            base.Awake();

            Controller.OnActionHandler += OnReplayAction;
        }

        public virtual void OnReplayAction(ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case ReplayAction.ExitReplay:

                    int i = 0;
                    foreach (Labyrinths.Labyrinth l in AdvancedController.Labyrinths)
                    {
                        l.gameObject.SetActive(true);
                        SetLabyrinthCamera(l, i);
                        i++;
                    }

                    break;

                case ReplayAction.SelectLabyrinth:

                    Labyrinths.Labyrinth lab = (Labyrinths.Labyrinth) args[0];

                    foreach (Labyrinths.Labyrinth l in AdvancedController.Labyrinths)
                    {
                        l.gameObject.SetActive(false);
                    }

                    lab.gameObject.SetActive(true);

                    var cc = SQLiteUtilities.GetSessionCourses();
                    AdvancedController.Courses =  SQLiteUtilities.GetSessionCoursesForLabyrinth(lab.Id);

                    labyrinthReplay = new LabyrinthReplay(this, lab);

                    labyrinthReplay.Start();

                    break;
            }
        }

        public virtual void Clear()
        {
            if (AdvancedController.Labyrinths.Count != 0)
            {
                //int i = 0;
                foreach (var l in AdvancedController.Labyrinths)
                {
                    Destroy(l.gameObject);
                    
                }
            }

            AdvancedController.IdLabyrinthPairs.Clear();
        }

        public virtual void SetLabyrinthCamera(Labyrinths.Labyrinth labyrinth, int i)
        {
            labyrinth.SetCamera(
                AdvancedController.LabyrinthsData.Count,
                resource.MaxHorizontal,
                i);
        }


        public override void OnServerGameStateChanged()
        {
            if (ServerGame.Instance.GameState ==
                ServerGameState.AdvancedReplay)
            {
                Clear();

                int i = 0;
                foreach (var data in AdvancedController.LabyrinthsData)
                {
                    Labyrinths.Labyrinth labyrinth = LabyrinthResources.Labyrinth.Create(data);
                    labyrinth.GenerateLabyrinthVisual();
                    labyrinth.transform.position = Vector3.down * SelectionOffset * i;
                    //Vector3 offset = labyrinth.GetLabyrinthPositionInWorldPosition(0, 0);
                    //labyrinth.transform.position -= offset;
                    AdvancedController.IdLabyrinthPairs.Add(data.currentId, labyrinth);

                    SetLabyrinthCamera(labyrinth, i);

                    i++;
                }

                StartCoroutine(DelayedOnAdvancedReplay());
            }
            else
            {
                if (labyrinthReplay != null)
                {
                    labyrinthReplay.Clear();
                }

                foreach (var l in AdvancedController.Labyrinths)
                {
                    l.gameObject.Destroy();
                }

                AdvancedController.Clear();
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
