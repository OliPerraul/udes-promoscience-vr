using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using Cirrus.Extensions;
using System.Linq;

namespace UdeS.Promoscience.Replays.UI
{
    /// <summary>
    /// Shows which team information is displayed
    /// </summary>
    public class SelectedTeamDisplay : MonoBehaviour
    {
        [SerializeField]
        private SelectedTeamAsset asset;

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
            //ReplayManager.Instance.OnLabyrinthReplayCreatedHandler += OnLabyrinthReplayStarted;
            asset.OnReplayCourseSelectedHandler += OnCourseSelected;
        }

        public void OnDestroy()
        {
            asset.OnReplayCourseSelectedHandler -= OnCourseSelected;
        }


        public void OnServerStateChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.LevelReplay:
                    //ReplayManager.Instance.LabyrinthReplay.CurrentCourse.OnValueChangedHandler += OnCourseSelected;


                    break;
            }
        }

        public void OnCourseSelected(TeamReplay replay)
        {
            if (replay.Execution == null)
                return;

            if (course == replay.Execution)
                return;

            if (course != null)
            {
                course.OnPlayerSequenceProgressedHandler -= OnCourseActionIndexChanged;
            }

            course = replay.Execution;

            teamIcon.color = replay.Execution.Course.Team.TeamColor.SetA(teamIcon.color.a);
            replay.Execution.OnPlayerSequenceProgressedHandler += OnCourseActionIndexChanged;

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
            stepCountText.text = course.CurrentMoveIndex.ToString();
            errorCountText.text = course.PreviousActionValue.error.ToString();
            elapsedTimeText.text = course.PreviousActionValue.elapsedSeconds.ToString() + " sec(s)";
        }
    }
}