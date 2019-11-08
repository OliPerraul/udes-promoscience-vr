
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
        [Cirrus.Editor.FindObjectOfType(typeof(Tilemap))]
        private Tilemap tilemap;


        [SerializeField]
        private string labyrinthPath;       

        public void Save()
        {
            TileBase[] tiles = tilemap.GetTilesBlock(tilemap.cellBounds);

            Resource resource = AssetDatabase.LoadAssetAtPath<Resource>(labyrinthPath);

            EditorUtility.SetDirty(resource);

            for (int y = 0; y < tilemap.size.y; y++)
            {
                for (int x = 0; x < tilemap.size.x; x++)
                {
                    int i = x + tilemap.size.x * y;

                    var tile = tilemap.GetTile<Tile>(new Vector3Int(x, y, 0));

                    resource.Tiles2[i] = tile == null ? TileType.Empty : tile.Type;
                }
            }

            resource.PopulateStartAndEndPositions();

            EditorApplication.ExecuteMenuItem("File/Save Project");
        }

        public void Load()
        {
            Debug.Log(Cirrus.AssetDatabase.CurrentFolder);

            labyrinthPath = EditorUtility.OpenFilePanel("Load labyrinth data", "Assets/Labyrinths", "asset");

            labyrinthPath = "Assets" + labyrinthPath.Substring(Application.dataPath.Length);

            Resource resource = AssetDatabase.LoadAssetAtPath<Resource>(labyrinthPath);

            tilemap.ClearAllTiles();

            for (int i = 0; i < resource.Tiles2.Length; i++)
            {
                if(resource.Tiles2[i] == TileType.Empty)
                    continue;

                tilemap.SetTile(
                    new Vector3Int(i.Mod(resource.SizeX), i / resource.SizeX, 0),
                    Resources.Instance.GetTile(resource.Tiles2[i])
                    );
            }
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