using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableAction))]
public class ScriptableActionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        ScriptableAction scriptableAction = target as ScriptableAction;

        if (GUILayout.Button("Fire Action"))
        {
            scriptableAction.FireAction();
        }
    }
}