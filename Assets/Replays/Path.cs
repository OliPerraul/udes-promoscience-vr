//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;

//namespace UdeS.Promoscience.Playbacks
//{

//#if UNITY_EDITOR

//    using UnityEngine;
//    using System.Collections;
//    using UnityEditor;

//    [CustomEditor(typeof(Path))]
//    public class PathEditor : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            DrawDefaultInspector();

//            Path myScript = (Path)target;
//            if (GUILayout.Button("Draw"))
//            {
//                //myScript.Draw();
//            }
//        }
//    }

//#endif

//    public class Path : MonoBehaviour
//    {
//        [SerializeField]
//        private Color color;

//        [SerializeField]
//        private float drawTime = 0.6f;

//        [SerializeField]
//        private float normalWidth = 1.25f;

//        [SerializeField]
//        private float backtrackWidth = 2.5f;

//        private List<Segment> segments;

//        [SerializeField]
//        private Segment currentSegment;

//        [SerializeField]
//        private GameObject arrowHead;

//        [SerializeField]
//        private Segment segmentTemplate;

//        [SerializeField]
//        private Transform positionsParent;

//        [SerializeField]
//        private List<Transform> markers;

//        [SerializeField]
//        private Vector3[] positions;

//        private Dictionary<Vector2Int, Segment> memory;

//        public void OnValidate()
//        {
//            if (positionsParent != null)
//            {
//                if (positions.Length == 0)
//                {
//                    markers = positionsParent.GetComponentsInChildren<Transform>().ToList();
//                    markers.Remove(positionsParent.transform);
//                    positions = markers.Select(x => x.position).ToArray();
//                }
//            }
//        }

//        private int counter = 0;

//        public Path Create(Transform parent)
//        {
//            Path path = Instantiate(
//                gameObject,
//                parent.transform.position,
//                Quaternion.identity,
//                parent)
//                .GetComponent<Path>();

//            path.segments = new List<Segment>();
//            path.memory = new Dictionary<Vector2Int, Segment>();

//            return path;
//        }

//        [ExecuteInEditMode]
//        public void FixedUpdate()
//        {
//            if (currentSegment != null)
//            {
//                arrowHead.transform.rotation =
//                    Quaternion.LookRotation(
//                        currentSegment.Destination - currentSegment.Origin,
//                        Vector3.up);

//                arrowHead.transform.position = currentSegment.Current;
//            }
//        }

//        public void Draw(Vector3[] positions)
//        {
//            this.positions = positions;

//            for (int i = 1; i < positions.Length; i++)
//            {
//                currentSegment = segmentTemplate.Create(
//                    this, 
//                    positions[i - 1], 
//                    positions[i], 
//                    drawTime, 
//                    normalWidth);

//                currentSegment.Draw();
//            }            
//        }

//        // Reduces the path importance (On teleportation
//        public void Reduce()
//        {
//            foreach (Segment segm in segments)
//            {
//                if (segm == null)
//                    continue;

//                Destroy(segm.gameObject);

//                //segm.Reduce();
//            }
//        }
        
//        public IEnumerator DrawBetween(Vector3 origin, Vector3 dest, bool backtrack=false)
//        {
//            currentSegment = segmentTemplate.Create(
//                this, 
//                origin, 
//                dest, 
//                drawTime,
//                backtrack? backtrackWidth : normalWidth);

//            segments.Add(currentSegment);
//            yield return StartCoroutine(currentSegment.DrawCoroutine());
//        }

//        //[ExecuteInEditMode]
//        //public void Start()
//        //{
//        //    StartCoroutine(DrawCoroutine());
//        //}



//    }
//}
