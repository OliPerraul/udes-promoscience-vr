using UnityEngine;
using System.Collections;
using System;
using UdeS.Promoscience.ScriptableObjects;
using System.Globalization;

namespace UdeS.Promoscience.Replay.UI
{
    public class SequenceToggleItem : MonoBehaviour
    {
        [SerializeField]
        private ScriptableReplayOptions replayOptions;

        [SerializeField]
        private UnityEngine.UI.Toggle toggle;

        [SerializeField]
        private UnityEngine.UI.Text label;

        [SerializeField]
        private UnityEngine.UI.Image colorImage;

        [SerializeField]
        private Course course;

        public UnityEngine.UI.Button button;




        //public UnityEngine.UI.Text scoreText;

        public SequenceToggleItem Create(Transform parent, Course course)
        {
            SequenceToggleItem item =
                Instantiate(gameObject, parent).GetComponent<SequenceToggleItem>();

            item.course = course;
            item.label.text = course.Team.name + " (" +
                            ScriptableObjects.ScriptableAlgorithm.Instance.GetName(course.Algorithm) +
                            ") ";

            item.colorImage.color = course.Team.TeamColor;
            item.gameObject.SetActive(true);
            item.button.onClick.AddListener(item.OnClicked);

            return item;
        }

        public void Awake()
        {
            toggle.onValueChanged.AddListener(OnToggle);
        }


        public void OnToggle(bool enabled)
        {
            if (replayOptions.OnSequenceToggledHandler != null)
            {
                replayOptions.OnSequenceToggledHandler.Invoke(course, enabled);
            }
        }

        public void OnClicked()
        {
            if (replayOptions.OnSequenceSelectedHandler != null)
            {
                replayOptions.OnSequenceSelectedHandler.Invoke(course);// enabled);
            }
        }


    }
}
