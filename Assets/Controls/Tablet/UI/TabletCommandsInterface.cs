﻿using Cirrus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UdeS.Promoscience.Controls;
using UnityEngine;


namespace UdeS.Promoscience.UI
{
    /// <summary>
    /// Tablet commands button interface located in the corner of the screen
    /// </summary>
    public class TabletCommandsInterface : MonoBehaviour
    {
        [SerializeField]
        private TabletCommandsAsset asset;

        [SerializeField]
        private HeadsetControlsAsset controls;

        [SerializeField]
        private TabletControlsAsset tabletControls;

        [SerializeField]
        private Algorithms.AlgorithmRespectAsset algorithmRespect;

        [SerializeField]
        private UnityEngine.UI.Button returnToDivergentButton;

        [SerializeField]
        private UnityEngine.UI.Image returnToDivergentButtonBG;

        private float initBGAslpha;

        [SerializeField]
        private UnityEngine.UI.Image returnToDivergentButtonICON;

        private float initIconAlph;



        [SerializeField]
        private UnityEngine.UI.Button howToButton;

        [SerializeField]
        private UnityEngine.UI.Button fixItButton;

        [SerializeField]
        private UnityEngine.UI.Button cameraButton;


        public void Awake()
        {
            initBGAslpha = returnToDivergentButtonBG.color.a;
            initIconAlph = returnToDivergentButtonICON.color.a;
            OnIsDivergin(false);
            algorithmRespect.IsDiverging.OnValueChangedHandler += OnIsDivergin;
            

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
                    tabletControls.TabletCameraMode.Value = (TabletCameraMode)((int)tabletControls.TabletCameraMode.Value + 1).Mod(TabletUtils.NumCameraMode));

            returnToDivergentButton.onClick.AddListener(() =>
            {
                asset.OnReturnToDivergentRequestHandler?.Invoke();
            }
            );


            //contro

        }


        public void OnIsDivergin(bool divergin)
        {
            returnToDivergentButtonBG.color = !divergin ? 
                returnToDivergentButtonBG.color.SetA(initBGAslpha / 3) :
                returnToDivergentButtonBG.color.SetA(initBGAslpha);

            returnToDivergentButtonICON.color = !divergin ?
                returnToDivergentButtonICON.color.SetA(initIconAlph / 3) :
                returnToDivergentButtonICON.color.SetA(initIconAlph);

            returnToDivergentButton.interactable = divergin;
        }


    }
}