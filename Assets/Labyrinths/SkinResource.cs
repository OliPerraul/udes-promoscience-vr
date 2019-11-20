using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UdeS.Promoscience.Labyrinths
{

    //public interface 



    [CreateAssetMenu(fileName = "Labyrinth", menuName = "Algorinthe/Labyrinths/Skin", order = 1)]
    public class SkinResource : ScriptableObject, ISkin
    {
        public int Id
        {
            get
            {
                return Resources.Instance.Skins.IndexOf(this);
            }
        }

        [SerializeField]
        public Material GreyFloorMaterial;

        [SerializeField]
        public Material YellowFloorMaterial;

        [SerializeField]
        public Material RedFloorMaterial;

        [SerializeField]
        public GameObject FloorStart;

        [SerializeField]
        public GameObject FloorEnd;

        [SerializeField]
        public GameObject FloorTile1;

        [SerializeField]
        public GameObject FloorTile2;

        [SerializeField]
        public GameObject FloorTile3;

        [SerializeField]
        public GameObject FloorTile4;

        [SerializeField]
        public GameObject FloorTile5;

        [SerializeField]
        public GameObject FloorTile6;

        [SerializeField]
        public GameObject FloorTile7;

        [SerializeField]
        public GameObject FloorTile8;

        [SerializeField]
        public GameObject FloorTile9;

        [SerializeField]
        public GameObject FloorTile10;

        [SerializeField]
        public GameObject FloorTile11;

        [SerializeField]
        public GameObject Corner1;

        [SerializeField]
        public GameObject Corner2;

        [SerializeField]
        public GameObject Corner3;
        
        [SerializeField]
        public GameObject Horizontal1;

        [SerializeField]
        public GameObject Horizontal2;

        [SerializeField]
        public GameObject Horizontal3;

        [SerializeField]
        public GameObject Horizontal4;

        [SerializeField]
        public GameObject Vertical1;

        [SerializeField]
        public GameObject Vertical2;

        [SerializeField]
        public GameObject Vertical3;

        [SerializeField]
        public GameObject Vertical4;

        [SerializeField]
        private Piece[] pieces;

        public IEnumerable<Piece> Pieces => pieces;


    }
}