using UnityEngine;
using System.Collections;
using System.Linq;

namespace UdeS.Promoscience
{
    public abstract class Resources : ScriptableObject { }

    public abstract class BaseResources<T> : Resources where T : Resources
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = UnityEngine.Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                }
                               
                return _instance;
            }
        }
    }
    
    public class ResourcesUser : MonoBehaviour
    {
        [SerializeField]
        private Resources[] _resources;

        public void OnValidate()
        {
            if (_resources == null || _resources.Length == 0)
                _resources = Cirrus.AssetDatabase.FindObjectsOfType<Resources>();
        }
    }
}