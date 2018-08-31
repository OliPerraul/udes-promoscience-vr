﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class is a temporary UI class, should change with scriptable objects
public class UITextManager : MonoBehaviour
{
    public static UITextManager instance;

    [SerializeField]
    GameObject waitingText;

    [SerializeField]
    GameObject messageText;

    [SerializeField]
    GameObject readyButton;

    void Start ()
    {
	    if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
	}

    public void SetText(string s)
    {
        waitingText.GetComponent<Text>().text = s;
    }

    public void SetMessageText(string s)
    {
        messageText.GetComponent<Text>().text = s;
    }

    public void HideText()
    {
        waitingText.SetActive(false);
    }

    public void ShowReadyButton()
    {
        readyButton.SetActive(true);
    }

    public void HideReadyButton()
    {
        readyButton.SetActive(false);
    }
}
