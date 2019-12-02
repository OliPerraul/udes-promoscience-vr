using UnityEngine;
using System.Collections;


namespace UdeS.Promoscience
{
    public class Settings : Cirrus.BaseSingleton<Settings>
    {
        public void Awake()
        {
            if (_instance != null)
            {
                DestroyImmediate(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        [SerializeField]
        public LevelSelectionMode LevelSelectionMode;



    }
}
