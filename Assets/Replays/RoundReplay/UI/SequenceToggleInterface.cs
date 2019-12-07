using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays.UI
{
    public class SequenceToggleInterface : MonoBehaviour
    {
        //[SerializeField]
        private RoundReplay replay;

        [SerializeField]
        private SequenceToggleItem itemTemplate;

        [UnityEngine.Serialization.FormerlySerializedAs("scrollContentParent")]
        [SerializeField]
        private Transform toggleContentParent;

        [SerializeField]
        private Dictionary<int, SequenceToggleItem> items;
        
        public void Awake()
        {       
            items = new Dictionary<int, SequenceToggleItem>();
            ReplayManager.Instance.RoundReplay.OnValueChangedHandler += OnReplayChangedHandler;
        }

        public void OnReplayChangedHandler(RoundReplay replay)
        {
            if (this.replay != null)
            {
                this.replay.OnCourseAddedHandler -= OnCourseAdded;
            }

            this.replay = replay;

            this.replay.OnCourseAddedHandler += OnCourseAdded;
            this.replay.OnCourseAddedHandler += OnCourseRemoved;
        }


        public void OnCourseAdded(Course course)
        {
            if (items.ContainsKey(course.Id))
                return;

            SequenceToggleItem item = itemTemplate.Create(
                toggleContentParent,
                course);

            items.Add(course.Id, item);
        }

        public void OnCourseRemoved(Course course)
        {
            SequenceToggleItem item;
            if (items.TryGetValue(course.Id, out item))
            {
                item.gameObject.Destroy();
            }
        }

    }
}