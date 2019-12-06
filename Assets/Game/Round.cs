using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UdeS.Promoscience
{
    [System.Serializable]
    public class RoundPreset
    {
        [SerializeField]
        public Algorithms.Id Algorithm;

        [SerializeField]
        public Labyrinths.Resource Labyrinth;
    }

    public class Round
    {
        public int Number = 0;

        public List<Course> Courses = new List<Course>();

        public Algorithms.Algorithm Algorithm;

        public Labyrinths.ILabyrinth Labyrinth;

    }
}