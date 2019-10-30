using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;

namespace UdeS.Promoscience.UI
{
    // TODO: rename, remove, replace??
    // NOTE: we used to ask confirmation from the headset once request for return to divergence is sent
    // from the tablet. We do not do this anymore because it would create confusion from the kid with a headset.
    // To remove it, a workaround is simply to send answer as soon as request is received. 
    // TODO: this should not be handled in the UI bound classes ...
    public class ReturnToDivergenceDisplay : MonoBehaviour
    {
        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        ScriptableBoolean isDiverging;

        [SerializeField]
        ScriptableBoolean returnToDivergencePointAnswer;

        [SerializeField]
        ScriptableAction returnToDivergencePointRequest;

        [SerializeField]
        GameObject confirmationPanel;

        [SerializeField]
        private ScriptableBoolean grabbedMouseFocus;

        private bool init = false;


        void OnEnable()
        {
            if (init) return;

            init = true;

            controls.isControlsEnableValueChangedEvent += OnControlsEnableValueChanged;
            returnToDivergencePointRequest.action += OnReturnToDivergencePointRequest;
            isDiverging.valueChangedEvent += OnIsDivergingValueChanged;
            Client.Instance.clientStateChangedEvent += OnClientStateChanged;

            OnClientStateChanged();
        }

        void OnClientStateChanged()
        {
            switch (Client.Instance.State)
            {
                case ClientGameState.Playing:
                case ClientGameState.PlayingTutorial:
                    transform.GetChild(0).gameObject.SetActive(true);
                    break;

                default:
                    transform.GetChild(0).gameObject.SetActive(false);
                    break;
            }
        }


        void OnControlsEnableValueChanged()
        {
            if (!controls.IsControlsEnabled)
            {
                OnIsDivergingValueChanged();
            }
        }

        void OnReturnToDivergencePointRequest()
        {
            if (isDiverging.Value)
            {
                returnToDivergencePointAnswer.Value = true;
            }
            else
            {
                returnToDivergencePointAnswer.Value = false;
            }
        }

        void OnIsDivergingValueChanged()
        {
            if (confirmationPanel.activeSelf)
            {
                grabbedMouseFocus.Value = true;
                confirmationPanel.SetActive(false);
                controls.IsPlayerControlsEnabled = true;
                returnToDivergencePointAnswer.Value = false;
            }
        }
    }
}
