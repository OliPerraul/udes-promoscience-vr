using UnityEngine;
using System.Collections;
using System;
using Cirrus;

namespace UdeS.Promoscience
{

    // TODO client settings?
    [Serializable]
    public class ServerSettings
    {
        // TODO expose in menu
        // Would be useful when working on the level editor
        [SerializeField]
        private bool createSampleLabyrinths = false;

        public bool CreateSampleLabyrinths => createSampleLabyrinths;

        public const string PredefinedLevelOrderString = "PredefinedLevelOrder";

        [SerializeField]
        public ObservableBool IsLevelOrderPredefined = new ObservableBool(false);

        public const int MinNumberOfRounds = 3;

        public const int MaxNumberOfRounds = 10;

        public const string NumberOfRoundsString = "NumberOfRounds";

        [SerializeField]
        public ObservableInt NumberOfRounds = new ObservableInt(MinNumberOfRounds);

        public ServerSettings()
        {
            IsLevelOrderPredefined.OnValueChangedHandler +=
                (x) => OnSettingChanged(PredefinedLevelOrderString, x);

            NumberOfRounds.OnValueChangedHandler +=
                (x) => OnSettingChanged(NumberOfRoundsString, Mathf.Clamp(x, MinNumberOfRounds, MaxNumberOfRounds));
        }

        public void OnSettingChanged(string setting, bool enabled)
        {
            PlayerPrefs.SetInt(setting, enabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void OnSettingChanged(string setting, int value)
        {
            PlayerPrefs.SetInt(setting, value);
            PlayerPrefs.Save();
        }

        public void LoadFromPlayerPrefs()
        {
            if (PlayerPrefs.HasKey(PredefinedLevelOrderString))
                IsLevelOrderPredefined.Set(PlayerPrefs.GetInt(PredefinedLevelOrderString) == 1, notify: false);

            if (PlayerPrefs.HasKey(NumberOfRoundsString))
                NumberOfRounds.Set(PlayerPrefs.GetInt(NumberOfRoundsString), notify: false);
        }
    }
}