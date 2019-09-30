using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;

using Cirrus.Extensions;

using System.Linq;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replay
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

        private int gameActionIndex = 0;

        private int moveIndex = 0;

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
            sequence.moveCount = sequence.data.Actions.Aggregate(0, (x, y) => IsMovement((GameAction)y) ? x + 1 : x);
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

        public void Move(int target)
        {
            float sign = Mathf.Sign(target - moveIndex);

            if (sign < 0)
            {
                for (; HasPrevious; Reverse()) ;
            }
            else
            {
                for (; HasNext; Perform()) ;
            }
        }

        public void Move(float progress)
        {
            int target = Mathf.RoundToInt(progress * moveCount);
            Move(target);
        }

        public bool HasPrevious
        {
            get
            {
                if (data.Actions.Length == 0)
                    return false;

                return moveIndex >= 0;
            }
        }


        public void Reverse()
        {
            DoReverse(
                (GameAction)data.Actions[gameActionIndex],
                data.ActionValues[gameActionIndex]);

            float progress = moveIndex < 0 ? 0 : ((float)moveIndex) / moveCount;
            OnProgressHandler.Invoke(progress);

            int index = gameActionIndex--;
            while (!IsMovement((GameAction)data.Actions[index]) && index >= 0)
            {
                index--;
            }

            if (index < 0)
                gameActionIndex = 0;
            else
                gameActionIndex = index;


            // Clamp to stay within bounds;
            // let index overflow: required by iteration
            moveIndex = Mathf.Clamp(moveIndex, 0, moveCount - 1);
            moveIndex--;
        }

        private void DoReverse(GameAction gameAction, string info)
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

        public bool HasNext
        {
            get
            {

                if (data.Actions.Length == 0)
                    return false;
                
                return moveIndex < moveCount;
            }
        }


        public void Perform()
        {
            DoPerform(
                (GameAction)data.Actions[gameActionIndex],
                data.ActionValues[gameActionIndex]);

         
            int index = gameActionIndex++;
            while (!IsMovement((GameAction)data.Actions[index]))
            {
                index = index + 1 >= data.Actions.Length ? index - 1 : index + 1;
            }

            if (index >= data.Actions.Length)
                gameActionIndex = index;
            else
                gameActionIndex = index;

            float progress = moveIndex >= moveCount ? 1 : ((float)moveIndex) / moveCount;
            OnProgressHandler.Invoke(progress);

            // Clamp to stay within bounds;
            // Index overflow required by iteration
            moveIndex = Mathf.Clamp(moveIndex, 0, moveCount - 1);
            moveIndex++;

        }

        private void DoPerform(GameAction gameAction, string info)
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


        public IEnumerator PerformCoroutine()
        {
            yield return StartCoroutine(DoPerformCoroutine(
                (GameAction)data.Actions[gameActionIndex],
                data.ActionValues[gameActionIndex]));


            int index = gameActionIndex++;
            while (!IsMovement((GameAction)data.Actions[index]))
            {
                index = index + 1 >= data.Actions.Length ? index - 1 : index + 1;
            }

            if (index >= data.Actions.Length)
                gameActionIndex = index;
            else
                gameActionIndex = index;

            float progress = moveIndex >= moveCount ? 1 : ((float)moveIndex) / moveCount;
            OnProgressHandler.Invoke(progress);

            // Clamp to stay within bounds;
            // Index overflow required by iteration
            moveIndex = Mathf.Clamp(moveIndex, 0, moveCount - 1);
            moveIndex++;
        }

        public IEnumerator DoPerformCoroutine(GameAction gameAction, string info)
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

                    Redraw(actionInfo.playerSteps);

                    break;
            }

            yield return null;

        }


        public void Resume()
        {
            StartCoroutine(ResumeCoroutine());
        }

        public void Pause()
        {
            StopAllCoroutines();
            Move(moveIndex - 1);
        }


        public void Stop()
        {
            StopAllCoroutines();
            Move(0);
        }

        public IEnumerator ResumeCoroutine()
        {
            while (HasNext)
            {
                yield return StartCoroutine(PerformCoroutine());
            }
        }

    }
}