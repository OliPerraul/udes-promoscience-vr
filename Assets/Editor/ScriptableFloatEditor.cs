using UnityEngine;
using UnityEditor;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Network;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Editor
{

    [CustomEditor(typeof(ScriptableFloat))]
    public class ScriptableFloatEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            ScriptableFloat scriptableFloat = target as ScriptableFloat;

            if (GUILayout.Button("On Value Changed"))
            {
                scriptableFloat.OnValueChanged();
            }
        }
    }
}