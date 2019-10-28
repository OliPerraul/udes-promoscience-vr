using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays.UI
{
    public class SequenceToggle : MonoBehaviour
    {
        [SerializeField]
        private ScriptableController replayOptions;

        //[SerializeField]
        //private ScriptableObjects.ScriptableServerGameInformation server;

        [SerializeField]
        private SequenceToggleItem scrollItemTemplate;

        [SerializeField]
        private Transform scrollContentParent;

        [SerializeField]
        private Dictionary<int, SequenceToggleItem> items;
        
        private SequenceToggleItem first = null;

        private bool init = false;
        
        public void OnEnable()
        {
            if (init) return;

            init = true;
                        
            items = new Dictionary<int, SequenceToggleItem>();
            replayOptions.OnActionHandler += OnReplayAction;
        }

        public IEnumerator DelayedSelectCoroutine(SequenceToggleItem item)
        {
            yield return new WaitForEndOfFrame();            
            item.OnClicked();
        }

        public void OnReplayAction(ReplayAction action, params object[] args)
        {
            if(action == ReplayAction.Reset)
            {
                first = null;

                foreach (Transform child in scrollContentParent)
                {
                    if (!child.gameObject.activeSelf)
                        continue;

                    Destroy(child.gameObject);
                }

                items.Clear();
            }
            else if (action == ReplayAction.AddCourse)
            {
                bool added = (bool)args[0];
                var course = (Course)args[1];

                if (added)
                {
                    if (items.ContainsKey(course.Id))
                        return;

                    SequenceToggleItem item = scrollItemTemplate.Create(
                        scrollContentParent,
                        course);

                    items.Add(course.Id, item);

                    if (first == null)
                    {
                        first = item;
                        StartCoroutine(DelayedSelectCoroutine(first));
                    }
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
        }
    }
}