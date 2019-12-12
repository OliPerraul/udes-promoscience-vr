using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience
{
    [System.Serializable]
    public class LevelPreset
    {
        [SerializeField]
        public Algorithms.Id Algorithm;

        //[SerializeField]
        //private int labyrinthId;

        //public int LabyrinthId => labyrinthId.Mod(Server.Instance.Labyrinths.Count);

        public Labyrinths.ILabyrinth Labyrinth => Labyrinths.LabyrinthManager.Instance.GetNextLabyrinth();
    }

    // Previously named Round
    // TODO rename remaining round reference?
    public class Level
    {
        public int Number = 0;

        public List<Course> Courses = new List<Course>();

        public Algorithms.Algorithm Algorithm;

        public Labyrinths.ILabyrinth Labyrinth;

    }
}