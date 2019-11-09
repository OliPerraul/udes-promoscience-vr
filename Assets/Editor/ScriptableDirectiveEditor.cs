//using UnityEngine;
//using UnityEditor;

//using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Network;
//using UdeS.Promoscience;

//namespace UdeS.Promoscience.Editor
//{
//    [CustomEditor(typeof(DirectiveManagerAsset))]
//    public class ScriptableDirectiveEditor : UnityEditor.Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();

//            GUI.enabled = Application.isPlaying;

//            DirectiveManagerAsset scriptableGameState = target as DirectiveManagerAsset;

//            if (GUILayout.Button("On Value Changed"))
//            {
//                scriptableGameState.On();
//            }
//        }
//    }
//}