using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;
using Cirrus;
using Cirrus.Extensions;

namespace UdeS.Promoscience.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/ClientGameState", order = 1)]
    public class ScriptableClientGameState : ScriptableObject
    {
        public OnEvent OnLabyrinthChangedHandler;

        [SerializeField]
        private Labyrinths.IData labyrinthData;

        public Labyrinths.IData LabyrinthData
        {
            get
            {
                return labyrinthData;
            }

            set
            {
                labyrinthData = value;

                if (OnLabyrinthChangedHandler != null)
                    OnLabyrinthChangedHandler.Invoke();
            }
        }

        private Labyrinths.Labyrinth labyrinth;

        public Labyrinths.Labyrinth Labyrinth
        {
            get
            {
                return labyrinth;
            }

            set
            {
                labyrinth = value;
            }
        }

        public OnEvent OnAlgorithmChangedHandler;

        [SerializeField]
        private Algorithms.Algorithm algorithm;

        public Algorithms.Algorithm Algorithm
        {
            get
            {
                return algorithm;
            }

            set
            {
                algorithm = value;

                if(OnAlgorithmChangedHandler != null)
                    OnAlgorithmChangedHandler.Invoke();
            }
        }


        [SerializeField]
        private ClientGameState value;

        public Action clientStateChangedEvent;

        public void OnEnable()
        {
            value = ClientGameState.Connecting;
            ErrorCount = 0;
            //previousRespect = 1;
            respect = 1;
        }

        // TODO replace algorithm and labyrinth fields by Course
        //public OnEvent OnCourseChangedHandler;

        //public Course course;

        //public Course Course
        //{
        //    get
        //    {
        //        return course;
        //    }

        //    set
        //    {
        //        course = value;
        //        if(OnCourseChangedHandler != null)
        //            OnCourseChangedHandler.Invoke();
        //    }
        //}

        public string[] ActionValues;

        public int[] ActionSteps;

        public int ErrorCount = 0;

        private float respect;

        public OnFloatEvent OnRespectChangedHandler;

        public float Respect
        {
            get
            {
                return respect;
            }

            set
            {
                if (respect.Approximately(value))
                    return;

                respect = value;
                if (OnRespectChangedHandler != null)
                {
                    OnRespectChangedHandler.Invoke(respect);
                }
            }
        }

        public ClientGameState Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                OnValueChanged();
            }
        }

        public void OnValueChanged()
        {
            if (clientStateChangedEvent != null)
            {
                clientStateChangedEvent();
            }
        }
    }
}

