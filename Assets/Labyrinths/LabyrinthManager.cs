using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UdeS.Promoscience.Labyrinths
{
    [System.Serializable]
    public class LabyrinthManager
    {
        public IData CurrentData { get; set; }

        public Labyrinth CurrentLabyrinth { get; set; }

        public ICollection<IData> data = new List<IData>();

        public List<Resource> Data
        {
            get
            {
                return Resources.Instance.Labyrinths;
            }
        }

        public ICollection<Labyrinth> Labyrinths
        {
            get
            {
                return IdPairs.Values;
            }
        }

        private Dictionary<int, Labyrinth> labyrinths = new Dictionary<int, Labyrinth>();

        public IDictionary<int, Labyrinth> IdPairs
        {
            get
            {
                if (labyrinths == null)
                    labyrinths = new Dictionary<int, Labyrinth>();
                return labyrinths;
            }
        }

        public void Clear()
        {
            if (labyrinths != null)
                labyrinths.Clear();
        }
    }
}