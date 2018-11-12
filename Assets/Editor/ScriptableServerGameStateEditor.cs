using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableServerGameState))]
public class ScriptableServerGameStateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        ScriptableServerGameState scriptableGameState = target as ScriptableServerGameState;

        if (GUILayout.Button("On Value Changed"))
        {
            scriptableGameState.OnValueChanged();
        }
    }
}