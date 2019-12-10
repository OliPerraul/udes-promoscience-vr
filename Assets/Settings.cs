using UnityEngine;
using System.Collections;
using System;
using Cirrus;

namespace UdeS.Promoscience
{
    public class Settings
    {
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
    }


    // TODO client settings?
    [Serializable]
    public class ClientSettings : Settings
    {
        public const string IsLeftHandedString = "IsLeftHanded";

        [SerializeField]
        public ObservableBool IsLeftHanded = new ObservableBool(false);


        public ClientSettings()
        {
            IsLeftHanded.OnValueChangedHandler +=
                (x) => OnSettingChanged(IsLeftHandedString, x);
        }

        public void LoadFromPlayerPrefs()
        {
            if (PlayerPrefs.HasKey(IsLeftHandedString))
                IsLeftHanded.Set(PlayerPrefs.GetInt(IsLeftHandedString) == 1, notify: false);

        }
    }

    [Serializable]
    public class ServerSettings : Settings
    {
        public const string CreateSampleLabyrinthsString = "CreateSampleLabyrinths";

        [SerializeField]
        public ObservableBool CreateSampleLabyrinths = new ObservableBool(false);

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
            CreateSampleLabyrinths.OnValueChangedHandler +=
                (x) => OnSettingChanged(CreateSampleLabyrinthsString, x);

            IsLevelOrderPredefined.OnValueChangedHandler +=
                (x) => OnSettingChanged(PredefinedLevelOrderString, x);

            NumberOfRounds.OnValueChangedHandler +=
                (x) => OnSettingChanged(NumberOfRoundsString, Mathf.Clamp(x, MinNumberOfRounds, MaxNumberOfRounds));
        }

        public void LoadFromPlayerPrefs()
        {
            if (PlayerPrefs.HasKey(CreateSampleLabyrinthsString))
                CreateSampleLabyrinths.Set(PlayerPrefs.GetInt(CreateSampleLabyrinthsString) == 1, notify: false);

            if (PlayerPrefs.HasKey(PredefinedLevelOrderString))
                IsLevelOrderPredefined.Set(PlayerPrefs.GetInt(PredefinedLevelOrderString) == 1, notify: false);

            if (PlayerPrefs.HasKey(NumberOfRoundsString))
                NumberOfRounds.Set(PlayerPrefs.GetInt(NumberOfRoundsString), notify: false);
        }
    }
}