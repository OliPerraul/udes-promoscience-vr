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
        private LabyrinthReplay replay;

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

        private CourseExecution course;

        
        public void Awake()
        {
            ReplayManager.Instance.OnLabyrinthReplayCreatedHandler += OnLabyrinthReplayStarted;
        }

        public void OnLabyrinthReplayStarted(LabyrinthReplay replay)
        {
            this.replay = replay;
            //replay.CurrentCourse.OnValueChangedHandler += OnCourseSelected;
        }

        public void OnCourseSelected(CourseExecution execution)
        {
            if (execution == null)
                return;

            if (course == execution)
                return;

            if (course != null)
            {
                course.OnPlayerSequenceProgressedHandler -= OnCourseActionIndexChanged;
            }

            course = execution;

            teamIcon.color = execution.Course.Team.TeamColor.SetA(teamIcon.color.a);
            execution.OnPlayerSequenceProgressedHandler += OnCourseActionIndexChanged;

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