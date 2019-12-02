using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Algorithms.UI
{
    public class LevelAlgorithmSelectionInteface : MonoBehaviour
    {
        public LocalizeInlineString randomString = new LocalizeInlineString("Randomized");

        public LocalizeInlineString gameRoundString = new LocalizeInlineString("Game Round");

        public UnityEngine.UI.Dropdown dropDown;

        private Algorithm[] algorithms;

        [SerializeField]
        private Id algorithmId = Id.LongestStraight;

        public Id AlgorithmId => algorithmId;

        public void Awake()
        {
            dropDown.ClearOptions();

            AddOption(randomString.Value);

            int i = 0;

            foreach (var algorithm in Resources.Instance.Algorithms)
            {
                AddOption(algorithm);
                i++;
            }

            dropDown.onValueChanged.AddListener(OnDropDownSelected);

            OnDropDownSelected(0);


            algorithmId = Utils.Random;
        }

        public void AddOption(string option)
        {
            var data = new UnityEngine.UI.Dropdown.OptionData(option);            
            dropDown.options.Add(data);
        }

        public void AddOption(Algorithm algorithm)
        {
            var data = new UnityEngine.UI.Dropdown.OptionData(algorithm.Name);
            dropDown.options.Add(data);
        }

        public void OnDropDownSelected(int i)
        {
            if (i != 0)
            {
                Debug.Log(algorithmId = Resources.Instance.Algorithms[i - 1].Id);
            }

            switch (i)
            {
                case 0:
                    algorithmId = Utils.Random;
                    break;

                default:
                    algorithmId = Resources.Instance.Algorithms[i - 1].Id;
                    break;
            }
        }
    }
}