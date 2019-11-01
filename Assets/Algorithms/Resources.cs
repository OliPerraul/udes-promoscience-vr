using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UdeS.Promoscience.Algorithms
{
    public class Resources : BaseResources<Resources>
    {

        [SerializeField]
        private List<Resource> algorithms;

        private Dictionary<Id, Algorithm> idAlgorithmPairs;// = new Dictionary<Id, Algorithm>();

        public Dictionary<Id, Algorithm> IdAlgorithmPairs
        {
            get
            {
                if (idAlgorithmPairs == null)
                {
                    idAlgorithmPairs = new Dictionary<Id, Algorithm>();
                }

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
                
                return idAlgorithmPairs;
            }
        }

        public ICollection<Algorithm> Algorithms
        {
            get
            {
                return IdAlgorithmPairs.Values;
            }
        }

        public Algorithm GetAlgorithm(Id id)
        {
            Algorithm algorithm;

            if (IdAlgorithmPairs.TryGetValue(id, out algorithm))
            {
                return algorithm;
            }

            return null;
        }


    }
}