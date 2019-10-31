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

        private bool init = false;

        public void OnEnable()
        {
            if (init) return;

            init = true;

            dropDown.ClearOptions();

            AddOption("(*) " + randomString.Value);

            AddOption("(*) " + gameRoundString.Value);

            algorithms = new Algorithm[Resources.Instance.Algorithms.Count];

            int i = 0;

            foreach (var algorithm in Resources.Instance.Algorithms)
            {
                AddOption(algorithm);
                algorithms[i] = algorithm;
                i++;
            }

            dropDown.onValueChanged.AddListener(OnDropDownSelected);
            
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
                    Server.Instance.SetAlgorithm(Id.Randomized);
                    break;

                case 1:
                    Server.Instance.SetAlgorithm(Id.GameRound);
                    break;

                default:
                    Server.Instance.SetAlgorithm(algorithms[i - 2].Id);
                    break;
            }
        }
    }
}