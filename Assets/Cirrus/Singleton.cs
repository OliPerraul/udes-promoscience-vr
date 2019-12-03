using UnityEngine;
using System.Collections;

namespace Cirrus
{
    public class Singleton : MonoBehaviour { }

    public class BaseSingleton<T> : Singleton where T : Singleton
    {
        protected static T _instance;

        public bool Exists
        {
            get {
                return _instance != null;
            }
        }

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


        public void Persist()
        {
            if (_instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            _instance = Instance;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

    }
}