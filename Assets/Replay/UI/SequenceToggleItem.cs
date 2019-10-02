using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Replay.UI
{
    public class SequenceToggleItem : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Toggle toggle;

        [SerializeField]
        private UnityEngine.UI.Text label;

        [SerializeField]
        private UnityEngine.UI.Image colorImage;

        private CourseData course;

        public OnSequenceToggled OnSequenceToggledHandler;

        public SequenceToggleItem Create(Transform parent, CourseData course)
        {
            SequenceToggleItem item =
                Instantiate(gameObject, parent).GetComponent<SequenceToggleItem>();
            item.course = course;
            item.label.text = course.Team.name + " (" +
                            ScriptableObjects.ScriptableAlgorithm.Instance.GetName(course.Algorithm) +
                            ") ";

            item.colorImage.color = course.Team.TeamColor;
            item.gameObject.SetActive(true);

            return item;
        }

        public void Awake()
        {
            toggle.onValueChanged.AddListener(OnToggle);
        }


        public void OnToggle(bool enabled)
        {
            if (OnSequenceToggledHandler != null)
            {
                OnSequenceToggledHandler.Invoke(course, enabled);
            }
        }


    }
}
