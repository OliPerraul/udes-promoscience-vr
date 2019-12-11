using Cirrus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UdeS.Promoscience.Controls;
using UnityEngine;


namespace UdeS.Promoscience.UI
{

    public class TabletCommandsInterface : MonoBehaviour
    {
        [SerializeField]
        private Controls.ControlsAsset controls;

        [SerializeField]
        private Algorithms.AlgorithmRespectAsset algorithmRespect;

        [SerializeField]
        private UnityEngine.UI.Button returnToDivergentButton;

        [SerializeField]
        private UnityEngine.UI.Button howToButton;

        [SerializeField]
        private UnityEngine.UI.Button fixItButton;

        [SerializeField]
        private UnityEngine.UI.Button cameraButton;


        public void Awake()
        {
            fixItButton.onClick.AddListener(
                () =>
                    algorithmRespect.IsCorrectingEnabled.Value =
                    !algorithmRespect.IsCorrectingEnabled.Value);

            howToButton.onClick.AddListener(
                () =>
                    algorithmRespect.IsCorrectingEnabled.Value =
                    !algorithmRespect.IsCorrectingEnabled.Value);

            cameraButton.onClick.AddListener(
                () =>
                    controls.TabletCameraMode.Value = (TabletCameraMode)((int)controls.TabletCameraMode.Value + 1).Mod(TabletUtils.NumCameraMode));
        }


    }
}