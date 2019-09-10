using UdeS.Promoscience.Network;
using UnityEditor;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Network;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Editor
{
    [CustomEditor(typeof(ClientNetworkDiscovery))]
    public class ClientNetworkDiscoveryEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}