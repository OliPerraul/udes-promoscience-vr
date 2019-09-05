using UnityEngine;
using UnityEditor;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.Game;

namespace UdeS.Promoscience.Editor
{
    [CustomEditor(typeof(ScriptableClientGameState))]
    public class ScriptableClientGameStateEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            ScriptableClientGameState scriptableGameState = target as ScriptableClientGameState;

            if (GUILayout.Button("On Value Changed"))
            {
                scriptableGameState.OnValueChanged();
            }
        }
    }
}