using UnityEngine;
using System.Collections;

namespace UdeS.Promoscience.Algorithms.UI
{
    public class AlgorithmSelect : MonoBehaviour
    {
        public LocalizeInlineString randomString = new LocalizeInlineString("Randomized");

        public LocalizeInlineString gameRoundString = new LocalizeInlineString("Game Round");

        public UnityEngine.UI.Dropdown dropDown;

        private Algorithm[] algorithms;


        private Algorithms.Id algorithmId = 0;

        public Algorithms.Id AlgorithmId => algorithmId;


        public void Awake()
        {
            dropDown.ClearOptions();

            AddOption("(*) " + randomString.Value);

            AddOption("(*) " + gameRoundString.Value);

            algorithms = new Algorithm[Resources.Instance.Algorithms.Length];

            int i = 0;

            foreach (var algorithm in Resources.Instance.Algorithms)
            {
                AddOption(algorithm);
                algorithms[i] = algorithm;
                i++;
            }

            dropDown.onValueChanged.AddListener(OnDropDownSelected);

            OnDropDownSelected(0);
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
            switch (i)
            {
                case 0:
                    algorithmId = Utils.Random;
                    break;

                case 1:
                    algorithmId = Id.GameRound;
                    break;

                default:
                    algorithmId = algorithms[i - 2].Id;
                    break;
            }
        }
    }
}