using UnityEngine;
using System.Collections;
using System;
using UdeS.Promoscience.ScriptableObjects;
using System.Globalization;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays.UI
{
    /// <summary>
    /// Item pertaining to a single team in the team toggle interface
    /// </summary>
    public class TeamToggleItem : MonoBehaviour
    {
        [SerializeField]
        private TeamToggleAsset asset;

        public static Cirrus.Event OnOtherSelectedHandler;

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


        public TeamToggleItem Create(
            Transform parent,
            Course course)
        {
            TeamToggleItem item = this.Create(parent);

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
                asset?.OnCourseSelectedHandler(course);
            });

            //ReplayManager.Instance.OnLabyrinthReplayCreatedHandler += OnLabyrinthReplay;
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

            asset.OnCourseToggledHandler?.Invoke(course, enabled);
        }
    }
}
