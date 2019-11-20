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
        [SerializeField]
        public RenderTexture RenderTexture;

        [SerializeField]
        public Labyrinth LabyrinthSmall;

        [SerializeField]
        public Labyrinth LabyrinthMedium;

        [SerializeField]
        public Labyrinth LabyrinthLarge;

        public void OnValidate()
        {
            if (Skins == null || Skins.Count == 0)
            {
                var skins = Cirrus.AssetDatabase.FindObjectsOfType<SkinResource>();
                Skins = skins == null ? null : skins.ToList();
            }

            if (LabyrinthData == null || LabyrinthData.Count == 0)
            {
                var labs = Cirrus.AssetDatabase.FindObjectsOfType<Resource>();
                LabyrinthData = labs == null ? null : labs.ToList();
            }
        }

        public Labyrinth GetLabyrinthTemplate(IData data)
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

        public static int NumLabyrinths => Instance.LabyrinthData.Count;

        [SerializeField]
        public List<Resource> LabyrinthData;

        //[SerializeField]
        //public Resource TutorialLabyrinthData;

        // TODO: remove offset with Id and index
        // right now needed cuz round determine labyrinth
        public IData GetLabyrinthData(int id)
        {
            return LabyrinthData[id-1];
        }

        [SerializeField]
        public List<SkinResource> Skins;

        [SerializeField]
        public SkinResource GreyboxSkin;
    }
}

