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


    public class ReplayControlsAsset : MonoBehaviour
    {
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

        public Cirrus.Event<float> OnPlaybackSpeedHandler;

        public Cirrus.ObservableValue<float> SlideValue = new Cirrus.ObservableValue<float>();

        public Cirrus.Event<ReplayControlAction> OnControlActionHandler;
    }
}