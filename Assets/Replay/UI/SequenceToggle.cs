using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Replay.UI
{
    public class SequenceToggle : MonoBehaviour
    {
        [SerializeField]
        private ScriptableReplayOptions replayOptions;


        [SerializeField]
        private ScriptableObjects.ScriptableServerGameInformation server;

        [SerializeField]
        private SequenceToggleItem scrollItemTemplate;

        [SerializeField]
        private Transform scrollContentParent;

        public void Awake()
        {
            server.gameStateChangedEvent += OnGameStateChanged;
        }

        public void OnGameStateChanged()
        {
            switch (server.GameState)
            {
                case Utils.ServerGameState.ViewingPlayback:

                    foreach (Transform child in scrollContentParent)
                    {
                        if (!child.gameObject.activeSelf)
                            continue;

                        Destroy(child.gameObject);
                    }

                    foreach (CourseData course in server.Courses)
                    {
                        SequenceToggleItem item = scrollItemTemplate.Create(
                            scrollContentParent,                            
                            course);

                        item.OnSequenceToggledHandler += OnSequenceToggled;
                    }

                    break;
            }
        }

        public void OnSequenceToggled(CourseData course, bool enabled)
        {
            if (replayOptions.OnSequenceToggledHandler != null)
            {
                replayOptions.OnSequenceToggledHandler.Invoke(course, enabled);
            }
        }

    }
}