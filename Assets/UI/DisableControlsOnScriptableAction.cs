//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience;

//namespace UdeS.Promoscience.UI
//{
//    public class DisableControlsOnScriptableAction : MonoBehaviour
//    {
//        [SerializeField]
//        ScriptableAction scriptableAction;

//        [SerializeField]
//        AvatarControllerAsset controls;

//        void Start()
//        {
//            scriptableAction.action += OnScriptableAction;
//        }

//        void OnScriptableAction()
//        {
//            controls.IsPlayerControlsEnabled.Value = false;
//        }
//    }
//}
