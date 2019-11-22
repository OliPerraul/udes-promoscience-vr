using UnityEngine;
using UnityEditor;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Network;
using UdeS.Promoscience;
using UdeS.Promoscience.Controls;

namespace UdeS.Promoscience.Editor
{
    [CustomEditor(typeof(AvatarControllerAsset))]
    public class ScriptableControlerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            AvatarControllerAsset scriptableControler = target as AvatarControllerAsset;

            if (GUILayout.Button("On Controls Enable Value Changed"))
            {
                // ??
                scriptableControler.IsPlayerControlsEnabled.OnValueChangedHandler.Invoke(scriptableControler.IsPlayerControlsEnabled.Value);
            }

            if (GUILayout.Button("Stop All Movement"))
            {
                scriptableControler.StopAllMovement();
            }
        }
    }
}