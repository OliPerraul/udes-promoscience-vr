using UnityEngine;
using UnityEditor;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Network;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Editor
{
    [CustomEditor(typeof(ScriptableString))]
    public class ScriptableStringEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            ScriptableString scriptableString = target as ScriptableString;

            if (GUILayout.Button("On Value Changed"))
            {
                scriptableString.OnValueChanged();
            }
        }
    }
}