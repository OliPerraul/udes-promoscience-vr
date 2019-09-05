using UnityEngine;
using UnityEditor;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.Game;

namespace UdeS.Promoscience.Editor
{

    [CustomEditor(typeof(ScriptableServerGameInformation))]
    public class ScriptableServerGameStateEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            ScriptableServerGameInformation scriptableGameState = target as ScriptableServerGameInformation;

            if (GUILayout.Button("On Value Changed"))
            {
                scriptableGameState.OnGameStateValueChanged();
            }
        }
    }
}