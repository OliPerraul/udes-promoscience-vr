using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replays.Advanced
{
    public class ScriptableController : Replays.ScriptableController
    {
        private Dictionary<int, Labyrinths.Labyrinth> labyrinths;

        public Cirrus.OnEvent OnAdvancedReplayHandler;

        public ICollection<Labyrinths.Labyrinth> Labyrinths
        {
            get
            {
                return labyrinths.Values;
            }
        }

        public IDictionary<int, Labyrinths.Labyrinth> LabyrinthIdPairs
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
        }
    }
}