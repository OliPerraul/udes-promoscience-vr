using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableServerGameInformation))]
public class ScriptableServerGameStateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        ScriptableServerGameInformation scriptableGameState = target as ScriptableServerGameInformation;

        if (GUILayout.Button("On Value Changed"))
        {
            scriptableGameState.OnGameStateValueChanged();
        }
    }
}