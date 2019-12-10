using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Menu
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Toggle predefinedLevelOrderToggle;

        [SerializeField]
        private UnityEngine.UI.Toggle generateSampleLevelsToggle;

        [SerializeField]
        private IntFieldSetting numberOfRoundField;

        // Start is called before the first frame update
        void Awake()
        {
            predefinedLevelOrderToggle.onValueChanged.AddListener(
                (x) => Server.Instance.Settings.IsLevelOrderPredefined.Value = x);

            generateSampleLevelsToggle.onValueChanged.AddListener(
                (x) => Server.Instance.Settings.CreateSampleLabyrinths.Value = x);

            numberOfRoundField.Value.OnValueChangedHandler += (x) =>
                Server.Instance.Settings.NumberOfRounds.Value = x;
           
        }

        private void Start()
        {
            numberOfRoundField.Value.Set(Server.Instance.Settings.NumberOfRounds.Value, notify: true);
            predefinedLevelOrderToggle.isOn = Server.Instance.Settings.IsLevelOrderPredefined.Value;
        }

    }
}