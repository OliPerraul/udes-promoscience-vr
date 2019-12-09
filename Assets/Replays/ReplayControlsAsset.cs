using UnityEngine;
using System.Collections;


namespace UdeS.Promoscience.Replays
{
    public enum ReplayControlAction
    {
        Previous,
        Next,
        Play,
        Stop,
        Pause,
        Resume,
        Slide,
        Reset,
    }


    public class ReplayControlsAsset : ScriptableObject
    {
        public Cirrus.Event<int> ReplayMoveIndexChangedHandler;

        [SerializeField]
        public Cirrus.ObservableInt ReplayMoveCount = new Cirrus.ObservableInt(0);


        public Cirrus.Event<float> OnPlaybackSpeedHandler;

        [SerializeField]
        public Cirrus.ObservableInt SlideValue = new Cirrus.ObservableInt(0);

        public Cirrus.Event<ReplayControlAction> OnControlActionHandler;

        private float playbackSpeed = 1f;

        public float PlaybackSpeed
        {
            get => playbackSpeed;

            set
            {
                playbackSpeed = value;
                if (playbackSpeed > Utils.MaxPlaybackSpeed) playbackSpeed = Utils.MinPlaybackSpeed;
                if (playbackSpeed < Utils.MinPlaybackSpeed) playbackSpeed = Utils.MaxPlaybackSpeed;
                if (OnPlaybackSpeedHandler != null)
                {
                    OnPlaybackSpeedHandler.Invoke(playbackSpeed);
                }
            }
        }

        public void OnEnable()
        {
            ReplayMoveCount = new Cirrus.ObservableInt(0);
            SlideValue = new Cirrus.ObservableInt(0);
        }


        public void SendAction(ReplayControlAction action)
        {
            OnControlActionHandler?.Invoke(action);
        }
    }
}