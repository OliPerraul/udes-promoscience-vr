using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UdeS.Promoscience.Utils;
using UnityEngine;

namespace UdeS.Promoscience.Labyrinths
{
    /// <summary>
    /// Contains all the resouses related to labyrinths
    /// </summary>
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

        [SerializeField]
        public List<SkinResource> Skins;

        [SerializeField]
        public SkinResource GreyboxSkin;

        public void OnValidate()
        {
            if (Skins == null || Skins.Count == 0)
            {
                var skins = Cirrus.AssetDatabase.FindObjectsOfType<SkinResource>();
                Skins = skins == null ? null : skins.ToList();
            }

            // Remove null skins
            for (int i = 0; i < Skins.Count; i++) // var skin in Skins)
            {
                if (Skins[i] == null)
                {
                    Skins.RemoveAt(i);
                }
            }

            if (LabyrinthsResources == null || LabyrinthsResources.Count == 0)
            {
                var labs = Cirrus.AssetDatabase.FindObjectsOfType<Resource>();
                LabyrinthsResources = labs == null ? null : labs.ToList();
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

        public static int NumLabyrinths => LabyrinthManager.Instance.Labyrinths.Count;
        
        // TODO deprecate
        [UnityEngine.Serialization.FormerlySerializedAs("LabyrinthData")]
        [SerializeField]
        public List<Resource> LabyrinthsResources;


        //// TODO this should not be inside a Sciptableobject
        //private List<ILabyrinth> labyrinths;

        //public List<ILabyrinth> Labyrinths
        //{
        //    get
        //    {
        //        if (labyrinths == null || labyrinths.Count == 0)
        //        {
        //            labyrinths = SQLiteUtilities.LoadAllLabyrinths2().ToList();
        //        }

        //        return labyrinths;
        //    }
        //}

        //[SerializeField]
        //public Resource TutorialLabyrinthData;

        // TODO: remove offset with Id and index
        // right now needed cuz round determine labyrinth
        // TODO remove 
        //public ILabyrinth GetLabyrinth(int id)
        //{
        //    return Server.Instance.Labyrinths.Where((x) => x.Id == id).FirstOrDefault();
        //}

    }
}

