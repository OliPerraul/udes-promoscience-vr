using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using Cirrus.Extensions;
using System.Linq;

namespace UdeS.Promoscience.Replays.UI
{
    public class SelectedSequenceDisplay : MonoBehaviour
    {
        [SerializeField]
        private ReplayManagerAsset replayOptions;

        [SerializeField]
        private UnityEngine.UI.Text teamName;

        [SerializeField]
        private UnityEngine.UI.Image teamIcon;

        [SerializeField]
        private UnityEngine.UI.Text stepCountText;

        [SerializeField]
        private UnityEngine.UI.Text respectPercentText;

        [SerializeField]
        private UnityEngine.UI.Text errorCountText;

        [SerializeField]
        private UnityEngine.UI.Text elapsedTimeText;

        private Course course;

        
        public void Awake()
        { 
            replayOptions.CurrentCourse.OnValueChangedHandler += OnCourseSelected;
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

            teamIcon.color = newCourse.Team.TeamColor.SetA(teamIcon.color.a);
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

            respectPercentText.text = course.PreviousActionValue.respect.ToString("P0");
            stepCountText.text = course.PreviousActionValue.error.ToString();
        }
    }
}