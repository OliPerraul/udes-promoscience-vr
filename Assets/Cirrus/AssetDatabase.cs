using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cirrus
{

    public static class AssetDatabase
    {
        //public static T FindObjectOfType<T>()
        //{
        //    UnityEditor.AssetDatabase.F
        //}



        public static T FindObjectOfType<T>() where T : UnityEngine.ScriptableObject
        {
#if UNITY_EDITOR

            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
#endif            

            return null;
        }


        public static Object FindObjectOfType(System.Type type)
        {
#if UNITY_EDITOR

            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:" + type.Name);  //FindAssets uses tags check documentation for more info
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            return UnityEditor.AssetDatabase.LoadAssetAtPath(path, type);
#endif            

            return null;
        }

        public static T[] FindObjectsOfType<T>() where T : UnityEngine.ScriptableObject
        {
#if UNITY_EDITOR

            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T));  //FindAssets uses tags check documentation for more info

            List<T> assets = new List<T>();

            foreach (var guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                assets.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path));
            }

            return assets.ToArray();
#endif    
            return null;
        }



        //// Update is called once per frame
        //void Update()
        //{



        //}
    }
}
