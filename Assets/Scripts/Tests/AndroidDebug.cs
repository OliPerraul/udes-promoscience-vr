using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//from : https://answers.unity.com/questions/1063391/unet-matchmaking-and-lan-not-working-on-android.html
public class AndroidDebug : MonoBehaviour
{
    [SerializeField]
    Text debugText;

    ////using the awake method in a random gameobject on the scene, subscribe to this event by assigning it an event handler. 
    void Awake()
    {
        Application.logMessageReceived += Application_logMessageReceived;
    }
    //define the event handler that displays the exception on the UI text component 
    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        debugText.text = string.Format("{0}, {1}, {2}", condition, stackTrace, type);
    }
}
