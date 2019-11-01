using UnityEngine;
using System.Collections;

namespace Cirrus
{
    public class Singleton : MonoBehaviour { }

    public class BaseSingleton<T> : Singleton where T : Singleton
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }

                return _instance;
            }
        }
    }
}