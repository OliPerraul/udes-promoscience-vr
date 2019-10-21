using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays
{
    // Playback for a single team
    public class SimpleReplay : Replay
    {


        private LabyrinthReplay labyrinthReplay;
        

        [SerializeField]
        private ScriptableController replayController;

        public override ScriptableController Controller
        {
            get
            {
                return replayController;
            }
        }


        public override void Awake()
        {
            base.Awake();
        }


        public override void OnServerGameStateChanged()
        {
            if (Server.GameState ==
                ServerGameState.SimpleReplay)
            {
                Labyrinths.Data labyrinthData = new Labyrinths.Data();

                SQLiteUtilities.ReadLabyrinthDataFromId(Server.GameRound, labyrinthData);

                Labyrinths.Labyrinth labyrinth = LabyrinthResources.Labyrinth.Create(labyrinthData);
                
                var courses = SQLiteUtilities.GetSessionCoursesForLabyrinth(labyrinthData.currentId);
                
                labyrinthReplay = new LabyrinthReplay(this, labyrinth, algorithm, courses);

                labyrinthReplay.Start();
            }
        }
    }
}
