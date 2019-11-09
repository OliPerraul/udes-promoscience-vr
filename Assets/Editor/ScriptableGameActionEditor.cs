using UnityEngine;
using UnityEditor;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Network;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Editor
{

    [CustomEditor(typeof(GameActionManagerAsset))]
    public class ScriptableGameActionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            GameActionManagerAsset scriptableGameState = target as GameActionManagerAsset;

            if (GUILayout.Button("On Value Changed"))
            {
                scriptableGameState.OnValueChanged();
            }
        }
    }
}