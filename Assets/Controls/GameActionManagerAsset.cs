﻿using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.Controls;

namespace UdeS.Promoscience.ScriptableObjects
{
    [Serializable]
    public class ActionValue
    {
        [SerializeField]
        public Tile tile;

        [SerializeField]
        public TileColor color;

        [SerializeField]
        public Tile[] wrongTiles;

        [SerializeField]
        public Vector2Int position;

        [SerializeField]
        public Quaternion rotation;

        [SerializeField]
        public Tile[] playerSteps;

        [SerializeField]
        public float respect = 0;

        [SerializeField]
        public int error = 0;

        [SerializeField]
        public int elapsedSeconds = 0;
    }

    [CreateAssetMenu(fileName = "Data", menuName = "Data/GameAction", order = 1)]
    public class GameActionManagerAsset : ScriptableObject
    {
        [SerializeField]
        private Algorithms.AlgorithmRespectAsset algorithmRespect;

        [SerializeField]
        private AvatarControllerAsset controls;


        [SerializeField]
        private GameAction action;

        DateTime dateTime;

        string value = "{}";

        public Action valueChangedEvent;

        public string Value => value;

        public GameAction Action => action;

        public string DateTimeString => dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");


        public void SetAction(GameAction gameAction)
        {
            action = gameAction;

            DateTime actionDateTime = DateTime.Now;

            TimeSpan elapsed = actionDateTime.Subtract(Client.Instance.RoundBeginTime);

            value = JsonUtility.ToJson(new ActionValue
            {
                respect = algorithmRespect.Respect,
                error = algorithmRespect.ErrorCount,
                color = controls.PlayerPaintTile.Value.Color,
                elapsedSeconds = Convert.ToInt32(elapsed.TotalSeconds)
            });

            if (actionDateTime == dateTime)//Doesn't seems o be working, there is event that have the same milliseconds
            {
                actionDateTime = actionDateTime.AddMilliseconds(1);//Used to avoid simultaneous actions
            }

            dateTime = actionDateTime;

            OnValueChanged();
        }        

        public void SetAction(GameAction gameAction, Vector2Int position, Quaternion rotation, Tile[] tiles, Tile[] playerSteps)
        {
            action = gameAction;            
                
            DateTime actionDateTime = DateTime.Now;

            value = JsonUtility.ToJson(new ActionValue
            {
                wrongTiles = tiles, // corrections
                position = position,
                rotation = rotation,
                playerSteps = playerSteps,
                respect = algorithmRespect.Respect,
                error = algorithmRespect.ErrorCount,
                color = controls.PlayerPaintTile.Value.Color
            });

            if (actionDateTime == dateTime)//Doesn't seems o be working, there is event that have the same milliseconds
            {
                actionDateTime = actionDateTime.AddMilliseconds(1);//Used to avoid simultaneous actions
            }

            dateTime = actionDateTime;

            OnValueChanged();
        }

        public void OnValueChanged()
        {
            if (valueChangedEvent != null)
            {
                valueChangedEvent();
            }
        }
    }

}