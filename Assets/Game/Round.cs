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
        public List<Course> Courses = new List<Course>();

        public Labyrinths.IData Labyrinth;

    }
}