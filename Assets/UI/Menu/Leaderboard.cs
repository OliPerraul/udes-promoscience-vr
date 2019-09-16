using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.UI
{
    public class Leaderboard : MonoBehaviour
    {
        [SerializeField]
        private GameObject column1;

        [SerializeField]
        private GameObject column2;

        public void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
