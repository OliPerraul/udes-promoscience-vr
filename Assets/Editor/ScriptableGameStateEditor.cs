using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableGameState))]
public class ScriptableGameStateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        ScriptableGameState scriptableGameState = target as ScriptableGameState;

        if (GUILayout.Button("On Value Changed"))
        {
            scriptableGameState.OnValueChanged();
        }
    }
}