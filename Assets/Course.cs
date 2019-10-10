//using UnityEngine;
//using System.Collections;
//using UdeS.Promoscience.ScriptableObjects;
//using System;
//using UdeS.Promoscience.Network;
//using System.Collections.Generic;
//using UdeS.Promoscience.Utils;

using System.Linq;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;

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

        private ActionValue previousValue;

        private int currentActionIndex = 0;

        private int previousIndex = 0;

        public int CurrentActionIndex
        {
            get
            {
                return currentActionIndex;
            }

            set
            {
                previousIndex = currentActionIndex;
                currentActionIndex = value;
                if(OnActionIndexChangedHandler!=null)
                OnActionIndexChangedHandler.Invoke(this);
            }
        }

        private bool IsMovement(GameAction action)
        {
            switch (action)
            {
                case GameAction.MoveUp:
                case GameAction.MoveDown:
                case GameAction.MoveLeft:
                case GameAction.MoveRight:
                case GameAction.ReturnToDivergencePoint:
                    return true;
                default:
                    return false;
            }
        }

        public int MoveCount
        {
            get
            {
                return Actions.Aggregate(0, (x, y) => IsMovement((GameAction)y) ? x + 1 : x);
            }
        }

        private int GetNextMovementIndex()
        {
            int index = CurrentActionIndex + 1;
            while (index < Actions.Length && !IsMovement((GameAction)Actions[index]))
            {
                index++;
            }

            return index >= Actions.Length ? Actions.Length - 1 : index;
        }

        private GameAction GetNextMovementAction()
        {
            int index = CurrentActionIndex + 1;
            while (index < Actions.Length && !IsMovement((GameAction)Actions[index]))
            {
                index++;
            }

            index = index >= Actions.Length ? Actions.Length - 1 : index;
            return (GameAction)Actions[index];
        }

        // TODO why not simply "Pop a stack"
        private int GetPreviousMovementAction()
        {
            int index = CurrentActionIndex - 1;
            while (index >= 0 && !IsMovement((GameAction)Actions[index]))
            {
                index--;
            }

            return index < 0 ? 0 : index;
        }


        public bool IncrementtMovementAction()
        {
            currentActionIndex = GetNextMovementIndex();
            return true;
        }      


        public bool DecrementMovementAction()
        {
            currentActionIndex = GetPreviousMovementAction();
            return true;
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
