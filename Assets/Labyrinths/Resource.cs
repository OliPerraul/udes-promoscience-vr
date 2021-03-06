﻿using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;
using UnityEditor;

using System.Linq;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Labyrinths
{
    /// <summary>
    /// TODO remove
    /// Asset representation of a labyrinth
    /// </summary>
    [CreateAssetMenu(fileName = "Labyrinth", menuName = "Algorinthe/Labyrinths/Labyrinth", order = 1)]
    public class Resource : ScriptableObject, ILabyrinth
    {
        [SerializeField]
        private SkinResource skin;

        public SkinResource Skin
        {
            get
            {
                return skin;
            }

            set
            {
                skin = value;
            }
        }

        [SerializeField]
        private Labyrinth data;

        public void OnValidate()
        {
            if (skin != null)
            {
                data.SkinId = skin.Id;
            }

            if (data.Id < 0)
            {
                data.Id = Resources.Instance.LabyrinthsResources.IndexOf(this);
            }

            if (data.Name.IsNullOrEmpty())
            {
                data.Name = name;
            }
        }


        public ILabyrinth Data
        {
            get
            {
                if (data == null)
                    data = new Labyrinth();
                return data;
            }
        }

        public int Id
        {
            get
            {
                return Data.Id;
            }

            set
            {
                Data.Id = value;
            }
        }

        public int SizeX { get { return Data.SizeX; } set { Data.SizeX = value; } }

        public int SizeY { get { return Data.SizeY; } set { Data.SizeY = value; } }

        public Vector2Int StartPos { get { return Data.StartPos; }  set { Data.StartPos = value; } }

        public Vector2Int EndPos { get { return Data.EndPos; } set { Data.EndPos = value; } }

        public int StartDirection { get { return Data.StartDirection; } }

        public TileType[] Tiles2 { get { return Data.Tiles2; } set { Data.Tiles2 = value; } }

        public string Json => Data.Json;

        public string Name { get => Data.Name; set => Data.Name = value; }

        public bool GetIsTileWalkable(int x, int y)
        {
            return Data.GetIsTileWalkable(x, y);
        }

        public bool GetIsTileWalkable(Vector2Int tile)
        {
            return Data.GetIsTileWalkable(tile);
        }

        public int GetLabyrithValueAt(int x, int y)
        {
            return Data.GetLabyrithValueAt(x, y);
        }

        public int GetLabyrithXLenght()
        {
            return Data.GetLabyrithXLenght();
        }

        public int GetLabyrithYLenght()
        {
            return Data.GetLabyrithYLenght();
        }

        public void PopulateStartAndEndPositions()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    if (
                        GetLabyrithValueAt(x, y) >= Utils.TILE_START_START_ID &&
                        GetLabyrithValueAt(x, y) <= Utils.TILE_START_END_ID)
                    {
                        StartPos = new Vector2Int(x, y);
                    }
                    else if (
                        GetLabyrithValueAt(x, y) >= Utils.TILE_END_START_ID &&
                        GetLabyrithValueAt(x, y) <= Utils.TILE_END_END_ID)
                    {
                        EndPos = new Vector2Int(x, y);
                    }
                }
            }
        }

        public void SetLabyrithValueAt(int x, int y, TileType tile)
        {
            Data.SetLabyrithValueAt(x, y, tile);
        }

        public void SetLabyrithValueAt(Vector2Int pos, TileType tile)
        {
            Data.SetLabyrithValueAt(pos, tile);
        }
    }


#if UNITY_EDITOR

    [CustomEditor(typeof(Resource))]
    public class SomeScriptEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Resource myScript = (Resource)target;
            if (GUILayout.Button("Populate Start and End Positions"))
            {
                myScript.PopulateStartAndEndPositions();
            }
        }
    }

#endif

}

