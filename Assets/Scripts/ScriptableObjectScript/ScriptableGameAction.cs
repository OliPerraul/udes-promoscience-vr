using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/GameAction", order = 1)]
    public class ScriptableGameAction : ScriptableObject
    {
        [SerializeField]
        GameAction action;

        DateTime dateTime;

        int backtrack = 0;

        public Action valueChangedEvent;



        public int Backtrack
        {
            get
            {
                return backtrack;
            }
        }


        public GameAction Action
        {
            get
            {
                return action;
            }
        }

        public String DateTimeString
        {
            get
            {
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
        }

        public void SetAction(GameAction gameAction, int backtrack=0)
        {
            action = gameAction;

            this.backtrack = backtrack;

            DateTime actionDateTime = DateTime.Now;

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