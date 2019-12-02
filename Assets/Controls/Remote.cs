using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Controls
{
    public class Remote : MonoBehaviour
    {
        [SerializeField]
        private AvatarControllerAsset controllerAsset;

        [SerializeField]
        private TMPro.TextMeshProUGUI distanceText;

        [SerializeField]
        private SpriteRenderer flightIcon;

        [SerializeField]
        private SpriteRenderer wallIcon;

        public void EnableFlight()
        {
            gameObject.SetActive(true);
            flightIcon.gameObject.SetActive(true);
            wallIcon.gameObject.SetActive(false);
        }

        public void EnableWall()
        {
            gameObject.SetActive(true);
            flightIcon.gameObject.SetActive(false);
            wallIcon.gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }


        // Start is called before the first frame update
        public void Awake()
        {
            EnableWall();
            Client.Instance.Algorithm.OnValueChangedHandler += OnAlgorithmChanged;      
        }


        public void OnAlgorithmChanged(Algorithms.Algorithm algorithm)
        {
            switch (algorithm.Id)
            {
                case Algorithms.Id.ShortestFlightDistance:
                    EnableFlight();
                    break;

                case Algorithms.Id.LongestStraight:
                    EnableWall();
                    break;

                default:
                    Disable();
                    break;

            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}