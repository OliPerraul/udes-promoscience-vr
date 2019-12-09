using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays.UI
{
    public class TeamToggleInterface : MonoBehaviour
    {
        //[SerializeField]
        private RoundReplay replay;

        [SerializeField]
        private TeamToggleItem itemTemplate;

        [UnityEngine.Serialization.FormerlySerializedAs("scrollContentParent")]
        [SerializeField]
        private Transform toggleContentParent;

        [SerializeField]
        private Dictionary<int, TeamToggleItem> items;
        
        public void Awake()
        {       
            items = new Dictionary<int, TeamToggleItem>();
            ReplayManager.Instance.RoundReplay.OnValueChangedHandler += OnReplayChangedHandler;
        }

        public void OnReplayChangedHandler(RoundReplay replay)
        {
  
            if (this.replay != null)
            {
                this.replay.OnCourseAddedHandler -= OnCourseAdded;
            }
           
            this.replay = replay;

            if (replay == null)
                return;

            this.replay.OnCourseAddedHandler += OnCourseAdded;
            this.replay.OnCourseAddedHandler += OnCourseRemoved;
        }


        public void OnCourseAdded(Course course)
        {
            if (items.ContainsKey(course.Id))
                return;

            TeamToggleItem item = itemTemplate.Create(
                toggleContentParent,
                course);

            items.Add(course.Id, item);
        }

        public void OnCourseRemoved(Course course)
        {
            TeamToggleItem item;
            if (items.TryGetValue(course.Id, out item))
            {
                item.gameObject.Destroy();
            }
        }

    }
}