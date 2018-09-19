using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableString))]
public class ScriptableStringEditor : Editor
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