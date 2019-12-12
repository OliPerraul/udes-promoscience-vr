using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Labyrinths
{
    [System.Serializable]
    public class LabyrinthManager : Cirrus.BaseSingleton<LabyrinthManager>
    {
        private static int nextLabyrinth = 0;

        private List<ILabyrinth> labyrinths;

        public List<ILabyrinth> Labyrinths
        {
            get
            {
                if (labyrinths == null || labyrinths.Count == 0)
                {
                    labyrinths = SQLiteUtilities.LoadAllLabyrinths2().ToList();
                }

                return labyrinths;
            }
        }

        public ILabyrinth GetLabyrinth(int id)
        {
            return Labyrinths.Where((x) => x.Id == id).FirstOrDefault();
        }

        public ILabyrinth GetNextLabyrinth()
        {
            var lab = Labyrinths[nextLabyrinth];
            nextLabyrinth++;
            nextLabyrinth = nextLabyrinth.Mod(Labyrinths.Count);
            return lab;
        }

    }
}