//using UnityEngine;
//using System.Collections;
//using UdeS.Promoscience.ScriptableObjects;
//using System;
//using UdeS.Promoscience.Network;
//using System.Collections.Generic;
////using UdeS.Promoscience.Utils;

using Cirrus;
using Cirrus.Extensions;
using System.Collections.Generic;
using System.Linq;
using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;

// TODO: course iterator vs. course as separate class?

namespace UdeS.Promoscience
{
    //public delegate void OnCourseEvent(Course course);

    public enum CourseStatus
    {
        // current: whether the course is used in the current play session (used for current replay)
        Active = 0,
        // current: whether the course is used in the current play session (used for the final replay)
        Session = 1,
        // finished: whether the course is kept as log
        Finished = 2
    }

    // Is associated to one particular team, vs Round is not
    // Levels contain a series of courses

    public class Course
    {
        public int Id;

        public int LabyrinthId;

        public CourseStatus Status;

        public Labyrinths.ILabyrinth Labyrinth;

        public Algorithms.Algorithm Algorithm;

        public Teams.TeamResource Team;
    }


    public class CourseExecution : Algorithms.ICourseExecution
    {
        private Course course;

        public Course Course => course;

        public Labyrinths.ILabyrinth Labyrinth;

        public int[] Actions;

        public string[] ActionValues;

        public Event<Course> OnActionIndexChangedHandler;

        public Event OnPlayerSequenceProgressedHandler;

        private ActionValue previousActionValue;

        private int currentActionIndex = 0;

        private int moveCount = -1;

        private int moveIndex = 0;

        public int CurrentMoveIndex => moveIndex;

        public CourseExecution(Course course, Labyrinths.ILabyrinth labyrinth)
        {
            this.course = course;

            Queue<int> steps;
            Queue<string> stepValues;
            SQLiteUtilities.GetPlayerStepsForCourse(course.Id, out steps, out stepValues);
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
                case GameAction.EndMovement:
                    //case GameAction.Finish://sentinel value
                    return true;
                default:
                    return false;
            }
        }

        private int CurrentActionIndex
        {
            get => currentActionIndex;          

            set
            {
                if (value != currentActionIndex)
                {
                    previousActionValue = CurrentActionValue;
                }

                currentActionIndex = value;

                if (OnActionIndexChangedHandler != null)
                {
                    OnActionIndexChangedHandler.Invoke(course);
                }
            }
        }


        public int MoveCount
        {
            get
            {
                if (moveCount < 0)
                {
                    moveCount = Actions.Aggregate(0, (x, y) => IsMovement((GameAction)y) ? x + 1 : x);
                }

                return moveCount;
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

        // TODO why not simply "Pop a stack"
        private int GetPreviousMovementIndex()
        {
            int index = CurrentActionIndex - 1;
            while (index >= 0 && !IsMovement((GameAction)Actions[index]))
            {
                index--;
            }

            return index < 0 ? 0 : index;
        }


        public bool Next()
        {
            moveIndex = HasNext ?
                (HasPrevious ?
                    moveIndex :
                    0) :
             moveIndex - 1;

            moveIndex++;

            CurrentActionIndex = GetNextMovementIndex();
            return true;
        }


        public bool Previous()
        {
            moveIndex = HasPrevious ?
                (HasNext ?
                    moveIndex :
                    moveCount) :
                0;

            moveIndex--;

            CurrentActionIndex = GetPreviousMovementIndex();
            return true;
        }



        public bool HasPrevious
        {
            get
            {
                if (MoveCount == 0)
                    return false;

                return moveIndex > 0;
            }
        }

        public bool HasNext
        {
            get
            {
                if (MoveCount == 0)
                    return false;

                return moveIndex < MoveCount;
            }
        }


        /// <summary>
        /// Warning JSON parsing done here! (TODO: remove property)
        /// </summary>
        public GameAction PreviousAction
        {
            get
            {
                return HasPrevious ?
                    (GameAction)Actions[GetPreviousMovementIndex()] :
                    (GameAction)Actions[0];
            }
        }

        /// <summary>
        /// Warning JSON parsing done here! (TODO: remove property)
        /// </summary>
        public ActionValue CurrentActionValue
        {
            get
            {
                if (currentActionIndex >= ActionValues.Length)
                    return new ActionValue();

                return UnityEngine.JsonUtility.FromJson<ActionValue>(ActionValues[CurrentActionIndex]);
            }
        }

        /// <summary>
        /// Warning JSON parsing done here! (TODO: remove property)
        /// </summary>
        public ActionValue PreviousActionValue
        {
            get
            {
                return HasPrevious ?
                    UnityEngine.JsonUtility.FromJson<ActionValue>(ActionValues[GetPreviousMovementIndex()]) :
                    UnityEngine.JsonUtility.FromJson<ActionValue>(ActionValues[0]);
            }
        }

        public GameAction CurrentAction
        {
            get
            {
                if (currentActionIndex >= Actions.Length)
                    return GameAction.Unknown;

                return (GameAction)Actions[CurrentActionIndex];
            }
        }

    }
}
