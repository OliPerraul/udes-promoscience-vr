using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using UdeS.Promoscience.ScriptableObjects;


namespace UdeS.Promoscience.UI.Menu
{
    public class ReadyMenu : MonoBehaviour
    {
        [SerializeField]
        private Text infoText;        

        [SerializeField]
        private Image headsetImage;

        [SerializeField]
        private Image tabletImage;

        [SerializeField]
        private float disconnectedAlpha = 0.4f;

        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private Image buttonImage;

        private Color color;

        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
                backgroundImage.color = color;
                buttonImage.color = color;
            }
        }


        [SerializeField]
        private Sprite circleIconTemplate;

        [SerializeField]
        private List<ScriptableTeam> teams;

        [SerializeField]
        private Dropdown dropdown;

        private Button button;

        // TODO does not work
        private void AddDropdownOption(ScriptableTeam team)
        {
            var texture = new Texture2D(1, 1); // creating texture with 1 pixel
            texture.SetPixel(1, 1, color); // setting to this pixel some color
            texture.Apply(); //applying texture. necessarily                         //circleIconTemplate.
            var item = new Dropdown.OptionData(
                team.name,
                Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0, 0))); // creating dropdown item and converting texture to sprite

            dropdown.options.Add(item); // adding this item to dropdown options
        }

        public void OnValidate()
        {
            if (dropdown != null)
            {
                dropdown.ClearOptions();
                
                foreach (var tm in teams)
                {
                    AddDropdownOption(tm);
                }
            }
        }

        public void OnDropdownItemSelected(int idx)//.OptionData data)
        {
            Color = teams[idx].TeamColor;
        }

        public void Awake()
        {
            dropdown.onValueChanged.AddListener(OnDropdownItemSelected);

        }

    }
}
