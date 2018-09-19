using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableControler))]
public class ScriptableControlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        ScriptableControler scriptableControler = target as ScriptableControler;

        if (GUILayout.Button("On Controls Enable Value Changed"))
        {
            scriptableControler.OnControlsEnableValueChanged();
        }

        if (GUILayout.Button("Stop All Movement"))
        {
            scriptableControler.StopAllMovement();
        }
    }
}