using UnityEngine;
using UnityEditor;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.Game;

namespace UdeS.Promoscience.Editor
{
    [CustomEditor(typeof(ScriptableBoolean))]
    public class ScriptableBooleanEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            ScriptableBoolean scriptableBoolean = target as ScriptableBoolean;

            if (GUILayout.Button("On Value Changed"))
            {
                scriptableBoolean.OnValueChanged();
            }
        }
    }
}