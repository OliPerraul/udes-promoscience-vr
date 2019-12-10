using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UdeS.Promoscience.Labyrinths.Editor.UI
{

    public class ThemeSelectionInterface : Cirrus.BaseSingleton<ThemeSelectionInterface>
    {
        [SerializeField]
        private UnityEngine.UI.Dropdown dropdown;

        public Cirrus.ObservableValue<SkinResource> SelectedTheme = new Cirrus.ObservableValue<SkinResource>(null);

        private List<SkinResource> skins = new List<SkinResource>();


        public void Awake()
        {
            dropdown.onValueChanged.AddListener(OnSelectedThemeChanged);
        }


        public void Start()
        {
            dropdown.ClearOptions();

            foreach (var skin in Labyrinths.Resources.Instance.Skins)
            {
                if (skin == null)
                    continue;

                dropdown.options.Add(new UnityEngine.UI.Dropdown.OptionData(skin.name.Replace("Skin", "").Replace(".", " ")));
                skins.Add(skin);
            }

            dropdown.value = 0;
        }

        //public bool IsValidTheme(SkinResource skin)
        //{
        //    foreach (var piece in skin.Pieces)
        //    {

        //    }

        //    return true;
        //}


        public void OnSelectedThemeChanged(int themeIdx)
        {
            SelectedTheme.Value = skins[themeIdx];
        }
    }
}