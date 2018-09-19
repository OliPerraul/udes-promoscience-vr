using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableBoolean))]
public class ScriptableBooleanEditor : Editor
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