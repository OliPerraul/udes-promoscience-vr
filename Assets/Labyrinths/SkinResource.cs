using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UdeS.Promoscience.Labyrinths
{    
    /// <summary>
     /// Represents a skin or theme used by a labyrinth (the visual aspect independent of the layout)
     /// </summary>
    [CreateAssetMenu(fileName = "Labyrinth", menuName = "Algorinthe/Labyrinths/Skin", order = 1)]
    public class SkinResource : ScriptableObject, ISkin
    {
        public string Name => name;

        public int Id => Resources.Instance.Skins.IndexOf(this);

        [SerializeField]
        private Piece[] pieces;

        public IEnumerable<Piece> Pieces => pieces;
    }
}