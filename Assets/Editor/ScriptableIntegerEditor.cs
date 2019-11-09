//using UnityEngine;
//using UnityEditor;

//using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Network;
//using UdeS.Promoscience;

//namespace UdeS.Promoscience.Editor
//{

//    [CustomEditor(typeof(GameRoundManagerAsset))]
//    public class ScriptableIntegerEditor : UnityEditor.Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();

//            GUI.enabled = Application.isPlaying;

//            GameRoundManagerAsset scriptableInteger = target as GameRoundManagerAsset;

//            if (GUILayout.Button("On Value Changed"))
//            {
//                scriptableInteger.OnValueChanged();
//            }
//        }
//    }
//}