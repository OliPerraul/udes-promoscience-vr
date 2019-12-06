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
        public static Cirrus.Event OnOtherSelectedHandler;

        private LabyrinthReplay replay;

        [SerializeField]
        private UnityEngine.Color selectedColor;

        [SerializeField]
        private UnityEngine.Color defaultColor;

        [SerializeField]
        private UnityEngine.UI.Toggle toggle;

        [SerializeField]
        private UnityEngine.UI.Text label;

        [SerializeField]
        private UnityEngine.UI.Image colorImage;

        [SerializeField]
        private UnityEngine.UI.Image selectedImage;
        


        [SerializeField]
        private Course course;

        public UnityEngine.UI.Button button;

        public void OnValidate()
        {
            //if(button != null)
            //defaultColor = button.image.color;
        }


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
            
            return item;
        }

        public void Awake()
        {
            toggle.onValueChanged.AddListener(OnToggle);
            button.onClick.AddListener(() =>
            {               
                OnOtherSelectedHandler?.Invoke();

                selectedImage.color = selectedColor;
                OnOtherSelectedHandler += OnOtherSelected;
                replay.CurrentCourse.Value = course;
            });

            ReplayManager.Instance.OnLabyrinthReplayCreatedHandler += OnLabyrinthReplay;
        }

        public void OnLabyrinthReplay(LabyrinthReplay replay)
        {
            this.replay = replay;            
        }


        public void OnDestroy()
        {
            OnOtherSelectedHandler -= OnOtherSelected;
        }

        public void OnOtherSelected()
        {
            OnOtherSelectedHandler -= OnOtherSelected;
            selectedImage.color = defaultColor;
        }


        public void OnToggle(bool enabled)
        {


            replay.OnCourseToggledHandler?.Invoke(course, enabled);
        }
    }
}
