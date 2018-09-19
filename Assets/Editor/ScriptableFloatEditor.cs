using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableFloat))]
public class ScriptableFloatEditor : Editor
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