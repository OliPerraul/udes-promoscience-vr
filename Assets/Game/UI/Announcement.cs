using System;
using System.Collections;
using System.Collections.Generic;
using UdeS.Promoscience.ScriptableObjects;
using UnityEngine;

namespace UdeS.Promoscience.UI
{
    public class Announcement : MonoBehaviour
    {
        //[SerializeField]
        //ScriptableClientGameState client;

        [SerializeField]
        private UnityEngine.UI.Text announcementText;

        [SerializeField]
        private float punchScaleAmount = 0.6f;

        [SerializeField]
        private float punchScaleTime = 1f;

        private Timer timer;

        [SerializeField]
        private float announcementTime = 5f;

        [SerializeField]
        private LocalizeInlineString tutorialString;

        [SerializeField]
        private LocalizeInlineString roundString;


        public void Awake()
        {
            //timer = new Timer(announcementTime, start: false, repeat: false);

            //gameRound.Round.OnValueChangedHandler += OnGameRoundValueChanged;
            //timer.OnTimeLimitHandler += OnAnnouncementTimeOut;
            //Client.Instance.clientStateChangedEvent += OnGameStateChanged;

            transform.GetChild(0).gameObject.SetActive(false);
         }







        private void OnGameStateChanged()
        {
            //if (Client.Instance.State == ClientGameState.TutorialLabyrinthReady ||
            //    Client.Instance.State == ClientGameState.PlayingTutorial)
            //{
            //    Message = tutorialString.Value + "\n" +
            //         "(" + Client.Instance.Algorithm.Name + ")";
            //}
            //else if (Client.Instance.State == ClientGameState.LabyrinthReady ||
            //    Client.Instance.State == ClientGameState.Playing
            //    )
            //{
            //    Message = roundString.Value + " " + gameRound.Round.Value.ToString() + "\n" +
            //        "(" + Client.Instance.Algorithm.Name + ")";
            //}

        }

        public void OnGameRoundValueChanged(int round)
        {    
            //Message = roundString.Value + " " + gameRound.Value.ToString() + "\n" +
            //     "(" + algorithm.Name + ")";
        }


        IEnumerator PunchValue()
        {
            iTween.Stop(announcementText.gameObject);
            announcementText.gameObject.transform.localScale = new Vector3(1, 1, 1);

            yield return new WaitForSeconds(0.01f);

            iTween.PunchScale(announcementText.gameObject,
                new Vector3(punchScaleAmount, punchScaleAmount, punchScaleAmount),
                punchScaleTime);

            yield return null;
        }

        private string message;

        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                transform.GetChild(0).gameObject.SetActive(true);
                message = value;
                announcementText.text = message;

                iTween.Init(announcementText.gameObject);
                iTween.Stop(announcementText.gameObject);
                announcementText.gameObject.transform.localScale = new Vector3(1, 1, 1);

                StartCoroutine(PunchValue());
                timer.Start();                
            }
        }

        public void OnAnnouncementTimeOut()
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

    }
}
