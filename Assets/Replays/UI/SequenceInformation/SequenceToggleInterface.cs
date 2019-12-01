using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays.UI
{
    public class SequenceToggleInterface : MonoBehaviour
    {
        [SerializeField]
        private ReplayManagerAsset replayOptions;

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
            replayOptions.OnActionHandler += OnReplayAction;
            replayOptions.OnCourseAddedHandler += OnCourseAdded;
        }


        public void OnCourseAdded(Course course, bool added)
        {
            if (added)
            {
                if (items.ContainsKey(course.Id))
                    return;

                SequenceToggleItem item = itemTemplate.Create(
                    toggleContentParent,
                    course);

                items.Add(course.Id, item);
            }
            else
            {
                SequenceToggleItem item;
                if (items.TryGetValue(course.Id, out item))
                {
                    item.gameObject.Destroy();
                }
            }
        }



        public void OnReplayAction(ReplayControlAction action, params object[] args)
        {            
            if(action == ReplayControlAction.Reset)
            {
                foreach (Transform child in toggleContentParent)
                {
                    if (!child.gameObject.activeSelf)
                        continue;

                    Destroy(child.gameObject);
                }

                items.Clear();
            }
        }
    }
}