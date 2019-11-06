using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UdeS.Promoscience.Algorithms
{
    public class Resources : BaseResources<Resources>
    {
         
        [SerializeField]
        private Algorithm[] algorithms;

        public Algorithm[] Algorithms
        {
            get
            {
                if (algorithms == null ||
                    algorithms.Length == 0)
                {
                    algorithms = Cirrus.AssetDatabase.FindObjectsOfType<Algorithm>();
                }

                return algorithms;
            }
        }

        public Algorithm GetAlgorithm(Id id)
        {
            return Algorithms.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}