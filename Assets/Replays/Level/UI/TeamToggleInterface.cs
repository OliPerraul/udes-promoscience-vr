using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays.UI
{
    public class TeamToggleInterface : MonoBehaviour
    {
        [SerializeField]
        private TeamToggleAsset asset;

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

            asset.OnReplayCourseAddedHandler += OnCourseAdded;
            asset.OnReplayCourseRemovedHandler += OnCourseRemoved;
            asset.OnReplayCourseRemoveAllHandler += OnCourseRemovedAll;
        }

        public void OnDestroy()
        {
            asset.OnReplayCourseAddedHandler -= OnCourseAdded;
            asset.OnReplayCourseRemovedHandler -= OnCourseRemoved;
            asset.OnReplayCourseRemoveAllHandler -= OnCourseRemovedAll;
        }



        public void OnCourseAdded(Course course)
        {
            if (items.ContainsKey(course.Id))
                return;

            /// TODO remove team toggle disabled template gobj
            TeamToggleItem item = Resources.Instance.teamToggleItem.Create(
                toggleContentParent,
                course);

            item.gameObject.SetActive(true);

            items.Add(course.Id, item);
        }

        public void OnCourseRemoved(Course course)
        {
            //TeamToggleItem item;
            //if (items.TryGetValue(course.Id, out item))
            //{
            //    items.Remove(course.Id);
            //    item.gameObject.Destroy();
            //}
        }

        public void OnCourseRemovedAll()//Course course)
        {
            //TeamToggleItem item;
            foreach (var item in items)
            {
                item.Value.OnDestroy();
            }

            // FIX:
            // Clear remaining entries...
            // TODO remove ?? 
            // We should not clear up like this
            foreach (Transform child in toggleContentParent.transform)
            {
                if (child == null)
                    continue;

                if (child.gameObject == null)
                    continue;

                child.gameObject.Destroy();
            }


            items.Clear();
        }

    }
}