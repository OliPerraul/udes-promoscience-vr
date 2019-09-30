using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;

using Cirrus.Extensions;

using System.Linq;
using System.Collections.Generic;

using System.Threading;

namespace UdeS.Promoscience.Replay
{
    public delegate void OnSequenceEvent(PlayerSequence sequence);

    public class PlayerSequence : MonoBehaviour
    {
        public OnEvent OnSequenceFinishedHandler;

        public OnIntEvent OnMoveCountSequenceDetermined;

        public OnIntEvent OnProgressHandler;

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
        private Material templateMaterial;

        [SerializeField]
        private Material templateBacktrackMaterial;

        private Material material;

        private Material backtrackMaterial;

        private Dictionary<Vector2Int, Stack<Segment>> segments;

        private bool backtrack = false;

        private int actionIndex = 0;

        private int moveIndex = 0;

        public int MoveCount = 0;

        private bool isPlaying = false;

        private Mutex mutex;

        private bool isReverse = true;

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
            sequence.MoveCount = sequence.data.Actions.Aggregate(0, (x, y) => IsMovement((GameAction)y) ? x + 1 : x);
            sequence.segments = new Dictionary<Vector2Int, Stack<Segment>>();

            sequence.material = new Material(templateMaterial);
            sequence.material.color = data.Team.TeamColor;

            sequence.backtrackMaterial = new Material(templateMaterial);
            sequence.backtrackMaterial.color = data.Team.TeamColor;

            sequence.arrowHead.GetComponentInChildren<SpriteRenderer>().color = sequence.material.color;

            return sequence;
        }

        public void Awake()
        {
            mutex = new Mutex();
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

        public void HandleAction(ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case ReplayAction.Previous:

                    if (isPlaying)
                    {
                        Pause();
                    }                    

                    if (HasPrevious)
                    {
                        Reverse();
                    }

                    break;

                case ReplayAction.Next:


                    if (isPlaying)
                    {
                        Pause();
                    }

                    if (HasNext)
                    {
                        Perform();
                    }

                    break;

                case ReplayAction.Play:
                    Move(0);
                    Resume();
                    break;

                case ReplayAction.Resume:
                    Resume();
                    break;

                case ReplayAction.Pause:
                    Pause();
                    break;

                case ReplayAction.Stop:
                    Stop();
                    break;

                case ReplayAction.Slide:

                    int current = (int)args[0];
                    Move(current);

                    break;
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
                    backtrack ? backtrackMaterial : material,
                    drawTime,
                    normalWidth);

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
                backtrack? backtrackMaterial : material,
                drawTime,
                normalWidth);

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
                backtrack ? backtrackMaterial : material,
                drawTime,
                normalWidth);

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
            if (target == moveIndex)
                return;

            if (Mathf.Sign(target - moveIndex) < 0)
            {
                while (HasPrevious)
                {
                    if (moveIndex < target)
                    {
                        return;
                    }

                    Reverse();
                }
            }
            else
            {
                while(HasNext)
                {
                    if (moveIndex >= target)
                    {                        
                        return;
                    }

                    Perform();
                }
            }

        }

        public bool HasPrevious
        {
            get
            {
                if (data.Actions.Length == 0)
                    return false;

                return moveIndex > 0;
            }
        }

        public int GetPreviousMovementAction()
        {
            int index = actionIndex - 1;
            while (index >= 0 && !IsMovement((GameAction)data.Actions[index]))
            {
                index--;
            }

            return index < 0 ? 0 : index;
        }

        public void Reverse()
        {
            mutex.WaitOne();

            actionIndex = GetPreviousMovementAction();

            isReverse = true;

            DoReverse(
                (GameAction)data.Actions[actionIndex],
                data.ActionValues[actionIndex]);

            moveIndex--;
            OnProgressHandler.Invoke(moveIndex);

            mutex.ReleaseMutex();
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
                
                return moveIndex < MoveCount;
            }
        }

        public int GetNextMovementAction()
        {
            int index = actionIndex + 1;
            while (index < data.Actions.Length && !IsMovement((GameAction)data.Actions[index]))
            {
                index++;
            }

            return index >= data.Actions.Length ? data.Actions.Length - 1 : index;     
        }


        public void Perform()
        {
            mutex.WaitOne();

            isReverse = false;

            DoPerform(
                (GameAction)data.Actions[actionIndex],
                data.ActionValues[actionIndex]);

            actionIndex = GetNextMovementAction();

            moveIndex++;
            OnProgressHandler.Invoke(moveIndex);

            mutex.ReleaseMutex();
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
            mutex.WaitOne();

            isReverse = false;

            yield return StartCoroutine(DoPerformCoroutine(
                (GameAction)data.Actions[actionIndex],
                data.ActionValues[actionIndex]));

            actionIndex = GetNextMovementAction();

            moveIndex++;
            OnProgressHandler.Invoke(moveIndex);

            mutex.ReleaseMutex();
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
            isPlaying = false;
            StopAllCoroutines();
            mutex.ReleaseMutex();
            Move(moveIndex - 1);
        }


        public void Stop()
        {
            isPlaying = false;
            StopAllCoroutines();
            mutex.ReleaseMutex();
            Move(0);
        }

        public IEnumerator ResumeCoroutine()
        {
            isPlaying = true;

            while (HasNext)
            {
                yield return StartCoroutine(PerformCoroutine());
            }

            isPlaying = false;

            if (OnSequenceFinishedHandler != null)
            {
                OnSequenceFinishedHandler.Invoke();
            }            
        }

    }
}