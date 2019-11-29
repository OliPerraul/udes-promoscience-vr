using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.UI
{

    //This class is a temporary UI class, should change with scriptable objects
    public class TextManager : MonoBehaviour
    {
        public static TextManager instance;

        [SerializeField]
        GameObject waitingText;

        [SerializeField]
        GameObject messageText;

        [SerializeField]
        GameObject readyButton;

        [SerializeField]
        GameObject messageButtonGroup;

        void OnEnable()
        {
            if (instance == null)
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

        public void ShowMessageButtonGroup()
        {
            messageButtonGroup.SetActive(true);
        }

        public void HideMessageButtonGroup()
        {
            messageButtonGroup.SetActive(false);
        }
    }
}
