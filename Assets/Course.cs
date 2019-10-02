//using UnityEngine;
//using System.Collections;
//using UdeS.Promoscience.ScriptableObjects;
//using System;
//using UdeS.Promoscience.Network;
//using System.Collections.Generic;
//using UdeS.Promoscience.Utils;

using UdeS.Promoscience.ScriptableObjects;

namespace UdeS.Promoscience
{
    public delegate void OnCourseEvent(CourseData course);

    public class CourseData
    {
        public int Id;

        public Utils.Algorithm Algorithm;

        public ScriptableTeam Team;

        public int[] Actions;

        public string[] ActionValues;
    }
}
