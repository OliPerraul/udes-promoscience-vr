using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;

using Cirrus.Extensions;

using System.Linq;
using System.Collections.Generic;

namespace UdeS.Promoscience.Playbacks
{
    public class PlayerSequence : MonoBehaviour
    {
        public OnProgress OnProgressHandler;

        [SerializeField]
        private float speed = 0.6f;

        private Labyrinth labyrinth;

        private Vector2Int labyrinthPosition;

        private Vector2Int lastLabyrinthPosition;

        private Vector3 targetPosition;

        private CourseData data;

        private Vector3 lastPosition;

        private TileColor lastColor;
        private Vector3 lastOffset;
        [SerializeField]
        private float drawTime = 0.6f;

        [SerializeField]
        private float normalWidth = 1.25f;

        [SerializeField]
        private Segment currentSegment;

        [SerializeField]
        private GameObject arrowHead;

        [SerializeField]
        private Segment segmentTemplate;

        [SerializeField]
        private Transform positionsParent;

        [SerializeField]
        private List<Transform> markers;

        [SerializeField]
        private Vector3[] positions;

        [SerializeField]
        private Material templateSegmentMaterial;

        private Material segmentMaterial;

        private Dictionary<Vector2Int, Stack<Segment>> segments;

        private bool backtrack = false;

        private int stepIndex = 0;

        private int movementIndex = 0;

        private int moveCount = 0;

        public PlayerSequence Create(
            CourseData data,
            Labyrinth labyrinth,
            Vector2Int labpos,
            Vector3 worldPos)
        {
            PlayerSequence sequence = Instantiate(
                gameObject,
                worldPos, Quaternion.identity)
                .GetComponent<PlayerSequence>();

            sequence.labyrinth = labyrinth;
            sequence.labyrinthPosition = labpos;
            sequence.transform.position = worldPos;
            sequence.targetPosition = worldPos;
            sequence.data = data;
            sequence.moveCount = sequence.data.Steps.Aggregate(0, (x, y) => IsMovement((GameAction)y) ? x + 1 : x);
            sequence.segments = new Dictionary<Vector2Int, Stack<Segment>>();
            sequence.segmentMaterial = new Material(templateSegmentMaterial);
            sequence.segmentMaterial.color = data.Team.TeamColor;
            sequence.arrowHead.GetComponentInChildren<SpriteRenderer>().color = sequence.segmentMaterial.color;

            return sequence;
        }

        public void OnValidate()
        {
            if (positionsParent != null)
            {
                if (positions.Length == 0)
                {
                    markers = positionsParent.GetComponentsInChildren<Transform>().ToList();
                    markers.Remove(positionsParent.transform);
                    positions = markers.Select(x => x.position).ToArray();
                }
            }
        }

        public void FixedUpdate()
        {
            if (currentSegment != null)
            {
                arrowHead.transform.rotation =
                    Quaternion.LookRotation(
                        currentSegment.Destination - currentSegment.Origin,
                        Vector3.up);

                arrowHead.transform.position = currentSegment.Current;
            }
        }

        public bool IsMovement(GameAction action)
        {
            switch (action)
            {
                case GameAction.MoveUp:
                case GameAction.MoveDown:
                case GameAction.MoveLeft:
                case GameAction.MoveRight:
                    return true;
                default:
                    return false;
            }
        }        

        public void Previous()
        {
            if (data.Steps.Length == 0)
                return;

            if (stepIndex < 0)
                return;

            while (!IsMovement((GameAction)data.Steps[stepIndex]))
            {
                stepIndex = stepIndex - 1 <= 0 ? 0 : stepIndex - 1;
            }


            Reverse(
                (GameAction)data.Steps[stepIndex],
                data.StepValues[stepIndex]);

            movementIndex--;
            stepIndex--;
            float progress = movementIndex < 0 ? 0 : ((float)movementIndex) / moveCount;
            OnProgressHandler.Invoke(progress);
        }

        public void Next()
        {
            if (data.Steps.Length == 0)
                return;

            if (stepIndex >= data.Steps.Length)
                return;

            while (!IsMovement((GameAction)data.Steps[stepIndex]))
            {
                stepIndex = stepIndex + 1 >= data.Steps.Length ? stepIndex - 1 : stepIndex + 1;
            }

            Perform(
                (GameAction)data.Steps[stepIndex],
                data.StepValues[stepIndex]);

            movementIndex++;
            stepIndex++;

            float progress = movementIndex >= moveCount ? 1 : ((float)movementIndex) / moveCount;
            OnProgressHandler.Invoke(progress);
        }


        public IEnumerator NextCoroutine()
        {
            if (data.Steps.Length == 0)
                yield return null;

            if (stepIndex >= data.Steps.Length)
                yield return null;

            while (!IsMovement((GameAction)data.Steps[stepIndex]))
            {
                stepIndex = stepIndex + 1 >= data.Steps.Length ? stepIndex - 1 : stepIndex + 1;
            }

            yield return StartCoroutine(PerformCoroutine(
                (GameAction)data.Steps[stepIndex],
                data.StepValues[stepIndex]));

            movementIndex++;
            stepIndex++;

            float progress = movementIndex >= moveCount ? 1 : ((float)movementIndex) / moveCount;
            OnProgressHandler.Invoke(progress);
        }

        public void Move(int target)
        {
            float sign = Mathf.Sign(target - movementIndex);

            if (sign < 0)
            {
                for (int i = 0; i < moveCount; i++)
                {
                    if (movementIndex < target)
                        return;

                    Previous();
                }
            }
            else
            {
                for (int i = 0; i < moveCount; i++)
                {
                    if (movementIndex >= target)
                        return;

                    Next();
                }
            }
        }


        public void Move(float progress)
        {            
            int target = Mathf.RoundToInt(progress * moveCount);
            Move(target);
        }


        public void Redraw(Tile[] tiles)
        {
            positions = tiles.Select
                    (x => labyrinth.GetLabyrinthPositionInWorldPosition(x.Position)).ToArray();

            Segment segment;
            Stack<Segment> stack;
            for (int i = 1; i < tiles.Length; i++)
            {
                Vector3 offset = Vector3.zero;
                if (segments.TryGetValue(tiles[i].Position, out stack))
                {
                    stack.Peek().gameObject.SetActive(false);
                }
                else
                {
                    stack = new Stack<Segment>();
                    segments[tiles[i].Position] = stack;
                }

                Vector3 origin = labyrinth.GetLabyrinthPositionInWorldPosition(tiles[i - 1].Position);
                Vector3 dest = labyrinth.GetLabyrinthPositionInWorldPosition(tiles[i].Position);

                segment = segmentTemplate.Create(
                    transform,
                    positions[i - 1],
                    positions[i],
                    segmentMaterial,
                    drawTime,
                    normalWidth,
                    backtrack);

                segment.Draw();
                stack.Push(segment);
            }
        }

        public void Draw(Vector2Int o, Vector3 origin, Vector3 dest, bool backtrack = false)
        {
            Stack<Segment> stack;
            if (segments.TryGetValue(o, out stack))
            {
                if (stack.Count > 0)
                {
                    stack.Peek().gameObject.SetActive(false);
                }
            }
            else
            {
                stack = new Stack<Segment>();
                segments[o] = stack;
            }

            currentSegment = segmentTemplate.Create(
                transform,
                origin,
                dest,
                segmentMaterial,
                drawTime,
                normalWidth,
                backtrack);

            segments[o].Push(currentSegment);
            currentSegment.Draw();
        }

        public IEnumerator DrawCoroutine(Vector2Int o, Vector3 origin, Vector3 dest, bool backtrack = false)
        {
            Stack<Segment> stack;
            if (segments.TryGetValue(o, out stack))
            {
                if (stack.Count > 0)
                {
                    stack.Peek().gameObject.SetActive(false);
                }
            }
            else
            {
                stack = new Stack<Segment>();
                segments[o] = stack;
            }

            currentSegment = segmentTemplate.Create(
                transform,
                origin,
                dest,
                segmentMaterial,
                drawTime,
                normalWidth,
                backtrack);

            segments[o].Push(currentSegment);
            yield return StartCoroutine(currentSegment.DrawCoroutine());
        }

        public void Reverse(GameAction gameAction, string info)
        {
            Segment segment;
            switch (gameAction)
            {
                case GameAction.MoveUp:

                    labyrinthPosition.y += 1;
                    segment = segments[labyrinthPosition].Pop();
                    Destroy(segment.gameObject);
                    if (segments[labyrinthPosition].Count != 0)
                    {
                        segments[labyrinthPosition].Peek().gameObject.SetActive(true);
                    }

                    break;

                case GameAction.MoveDown:

                    labyrinthPosition.y -= 1;
                    segment = segments[labyrinthPosition].Pop();
                    Destroy(segment.gameObject);
                    if (segments[labyrinthPosition].Count != 0)
                    {
                        segments[labyrinthPosition].Peek().gameObject.SetActive(true);
                    }

                    break;

                case GameAction.MoveLeft:

                    labyrinthPosition.x += 1;
                    segment = segments[labyrinthPosition].Pop();
                    Destroy(segment.gameObject);
                    if (segments[labyrinthPosition].Count != 0)
                    {
                        segments[labyrinthPosition].Peek().gameObject.SetActive(true);
                    }

                    break;

                case GameAction.MoveRight:

                    labyrinthPosition.x -= 1;
                    segment = segments[labyrinthPosition].Pop();                   
                    Destroy(segment.gameObject);
                    if (segments[labyrinthPosition].Count != 0)
                    {
                        segments[labyrinthPosition].Peek().gameObject.SetActive(true);
                    }

                    break;
            }
        }

        public void Perform(GameAction gameAction, string info)
        {
            lastLabyrinthPosition = labyrinthPosition;
            lastColor = labyrinth.GetTileColor(lastLabyrinthPosition);

            Vector3 origin = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);
            Vector3 dest;

            switch (gameAction)
            {
                case GameAction.MoveUp:

                    labyrinthPosition.y -= 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    Draw(
                        lastLabyrinthPosition,
                        origin,
                        dest,
                        backtrack);

                    break;

                case GameAction.MoveRight:

                    labyrinthPosition.x += 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    Draw(
                        lastLabyrinthPosition,
                        origin,
                        dest,
                        backtrack);

                    break;

                case GameAction.MoveDown:

                    labyrinthPosition.y += 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    Draw(
                        lastLabyrinthPosition,
                        origin,
                        dest,
                        backtrack);

                    break;

                case GameAction.MoveLeft:

                    labyrinthPosition.x -= 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    Draw(
                        lastLabyrinthPosition,
                        origin,
                        dest,
                        backtrack);

                    break;

                case GameAction.TurnLeft:
                    break;

                case GameAction.TurnRight:
                    break;

                case GameAction.PaintFloorRed:
                    backtrack = true;

                    break;

                case GameAction.PaintFloorYellow:
                    backtrack = false;

                    break;

                case GameAction.ReturnToDivergencePoint:

                    ActionValue actionInfo = JsonUtility.FromJson<ActionValue>(info);
                    labyrinthPosition = actionInfo.position;
                    targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    transform.position = targetPosition;
                    transform.rotation = actionInfo.rotation;

                    Redraw(actionInfo.playerSteps);

                    break;
            }
        }

        public void Resume()
        {
            StartCoroutine(ResumeCoroutine());
        }

        public void Pause()
        {
            StopAllCoroutines();
            Move(movementIndex-1);
        }


        public void Stop()
        {           
            StopAllCoroutines();
            Move(0);
        }


        public IEnumerator ResumeCoroutine()
        {
            while (movementIndex < moveCount)
            {
                yield return StartCoroutine(NextCoroutine());
            }
        }


        public IEnumerator PerformCoroutine(GameAction gameAction, string info)
        {
            yield return new WaitForEndOfFrame();
            lastLabyrinthPosition = labyrinthPosition;
            lastColor = labyrinth.GetTileColor(lastLabyrinthPosition);

            Vector3 origin = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);
            Vector3 dest;

            switch (gameAction)
            {
                case GameAction.MoveUp:

                    labyrinthPosition.y -= 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    yield return StartCoroutine(DrawCoroutine(
                        lastLabyrinthPosition,
                        origin,
                        dest,
                        backtrack));

                    break;

                case GameAction.MoveRight:

                    labyrinthPosition.x += 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    yield return StartCoroutine(DrawCoroutine(
                        lastLabyrinthPosition,
                        origin,
                        dest,
                        backtrack));

                    break;

                case GameAction.MoveDown:

                    labyrinthPosition.y += 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    yield return StartCoroutine(DrawCoroutine(
                        lastLabyrinthPosition,
                        origin,
                        dest,
                        backtrack));

                    break;

                case GameAction.MoveLeft:

                    labyrinthPosition.x -= 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    yield return StartCoroutine(DrawCoroutine(
                        lastLabyrinthPosition,
                        origin,
                        dest,
                        backtrack));

                    break;

                case GameAction.TurnLeft:
                    break;

                case GameAction.TurnRight:
                    break;

                case GameAction.PaintFloorRed:
                    backtrack = true;

                    break;

                case GameAction.PaintFloorYellow:
                    backtrack = false;

                    break;

                case GameAction.ReturnToDivergencePoint:

                    ActionValue actionInfo = JsonUtility.FromJson<ActionValue>(info);
                    labyrinthPosition = actionInfo.position;
                    targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    transform.position = targetPosition;
                    transform.rotation = actionInfo.rotation;

                    //foreach (var pair in segments)
                    //{
                    //    if (pair.Value == null)
                    //        continue;

                    //    pair.Value.Enable(false);
                    //}

                    Redraw(actionInfo.playerSteps);

                    break;
            }

            yield return null;

        }


    }
}