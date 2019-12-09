using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UdeS.Promoscience.Utils;
using UnityEngine;

namespace UdeS.Promoscience.Labyrinths
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/Ressources", order = 1)]
    public class Resources : BaseResources<Resources>
    {
        //[SerializeField]
        //public RenderTexture RenderTexture;

        [SerializeField]
        public LabyrinthObject LabyrinthSmall;

        [SerializeField]
        public LabyrinthObject LabyrinthMedium;

        [SerializeField]
        public LabyrinthObject LabyrinthLarge;

        public void OnValidate()
        {
            if (Skins == null || Skins.Count == 0)
            {
                var skins = Cirrus.AssetDatabase.FindObjectsOfType<SkinResource>();
                Skins = skins == null ? null : skins.ToList();
            }

            if (Labyrinths == null || Labyrinths.Count == 0)
            {
                var labs = Cirrus.AssetDatabase.FindObjectsOfType<Resource>();
                Labyrinths = labs == null ? null : labs.ToList();
            }
        }

        public LabyrinthObject GetLabyrinthObject(ILabyrinth data)
        {
            switch (Utils.GetType(data))
            {
                case Type.Small:
                    return LabyrinthSmall;

                case Type.Medium:
                    return LabyrinthMedium;

                case Type.Large:
                    return LabyrinthLarge;

                default:// Type.Unknown:
                    return LabyrinthMedium;
            }
        }

        public static int NumLabyrinths => Instance.Labyrinths.Count;
        
        [UnityEngine.Serialization.FormerlySerializedAs("LabyrinthData")]
        [SerializeField]
        public List<Resource> Labyrinths;

        //[SerializeField]
        //public Resource TutorialLabyrinthData;

        // TODO: remove offset with Id and index
        // right now needed cuz round determine labyrinth
        public ILabyrinth GetLabyrinth(int id)
        {
            return Labyrinths[id];
        }

        [SerializeField]
        public List<SkinResource> Skins;

        [SerializeField]
        public SkinResource GreyboxSkin;
    }
}

