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
    public delegate void OnCourseEvent(Course course);

    //[]
    public class Course
    {
        public int Id;

        public Utils.Algorithm Algorithm;

        public ScriptableTeam Team;

        public int[] Actions;

        public string[] ActionValues;

        public OnCourseEvent OnActionIndexChangedHandler;

        private int currentActionIndex = 0;

        public int CurrentActionIndex
        {
            get
            {
                return currentActionIndex;
            }

            set
            {
                currentActionIndex = value;
                if(OnActionIndexChangedHandler!=null)
                OnActionIndexChangedHandler.Invoke(this);
            }
        }

        public Utils.GameAction CurrentAction
        {
            get
            {
                return (Utils.GameAction)Actions[currentActionIndex];
            }
        }

        /// <summary>
        /// Warning JSON parsing done here!
        /// </summary>
        public ActionValue CurrentActionValue
        {
            get
            {
                return UnityEngine.JsonUtility.FromJson<ActionValue>(ActionValues[currentActionIndex]);
            }
        }

    }
}
