using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UdeS.Promoscience.Utils;
using System;

namespace UdeS.Promoscience.Replays.Advanced
{
    public class ScriptableController : Replays.ScriptableController
    {
        
        //private List<Cours>

        public Cirrus.OnEvent OnAdvancedReplayHandler;

        public override void OnEnable()
        {
            base.OnEnable();
            labyrinths = new Dictionary<int, Labyrinths.Labyrinth>();
            labyrinthsData = new List<Labyrinths.IData>();
        }


        public ICollection<Labyrinths.IData> labyrinthsData;

        public ICollection<Labyrinths.IData> LabyrinthsData
        {
            get
            {
                if (labyrinthsData == null || labyrinthsData.Count == 0)
                {
                    labyrinthsData = SQLiteUtilities.GetLabyrinths();
                }

                return labyrinthsData;
            }
        }

        public ICollection<Labyrinths.Labyrinth> Labyrinths
        {
            get
            {
                return IdLabyrinthPairs.Values;
            }
        }

        private Dictionary<int, Labyrinths.Labyrinth> labyrinths;

        public IDictionary<int, Labyrinths.Labyrinth> IdLabyrinthPairs
        {
            get
            {
                if (labyrinths == null)
                    labyrinths = new Dictionary<int, Labyrinths.Labyrinth>();
                return labyrinths;
            }
        }

        public void Clear()
        {
            if(labyrinthsData != null)
                labyrinthsData.Clear();

            if(labyrinths != null)
                labyrinths.Clear();
        }
    }
}