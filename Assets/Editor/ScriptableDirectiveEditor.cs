using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableDirective))]
public class ScriptableDirectiveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        ScriptableDirective scriptableGameState = target as ScriptableDirective;

        if (GUILayout.Button("On Value Changed"))
        {
            scriptableGameState.OnValueChanged();
        }
    }
}