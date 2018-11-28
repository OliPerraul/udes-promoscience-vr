using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/GameAction", order = 1)]
public class ScriptableGameAction : ScriptableObject
{
    [SerializeField]
    GameAction action;

    DateTime dateTime;

    public Action valueChangedEvent;

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

    public void SetAction(GameAction gameAction)
    {
        action = gameAction;

        DateTime actionDateTime = DateTime.Now;

        if (actionDateTime == dateTime)//Doesn't seems o be working, there is event that have the same milliseconds
        {
            Debug.Log("simultaneous actions");//temp
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

