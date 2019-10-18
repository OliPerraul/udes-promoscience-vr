using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replay
{
    public class FinalReplay : Replay
    {
        [SerializeField]
        private Algorithm algorithm;

        private Dictionary<int, Labyrinths.Labyrinth> labyrinths;

        [SerializeField]
        private FinalReplayResource resource;

        public ICollection<Labyrinths.Labyrinth> Labyrinths
        {
            get
            {
                return labyrinths.Values;
            }
        }

        public override void OnEnable()
        {
            if (init) return;

            base.OnEnable();

            labyrinths = new Dictionary<int, Labyrinths.Labyrinth>();
        }

        public override void OnReplayAction(ReplayAction action, params object[] args)
        {
            base.OnReplayAction(action, args);

            switch (action)
            {
                case ReplayAction.ToggleLabyrinth:

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



        public override void OnServerGameStateChanged()
        {
            if (server.GameState ==
                ServerGameState.FinalReplay)
            {
                var labyrinthData = SQLiteUtilities.GetLabyrinths();

                for (int i = 0; i < labyrinthData.Count; i++)
                {
                    Labyrinths.Labyrinth labyrinth = resources.Labyrinth.Create(transform, labyrinthData[i]);
                    labyrinth.GenerateLabyrinthVisual();
                    labyrinth.transform.position = Vector3.down * resource.SelectionOffset * i;
                    Vector3 offset = labyrinth.GetLabyrinthPositionInWorldPosition(0, 0);
                    labyrinth.transform.position -= offset;
                    labyrinths.Add(labyrinthData[i].currentId, labyrinth);

                    labyrinth.Camera.Split(resource.MaxHorizontal, resource.MaxHorizontal/labyrinthData.Count, i);
                    labyrinth.Camera.Source.gameObject.SetActive(true);
                }
            }
        }
    }
}
