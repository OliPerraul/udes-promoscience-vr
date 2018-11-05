using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableGameAction))]
public class ScriptableGameActionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        ScriptableGameAction scriptableGameState = target as ScriptableGameAction;

        if (GUILayout.Button("On Value Changed"))
        {
            scriptableGameState.OnValueChanged();
        }
    }
}