using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Cirrus
{
    public static class AssetDatabase
    {
        public static string CurrentFolder
        {
            get
            {
#if UNITY_EDITOR
                var path = "";
                var obj = Selection.activeObject;
                if (obj == null) path = "Assets";
                else path = UnityEditor.AssetDatabase.GetAssetPath(obj.GetInstanceID());
                if (path.Length > 0)
                {
                    if (Directory.Exists(path))
                    {
                        return path;
                    }
                    else
                    {
                        return path;
                    }
                }
                else
                {
                    return "";
                }
#endif

                return "";
            }
        }

        //public static T FindObjectOfType<T>()
        //{
        //    UnityEditor.AssetDatabase.F
        //}
        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// </summary>
        public static T CreateAsset<T>(string path) where T : ScriptableObject
        {
#if UNITY_EDITOR
            T asset = ScriptableObject.CreateInstance<T>();

            string assetPathAndName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/" + path + ".asset");

            UnityEditor.AssetDatabase.CreateAsset(asset, assetPathAndName);

            UnityEditor.AssetDatabase.SaveAssets();

            UnityEditor.AssetDatabase.Refresh();

            UnityEditor.EditorUtility.FocusProjectWindow();

            UnityEditor.Selection.activeObject = asset;

            UnityEditor.EditorUtility.SetDirty(asset);

            return asset;
#endif
            return null;
        }



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
