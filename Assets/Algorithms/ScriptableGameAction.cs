using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.ScriptableObjects
{
    [Serializable]
    public class ActionValue
    {
        [SerializeField]
        public Tile tile;

        [SerializeField]
        public TileColor previousColor;

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
    }

    [CreateAssetMenu(fileName = "Data", menuName = "Data/GameAction", order = 1)]
    public class ScriptableGameAction : ScriptableObject
    {
        [SerializeField]
        private ScriptableClientGameState gameState;
        
        // TODO: fix weird globally available variables (put in client game state)
        // playerPaintTile vs paintingColor?
        [SerializeField]
        private Labyrinths.ScriptableTile playerPaintTile;

        //[SerializeField]
        //private ScriptableTileColor paintingColor;


        [SerializeField]
        private GameAction action;

        DateTime dateTime;

        string value = "{}";

        public Action valueChangedEvent;

        public string Value
        {
            get
            {
                return value;
            }
        }


        public GameAction Action
        {
            get
            {
                return action;
            }
        }

        public string DateTimeString
        {
            get
            {
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
        }

        public void SetAction(GameAction gameAction)
        {
            action = gameAction;

            value = JsonUtility.ToJson(new ActionValue
            {
                respect = gameState.Respect,
                error = gameState.ErrorCount,
                previousColor = playerPaintTile.Tile.color//.Value;//.Tile;
            });

            DateTime actionDateTime = DateTime.Now;

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
                respect = gameState.Respect,
                error = gameState.ErrorCount,
                previousColor = playerPaintTile.TilePreviousColor
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