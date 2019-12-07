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

        private RoundReplay replay;

        public void Awake()
        {
            //ReplayManager.Instance.OnLabyrinthReplayCreatedHandler += OnLabyrinthReplayStarted;
            ReplayManager.Instance.RoundReplay.OnValueChangedHandler += OnReplayChanged;
        }

        public void OnReplayChanged(RoundReplay replay)
        {
            if (this.replay != null)
            {
                //ReplayManager.Instance.LabyrinthReplay.CurrentCourse.OnValueChangedHandler += OnCourseSelected;
            }

            this.replay = replay;
        }

        public void OnServerStateChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.RoundReplay:
                    //ReplayManager.Instance.LabyrinthReplay.CurrentCourse.OnValueChangedHandler += OnCourseSelected;


                    break;
            }
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