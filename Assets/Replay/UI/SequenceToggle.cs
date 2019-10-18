using UnityEngine;
using System.Collections;
using System.Linq;

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

        private SequenceToggleItem firstItem = null;

        public void Awake()
        {
            server.gameStateChangedEvent += OnGameStateChanged;
        }

        public void OnGameStateChanged()
        {
            switch (server.GameState)
            {
                case Promoscience.Utils.ServerGameState.IntermissionReplay:

                    firstItem = null;

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

                        firstItem = item;
                    }

                    break;
            }
        }
    }
}