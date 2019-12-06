using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UdeS.Promoscience.Labyrinths
{
    [System.Serializable]
    public class LabyrinthManager
    {
        public ILabyrinth CurrentData { get; set; }

        public LabyrinthObject CurrentLabyrinth { get; set; }

        public ICollection<ILabyrinth> data = new List<ILabyrinth>();

        public List<Resource> Data
        {
            get
            {
                return Resources.Instance.Labyrinths;
            }
        }

        public ICollection<LabyrinthObject> Labyrinths
        {
            get
            {
                return IdPairs.Values;
            }
        }

        private Dictionary<int, LabyrinthObject> labyrinths = new Dictionary<int, LabyrinthObject>();

        public IDictionary<int, LabyrinthObject> IdPairs
        {
            get
            {
                if (labyrinths == null)
                    labyrinths = new Dictionary<int, LabyrinthObject>();
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