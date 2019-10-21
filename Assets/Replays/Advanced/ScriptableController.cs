using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.Replays.Advanced
{
    public class ScriptableController : Replays.ScriptableController
    {
        private Dictionary<int, Labyrinths.Labyrinth> labyrinths;

        //private List<Cours>

        public Cirrus.OnEvent OnAdvancedReplayHandler;

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
                return labyrinths.Values;
            }
        }

        public IDictionary<int, Labyrinths.Labyrinth> IdLabyrinthPairs
        {
            get
            {
                return labyrinths;
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            labyrinths = new Dictionary<int, Labyrinths.Labyrinth>();
            labyrinthsData = new List<Labyrinths.IData>();
        }
    }
}