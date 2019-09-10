using UnityEngine;
using UnityEditor;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Network;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Editor
{

    [CustomEditor(typeof(ScriptableInteger))]
    public class ScriptableIntegerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            ScriptableInteger scriptableInteger = target as ScriptableInteger;

            if (GUILayout.Button("On Value Changed"))
            {
                scriptableInteger.OnValueChanged();
            }
        }
    }
}