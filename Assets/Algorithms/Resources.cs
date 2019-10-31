using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UdeS.Promoscience.Algorithms
{
    public class Resources : ScriptableObject
    {
        public static Resources Instance;

        public void Awake()
        {
            Instance = this;
        }

        public void OnEnable()
        {
            Instance = this;            
        }

        [SerializeField]
        private List<Resource> algorithms;

        private Dictionary<Id, Algorithm> idAlgorithmPairs = new Dictionary<Id, Algorithm>();


        public ICollection<Algorithm> Algorithms {

            get
            {
                if (idAlgorithmPairs.Count == 0)
                {
                    foreach (Resource res in algorithms)
                    {
                        if (!idAlgorithmPairs.ContainsKey(res.Id))
                        {
                            idAlgorithmPairs.Add(res.Id, res.Create());
                        }
                    }
                }

                return idAlgorithmPairs.Values;
            }
        }

        public Algorithm GetAlgorithm(Id id)
        {
            Algorithm algorithm;

            if (idAlgorithmPairs.Count == 0)
            {
                foreach (Resource res in algorithms)
                {
                    if (!idAlgorithmPairs.ContainsKey(res.Id))
                    {
                        idAlgorithmPairs.Add(res.Id, res.Create());
                    }
                }
            }

            if (idAlgorithmPairs.TryGetValue(id, out algorithm))
            {
                return algorithm;
            }

            return null;
        }


    }
}