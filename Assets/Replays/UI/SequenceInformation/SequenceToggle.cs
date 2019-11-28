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
        private ReplayControllerAsset replayOptions;

        //[SerializeField]
        //private ScriptableObjects.ScriptableServerGameInformation server;

        [SerializeField]
        private SequenceToggleItem scrollItemTemplate;

        [SerializeField]
        private Transform scrollContentParent;

        [SerializeField]
        private Dictionary<int, SequenceToggleItem> items;
        
        
        public void Awake()
        {       
            items = new Dictionary<int, SequenceToggleItem>();
            replayOptions.OnActionHandler += OnReplayAction;
        }

        public void OnReplayAction(ReplayAction action, params object[] args)
        {            
            if(action == ReplayAction.Reset)
            {
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