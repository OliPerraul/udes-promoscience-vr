using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience
{
    public class AudioManager : MonoBehaviour
    { 
        //{
    //    [SerializeField]
    //    ScriptableClientGameState gameState;

        [SerializeField]
        AudioSource musicSource;

        [SerializeField]
        AudioClip classicLoopMusic;

        [SerializeField]
        AudioClip classicLoopWithOutDrumMusic;

        [SerializeField]
        AudioClip modernLoopMusic;

        void Start()
        {
            Client.Instance.State.OnValueChangedHandler += OnGameStateChanged;
        }

        void OnGameStateChanged(ClientGameState state)
        {
            if (Client.Instance.State.Value == ClientGameState.Playing)
            {
                musicSource.clip = classicLoopMusic;
                musicSource.volume = 1f;
                musicSource.Play();
            }
            else if (Client.Instance.State.Value == ClientGameState.PlayingTutorial)
            {
                musicSource.clip = modernLoopMusic;
                musicSource.volume = 0.1f;
                musicSource.Play();
            }
            else
            {
                musicSource.Stop();
            }
        }
    }
}
