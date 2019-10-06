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

                    foreach (Course course in server.Courses)
                    {
                        if (course == null)
                            continue;

                        SequenceToggleItem item = scrollItemTemplate.Create(
                            scrollContentParent,                            
                            course);
                    }

                    break;
            }
        }
    }
}