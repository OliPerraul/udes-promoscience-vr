using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



#if UNITY_EDITOR

using Unity.EditorCoroutines.Editor;

namespace UdeS.Promoscience.Labyrinths.Editor
{

    public class AlgorithmChecker : MonoBehaviour
    {
        [SerializeField]
        private Algorithms.Algorithm algorithm;

        [SerializeField]
        private LabyrinthEditor editor;

        [SerializeField]
        private UnityEngine.Tilemaps.Tilemap templateTilemap;

        [SerializeField]
        private UnityEngine.Tilemaps.Tilemap dirtyTilemap;

        [SerializeField]
        public bool running = false;

        [SerializeField]
        private Algorithms.AlgorithmProgressState state;

        private EditorCoroutine coroutine;

        [SerializeField]
        private float stepSeconds = 0.1f;

        public void Play()
        {
            if (dirtyTilemap == null)
            {
                state = new Algorithms.AlgorithmProgressState();
                algorithm.ResetProgressState(state, editor.resource);
                dirtyTilemap = Instantiate(templateTilemap, templateTilemap.transform.parent);
            }

            running = true;

            coroutine = EditorCoroutineUtility.StartCoroutine(DoCheckCoroutine(), this);
        }

        public void Pause()
        {
            if (coroutine != null)
            {
                EditorCoroutineUtility.StopCoroutine(coroutine);
                coroutine = null;
            }

            running = false;
        }


        public void Stop()
        {
            if (coroutine != null)
            {
                EditorCoroutineUtility.StopCoroutine(coroutine);
                coroutine = null;
            }

            if (dirtyTilemap != null)
            {

                DestroyImmediate(dirtyTilemap.gameObject);
                dirtyTilemap = null;
            }

            running = false;
        }


        IEnumerator DoCheckCoroutine()
        {
            while (running)
            {
                if (!algorithm.GetNextStep(state, editor.resource, out Promoscience.Tile tile))
                    break;

                if (tile.Color == TileColor.Red)
                {
                    dirtyTilemap.SetTile(new Vector3Int(tile.x, tile.y, 0), Resources.Instance.GetTile(TileType.DebugRed));

                    Debug.Log("red: " + tile.Position);
                }
                else if (tile.color == TileColor.Yellow)
                {
                    dirtyTilemap.SetTile(new Vector3Int(tile.x, tile.y, 0), Resources.Instance.GetTile(TileType.DebugYellow));

                    Debug.Log("yellow: " + tile.Position);
                }

                yield return new EditorWaitForSeconds(stepSeconds);

            }
        }

    }


    [CustomEditor(typeof(AlgorithmChecker))]
    public class SomeScriptEditor2 : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AlgorithmChecker myScript = (AlgorithmChecker)target;
            if (GUILayout.Button("PLAY"))
            {
                myScript.Play();
            }

            if (GUILayout.Button("PAUSE"))
            {
                myScript.Pause();
            }

            if (GUILayout.Button("STOP"))
            {
                myScript.Stop();
            }

        }
    }
}

#endif