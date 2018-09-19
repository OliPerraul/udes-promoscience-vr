using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableInteger))]
public class ScriptableIntegerEditor : Editor
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