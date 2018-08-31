using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextManager : MonoBehaviour
{
    public static UITextManager instance;

    [SerializeField]
    GameObject waitingText;

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

    public void HideText()
    {
        waitingText.SetActive(false);
    }
}
