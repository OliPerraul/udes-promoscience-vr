﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    ScriptableClientGameState gameState;

    [SerializeField]
    AudioSource musicSource;

    [SerializeField]
    AudioClip classicLoopMusic;

    [SerializeField]
    AudioClip classicLoopWithOutDrumMusic;

    [SerializeField]
    AudioClip modernLoopMusic;

    void Start ()
    {
        gameState.valueChangedEvent += OnGameStateChanged;
	}
	
    void OnGameStateChanged()
    {
        if(gameState.Value == ClientGameState.Playing)
        {
            musicSource.clip = classicLoopMusic;
            musicSource.volume = 1f;
            musicSource.Play();
        }
        else if (gameState.Value == ClientGameState.PlayingTutorial)
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