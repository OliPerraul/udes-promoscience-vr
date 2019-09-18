using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PathDrawer))]
public class PathDrawerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathDrawer myScript = (PathDrawer)target;
        if (GUILayout.Button("Reset Width Curve"))
        {
            myScript.ResetWidthCurve();
        }
    }
}

#endif


public class PathDrawer : MonoBehaviour
{
    [SerializeField]
    private List<Transform> markers;

    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private Vector3[] points;

    [SerializeField]
    private float backtrackOffset;


    public void OnValidate()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        if (markers.Count == 0)
        {
            markers = transform.GetComponentsInChildren<Transform>().ToList();
            markers.Remove(transform);

            points = markers.Select(x => x.position).ToArray();
        }
    }

    public enum Direction
    {
        RightDown,
        DownRight,
        RightUp,
        UpRight,
        RightLeft,
        LeftRight,
        LeftDown,
        DownLeft,
        LeftUp,
        UpLeft,
        UpDown,
        DownUp,
        Left,
        Right,
        Down,
        Up,
        Unknown
    }

    public Direction GetTurn(Vector3 prev, Vector3 curr, Vector3 next)
    {
        // right -> down
        // down -> right
        if (
        prev.x < next.x &&
        prev.z < next.z)
        {
            return prev.x == curr.x ? Direction.DownRight : Direction.RightDown;
        }
        // right -> up
        // up -> right
        else if (
            prev.x < next.x &&
            prev.z > next.z)
        {
            return prev.x == curr.x ? Direction.UpRight : Direction.RightUp;
        }
        // right -> left (backtrack)
        else if (
            prev.x < curr.x &&
            curr.x > prev.x)
        {
            return Direction.RightLeft;
        }
        // left -> down
        // down -> left
        else if (
            prev.x > next.x &&
            prev.z < next.z)
        {
            return prev.x == curr.x ? Direction.DownLeft : Direction.LeftDown;
        }
        // left -> up
        // up -> left
        else if (
            prev.x > next.x &&
            prev.z > next.z)
        {
            return prev.x == curr.x ? Direction.LeftUp : Direction.UpLeft;
        }
        // left -> right (backtrack)
        else if (
            prev.x < curr.x &&
            curr.x > prev.x)
        {
            return Direction.LeftRight;
        }
        // TODO UpDown, DownUp
        else
        {
            return Direction.Unknown;
        }
    }

    public Direction NextTurn(int i)
    {
        for (int j = i+1; j < points.Length - 1; j++)
        {
            Vector3 prev = points[i - 1];
            Vector3 curr = points[i];
            Vector3 next = points[i + 1];            

            Direction dir = GetTurn(prev, curr, next);
            return dir;
        }

        return Direction.Unknown;
    }

    public void ResetWidthCurve()
    {
        //lineRenderer.l
        
        lineRenderer.widthCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0, 1));
    }
    //{
    //    List<Vector3> positions = new List<Vector3>();

    //    positions.Add(points[0]);

    //    for (int i = 1; i < points.Length-1; i++)
    //    {
    //        Vector3 prev = points[i-1];
    //        Vector3 curr = points[i];
    //        Vector3 next = points[i + 1];

    //        Direction dir = GetTurn(prev, curr, next);
    //        switch (dir)
    //        {
    //            default:
    //                positions.Add(curr);
    //                break;                
    //        }

    //        //Instantiate(obj, spawnPoint, Quaternion.identity);
    //    }

    //    positions.Add(points[points.Length-1]);

    //    lineRenderer.positionCount = positions.Count;
    //    lineRenderer.SetPositions(positions.ToArray());
    //}
}

