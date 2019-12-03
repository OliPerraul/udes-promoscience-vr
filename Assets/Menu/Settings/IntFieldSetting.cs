using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Menu
{

    public class IntFieldSetting : MonoBehaviour
    {
        [SerializeField]
        public Cirrus.ObservableInt Value = new Cirrus.ObservableInt(3);

        [SerializeField]
        private UnityEngine.UI.InputField inputField;


        [SerializeField]
        private UnityEngine.UI.Slider slider;

        // Start is called before the first frame update
        public void Awake()
        {
            Value.OnValueChangedHandler += (x) =>
            {
                Debug.Log("field " + x);
                slider.SetValueWithoutNotify(x);
                inputField.SetTextWithoutNotify(x.ToString());
            };

            inputField.onValueChanged.AddListener(
                (x) => Value.Value = int.TryParse(inputField.text, out int integer) ? integer : Value.Value);

            slider.onValueChanged.AddListener(
                (x) =>
                {
                    Value.Value = (int)slider.value;
                }
                );

        }

    }
}