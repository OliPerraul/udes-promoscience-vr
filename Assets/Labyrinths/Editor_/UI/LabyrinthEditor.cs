﻿
#if UNITY_EDITOR

using Cirrus.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityEngine.Tilemaps;

namespace UdeS.Promoscience.Labyrinths.Editor
{
    public class LabyrinthEditor : MonoBehaviour
    {

        [SerializeField]
        public Tilemap tilemap;

        [SerializeField]
        private string labyrinthPath;

        [SerializeField]
        private ControllerAsset controller;

        [SerializeField]
        private UnityEngine.UI.Button exitButton;



        [SerializeField]
        public Resource resource;

        public virtual void Awake()
        {
            controller.State.OnValueChangedHandler += OnStateChanged;
            exitButton.onClick.AddListener(OnExitClicked);
        }

        public virtual void OnDestroy()
        {
            controller.State.OnValueChangedHandler -= OnStateChanged;
        }


        public virtual void OnStateChanged(State state)
        {
            switch (state)
            {
                case State.Select:

                    gameObject.SetActive(false);

                    break;

                case State.Editor:

                    gameObject.SetActive(true);
                    break;

                default:
                    gameObject.SetActive(false);
                    break;
            }
        }


        public void OnExitClicked()
        {
            controller.State.Set(State.Select);
        }

        // TODO
        public void Save()
        {
            //TileBase[] tiles = tilemap.GetTilesBlock(tilemap.cellBounds);

            //Resource resource = AssetDatabase.LoadAssetAtPath<Resource>(labyrinthPath);

            //EditorUtility.SetDirty(resource);

            //for (int y = 0; y < tilemap.size.y; y++)
            //{
            //    for (int x = 0; x < tilemap.size.x; x++)
            //    {
            //        var tile = tilemap.GetTile<Tile>(new Vector3Int(x, y, 0));

                       // BROKEN HERE
                       // WE NEED TO DO resource.SetTile(x, y)...
            //        int i = x + tilemap.size.x * y;

            //        resource.Tiles2[i] = tile == null ? TileType.Empty : tile.Type;
            //    }
            //}

            //resource.PopulateStartAndEndPositions();

            //EditorApplication.ExecuteMenuItem("File/Save Project");
        }

        public void Load()
        {
            Debug.Log(Cirrus.AssetDatabase.CurrentFolder);

            labyrinthPath = EditorUtility.OpenFilePanel("Load labyrinth data", "Assets/Labyrinths", "asset");

            labyrinthPath = "Assets" + labyrinthPath.Substring(Application.dataPath.Length);

            resource = AssetDatabase.LoadAssetAtPath<Resource>(labyrinthPath);

            tilemap.ClearAllTiles();
            
            for (int x = 0; x < resource.GetLabyrithXLenght(); x++)
            {
                for (int y = 0; y < resource.GetLabyrithYLenght(); y++)
                {
                    if (resource.GetLabyrithValueAt(x, y) == (int)TileType.Empty)
                        continue;

                    tilemap.SetTile(
                        new Vector3Int(x, y, 0),
                        Resources.Instance.GetTile((TileType)resource.GetLabyrithValueAt(x, y)));
                }
            }
        }


        public void SetTile(int x, int y, TileType tile)
        {
            tilemap.SetTile(
             new Vector3Int(x, y, 0),
             Resources.Instance.GetTile(tile)
             );
        }

    }

    [CustomEditor(typeof(LabyrinthEditor))]
    public class SomeScriptEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            LabyrinthEditor myScript = (LabyrinthEditor)target;
            if (GUILayout.Button("Save Labyrinth"))
            {
                myScript.Save();
            }

            if (GUILayout.Button("Load Labyrinth"))
            {
                myScript.Load();
            }
        }
    }

}

#endif