using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableClientGameState))]
public class ScriptableClientGameStateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        ScriptableClientGameState scriptableGameState = target as ScriptableClientGameState;

        if (GUILayout.Button("On Value Changed"))
        {
            scriptableGameState.OnValueChanged();
        }
    }
}