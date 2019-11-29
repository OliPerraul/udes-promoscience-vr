using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using Cirrus.Extensions;
using System.Linq;

namespace UdeS.Promoscience.Replays.UI
{
    public class SequencePopup : MonoBehaviour
    {
        [SerializeField]
        private ReplayManagerAsset replayOptions;

        //[SerializeField]
        //private ScriptableServerGameInformation server;

        [SerializeField]
        private UnityEngine.UI.Text scoreText;

        [SerializeField]
        private UnityEngine.UI.Text respectText;

        [SerializeField]
        private UnityEngine.UI.Image image;


        private Course course;

        
        public void OnEnable()
        { 
            replayOptions.OnCourseSelectedHandler += OnCourseSelected;
        }


        public void OnCourseSelected(Course newCourse)
        {

            if (newCourse == null)
                return;

            if (course == newCourse)
                return;

            if (course != null)
            {
                course.OnPlayerSequenceProgressedHandler -= OnCourseActionIndexChanged;
            }

            course = newCourse;

            image.color = newCourse.Team.TeamColor.SetA(image.color.a);
            newCourse.OnPlayerSequenceProgressedHandler += OnCourseActionIndexChanged;

            OnCourseActionIndexChanged();
        }

        private void OnCourseActionIndexChanged()//Course course)
        {            
            if (course.CurrentAction == GameAction.EndMovement ||
                course.CurrentAction == GameAction.Unknown)
            {
                return;
            }

            respectText.text = course.PreviousActionValue.respect.ToString("P0");
            scoreText.text = course.PreviousActionValue.error.ToString();
        }


        //public void OnGameStateChanged()
        //{
        //    switch (server.GameState)
        //    {
        //        case Promoscience.Utils.ServerGameState.SimpleReplay:

        //            if (server.Courses.Count != 0)
        //            {
        //                OnSequenceSelected(server.Courses.First());
        //            }

        //            break;
        //    }
        //}


    }
}