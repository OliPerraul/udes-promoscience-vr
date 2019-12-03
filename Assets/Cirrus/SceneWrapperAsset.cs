using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Cirrus
{
    [CreateAssetMenu(menuName = "Cirrus/Scene Wrapper")]
    public class SceneWrapperAsset : ScriptableObject
    {
        [SerializeField]
        private Object _scene;

        [HideInInspector]
        [SerializeField]
        private string _scenePath;

        public void Load()
        {
            SceneManager.LoadScene(_scenePath);
        }

        public void OnValidate()
        {
#if UNITY_EDITOR
            if (_scene != null)
            {
                _scenePath = UnityEditor.AssetDatabase.GetAssetPath(_scene.GetInstanceID());
            }
#endif
        }
    }
}