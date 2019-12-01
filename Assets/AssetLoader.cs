using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience
{
    // TODO
    // ManagerAsset are a bad idea.. (e.g ReplayManagerAsset, GameManagerAsset ..(
    // They are destroyed when changing scene (if not used in the scene destination), and not loaded back in when returning
    // Theses assets are a legacy of the old scriptable object architecture
    // WORK AROUND: This class...

    public class AssetLoader : MonoBehaviour
    {
        [SerializeField]
        ScriptableObject[] assets;

        public void OnValidate()
        {
            if (assets == null || assets.Length == 0)
            {
                assets = Cirrus.AssetDatabase.FindObjectsOfType<ScriptableObject>();
            }
        }
    }
}
