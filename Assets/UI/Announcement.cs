using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.UI
{
    public class Announcement : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Text announcementText;

        [SerializeField]
        private float punchScaleAmount = 0.6f;

        [SerializeField]
        private float punchScaleTime = 1f;

        private Timer timer;

        [SerializeField]
        private float announcementTime = 5f;


        public void Awake()
        {
            timer = new Timer(announcementTime, start: false, repeat: false);
            timer.OnTimeLimitHandler += OnAnnouncementTimeOut;
            gameObject.SetActive(false);
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
                gameObject.SetActive(true);
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
            gameObject.SetActive(false);
        }

    }
}
