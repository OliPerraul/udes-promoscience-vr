using UnityEngine;
using UnityEditor;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Network;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Editor
{
    [CustomEditor(typeof(ScriptableControler))]
    public class ScriptableControlerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            ScriptableControler scriptableControler = target as ScriptableControler;

            if (GUILayout.Button("On Controls Enable Value Changed"))
            {
                scriptableControler.OnPlayerControlsEnableValueChanged();
            }

            if (GUILayout.Button("Stop All Movement"))
            {
                scriptableControler.StopAllMovement();
            }
        }
    }
}