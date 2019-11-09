//using UnityEngine;
//using UnityEditor;

//using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Network;
//using UdeS.Promoscience;

//namespace UdeS.Promoscience.Editor
//{
//    namespace UdeS.Promoscience.Editor
//    {
//        [CustomEditor(typeof(ScriptableAction))]
//        public class ScriptableActionEditor : UnityEditor.Editor
//        {
//            public override void OnInspectorGUI()
//            {
//                base.OnInspectorGUI();

//                GUI.enabled = Application.isPlaying;

//                ScriptableAction scriptableAction = target as ScriptableAction;

//                if (GUILayout.Button("Fire Action"))
//                {
//                    scriptableAction.FireAction();
//                }
//            }
//        }
//    }
//}
