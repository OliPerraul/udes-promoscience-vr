using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replay.UI
{
    public class SequencePopup : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Text scoreText;

        [SerializeField]
        private UnityEngine.UI.Text respectText;

        [SerializeField]
        private UnityEngine.UI.Image image;

        [SerializeField]
        private ScriptableReplayOptions replayOptions;

        private Course course;

        public void Awake()
        {
            replayOptions.OnSequenceSelectedHandler += OnSequenceSelected;            
        }

        public void OnSequenceSelected(Course course)
        {
            if (course == null)
                return;

            if (this.course == course)
                return;

            if(course != null)
                course.OnActionIndexChangedHandler -= OnCourseActionIndexChanged;

            this.course = course;

            image.color = course.Team.TeamColor.SetA(image.color.a);
            course.OnActionIndexChangedHandler += OnCourseActionIndexChanged;

            OnCourseActionIndexChanged(course);
        }

        private void OnCourseActionIndexChanged(Course course)
        {
            ActionValue value = course.CurrentActionValue;

            respectText.text = value.respect.ToString("P1");// CultureInfo.InvariantCulture);
            scoreText.text = value.error.ToString();
        }


    }
}