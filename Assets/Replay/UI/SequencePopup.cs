using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using Cirrus.Extensions;
using System.Linq;

namespace UdeS.Promoscience.Replay.UI
{
    public class SequencePopup : MonoBehaviour
    {
        [SerializeField]
        private ScriptableReplayOptions replayOptions;

        [SerializeField]
        private ScriptableServerGameInformation server;

        [SerializeField]
        private UnityEngine.UI.Text scoreText;

        [SerializeField]
        private UnityEngine.UI.Text respectText;

        [SerializeField]
        private UnityEngine.UI.Image image;


        private Course course;

        private bool init = false;

        public void OnEnable()
        {
            if (init)
                return;

            init = true;

            replayOptions.OnSequenceSelectedHandler += OnSequenceSelected;
            server.gameStateChangedEvent += OnGameStateChanged;
        }

        public void OnSequenceSelected(Course newCourse)
        {
            if (newCourse == null)
                return;

            if (course == newCourse)
                return;

            if (course != null)
            {
                course.OnActionIndexChangedHandler -= OnCourseActionIndexChanged;
            }          

            course = newCourse;

            image.color = newCourse.Team.TeamColor.SetA(image.color.a);
            newCourse.OnActionIndexChangedHandler += OnCourseActionIndexChanged;

            OnCourseActionIndexChanged(newCourse);
        }

        private void OnCourseActionIndexChanged(Course course)
        {
            if (course.CurrentAction == Utils.GameAction.Finish ||
                course.CurrentAction == Utils.GameAction.EndMovement)// ||
                //course.CurrentAction == Utils.GameAction.ReturnToDivergencePoint ||
                //course.CurrentAction == Utils.GameAction.CompletedRound)
            {
                return;
            }

            respectText.text = course.PreviousActionValue.respect.ToString("P0");
            scoreText.text = course.PreviousActionValue.error.ToString();            
        }


        public void OnGameStateChanged()
        {
            switch (server.GameState)
            {
                case Utils.ServerGameState.ViewingPlayback:

                    if(server.Courses.Count != 0)
                    {
                        OnSequenceSelected(server.Courses.First());
                    }

                    break;
            }
        }


    }
}