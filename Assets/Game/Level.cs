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

        public Labyrinths.ILabyrinth Labyrinth => Labyrinths.LabyrinthManager.Instance.GetNextLabyrinth();
    }

    /// <summary>
    /// Previously named Round 
    /// Refers to a labyrinth and a base algorithm
    /// </summary>
    public class Level
    {
        public int Number = 0;

        public List<Course> Courses = new List<Course>();

        public Algorithms.Algorithm Algorithm;

        public Labyrinths.ILabyrinth Labyrinth;

    }
}