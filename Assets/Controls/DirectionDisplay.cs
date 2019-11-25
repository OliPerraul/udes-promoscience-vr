﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Controls;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{

    public class DirectionDisplay : MonoBehaviour
    {
        [SerializeField]
        AvatarControllerAsset controls;

        [SerializeField]
        GameObject directionDisplayer;

        void Awake()
        {
            Client.Instance.clientStateChangedEvent += OnClientStateChanged;

            OnClientStateChanged();
        }
        
        void OnClientStateChanged()
        {
            switch (Client.Instance.State)
            {
                case ClientGameState.Playing:
                case ClientGameState.PlayingTutorial:
                    directionDisplayer.SetActive(true);
                    break;

                default:
                    directionDisplayer.SetActive(false);
                    break;

            }
        }
    }
}