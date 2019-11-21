﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UdeS.Promoscience.Labyrinths
{

    //public interface 



    [CreateAssetMenu(fileName = "Labyrinth", menuName = "Algorinthe/Labyrinths/Skin", order = 1)]
    public class SkinResource : ScriptableObject, ISkin
    {
        public string Name => name;

        public int Id
        {
            get
            {
                return Resources.Instance.Skins.IndexOf(this);
            }
        }

        [SerializeField]
        private Piece[] pieces;

        public IEnumerable<Piece> Pieces => pieces;


    }
}