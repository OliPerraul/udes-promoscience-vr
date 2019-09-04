using UnityEngine;
using UnityEditor;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.Game;

namespace UdeS.Promoscience.Editor
{

    [CustomEditor(typeof(ScriptableGameAction))]
    public class ScriptableGameActionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            ScriptableGameAction scriptableGameState = target as ScriptableGameAction;

            if (GUILayout.Button("On Value Changed"))
            {
                scriptableGameState.OnValueChanged();
            }
        }
    }
}