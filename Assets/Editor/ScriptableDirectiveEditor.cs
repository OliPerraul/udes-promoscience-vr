using UnityEngine;
using UnityEditor;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.Game;

namespace UdeS.Promoscience.Editor
{
    [CustomEditor(typeof(ScriptableDirective))]
    public class ScriptableDirectiveEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            ScriptableDirective scriptableGameState = target as ScriptableDirective;

            if (GUILayout.Button("On Value Changed"))
            {
                scriptableGameState.OnValueChanged();
            }
        }
    }
}