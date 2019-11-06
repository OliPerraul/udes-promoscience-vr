using UnityEngine;
using System.Collections;
using System;
using UdeS.Promoscience.ScriptableObjects;
using System.Globalization;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays.UI
{
    public class SequenceToggleItem : MonoBehaviour
    {
        [SerializeField]
        private ControllerAsset replayController;

        [SerializeField]
        private UnityEngine.UI.Toggle toggle;

        [SerializeField]
        private UnityEngine.UI.Text label;

        [SerializeField]
        private UnityEngine.UI.Image colorImage;

        [SerializeField]
        private Course course;

        public UnityEngine.UI.Button button;


        public SequenceToggleItem Create(
            Transform parent, 
            Course course)
        {
            SequenceToggleItem item = this.Create(parent);

            item.course = course;
            item.label.text = course.Team.name + " (" +
                            course.Algorithm.Name +
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
            replayController.SendAction(ReplayAction.SequenceToggled, enabled);
        }

        public void OnClicked()
        {
            replayController.SendAction(ReplayAction.SequenceSelected, course);   
        }


    }
}
