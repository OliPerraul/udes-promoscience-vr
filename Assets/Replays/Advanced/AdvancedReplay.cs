using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replays.Advanced
{
    public class AdvancedReplay : Replay
    {
        [SerializeField]
        private Resource resource;

        [SerializeField]
        private ScriptableController replayController;

        protected override Replays.ScriptableController ReplayController
        {
            get
            {
                return replayController;
            }
        }

        [SerializeField]
        private Algorithm algorithm;

        public override void OnEnable()
        {
            base.OnEnable();

            if (init) return;

            base.OnEnable();
        }

        public override void OnReplayAction(ReplayAction action, params object[] args)
        {
            base.OnReplayAction(action, args);

            switch (action)
            {
                case ReplayAction.ToggleLabyrinth:

                    Labyrinths.Labyrinth lab = (Labyrinths.Labyrinth) args[0];

                    foreach (Labyrinths.Labyrinth l in replayController.Labyrinths)
                    {
                        l.gameObject.SetActive(false);
                    }

                    lab.gameObject.SetActive(true);
                    lab.Camera.Maximize();

                    break;

                // TODO: Handle play/ stop from replay object and not sequences
                // to prevent synch issues
                case ReplayAction.ToggleAlgorithm:

                    break;

                case ReplayAction.Play:


                    break;

                case ReplayAction.Resume:

                    break;

                case ReplayAction.Pause:

                    //mutex.WaitOne();

                    //Pause();

                    //mutex.ReleaseMutex();

                    break;

                case ReplayAction.Slide:



                    break;


                case ReplayAction.Next:



                    break;

                case ReplayAction.Previous:



                    break;

                case ReplayAction.Stop:

                    //mutex.WaitOne();

                    //Stop();

                    //mutex.ReleaseMutex();

                    break;
            }
        }

        public virtual void Clear()
        {
            if (replayController.Labyrinths.Count != 0)
            {
                foreach (var l in replayController.Labyrinths)
                {
                    Destroy(l.gameObject);
                }
            }

            replayController.LabyrinthIdPairs.Clear();
        }

        public override void OnServerGameStateChanged()
        {
            if (server.GameState ==
                ServerGameState.FinalReplay)
            {
                Clear();

                var labyrinthData = SQLiteUtilities.GetLabyrinths();

                for (int i = 0; i < labyrinthData.Count; i++)
                {
                    Labyrinths.Labyrinth labyrinth = resources.Labyrinth.Create(transform, labyrinthData[i]);
                    labyrinth.GenerateLabyrinthVisual();
                    labyrinth.transform.position = Vector3.down * resource.SelectionOffset * i;
                    Vector3 offset = labyrinth.GetLabyrinthPositionInWorldPosition(0, 0);
                    labyrinth.transform.position -= offset;
                    replayController.LabyrinthIdPairs.Add(labyrinthData[i].currentId, labyrinth);

                    labyrinth.Camera.Split(
                        resource.MaxHorizontal,
                        labyrinthData.Count/resource.MaxHorizontal,
                        i);

                    labyrinth.Camera.Source.transform.position += Vector3.up * resource.SelectionOffset/2;

                    labyrinth.Camera.Source.gameObject.SetActive(true);
                }

                StartCoroutine(DelayedOnAdvancedReplay());
            }
        }

        public IEnumerator DelayedOnAdvancedReplay()
        {
            yield return new WaitForEndOfFrame();

            if (replayController.OnAdvancedReplayHandler != null)
            {
                replayController.OnAdvancedReplayHandler.Invoke();
            }

            yield return null;
        }

    }
}
