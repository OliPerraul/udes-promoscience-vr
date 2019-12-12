using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UdeS.Promoscience
{
    [System.Serializable]
    public class LevelPreset
    {
        [SerializeField]
        public Algorithms.Id Algorithm;

        [SerializeField]
        public Labyrinths.Resource Labyrinth;
    }

    public class Level
    {
        public int Number = 0;

        public List<Course> Courses = new List<Course>();

        public Algorithms.Algorithm Algorithm;

        public Labyrinths.ILabyrinth Labyrinth;

    }
}