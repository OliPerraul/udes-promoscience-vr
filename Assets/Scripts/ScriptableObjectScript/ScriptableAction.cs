using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Action", order = 1)]
public class ScriptableAction : ScriptableObject
{
    public Action action;

    public void FireAction()
    {
        if (action != null)
        {
            action();
        }
    }
}

