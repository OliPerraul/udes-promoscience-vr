using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/LocalizeString", order = 1)]
public class ScriptableLocalizeString : ScriptableObject
{
    [SerializeField]
    string english;

    [SerializeField]
    string french;

    public string Value
    {
        get
        {
            if (Application.systemLanguage == SystemLanguage.French)
            {
                return french;
            }
            else
            {
                return english;
            }
        }
    }
}

