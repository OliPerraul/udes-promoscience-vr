using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;

using Cirrus.Extensions;

using System.Linq;
using System.Collections.Generic;

using System.Threading;
using System;

namespace UdeS.Promoscience.Replay
{
    public delegate void OnSequenceEvent(PlayerSequence sequence);

    public class PlayerSequence : MonoBehaviour
    {
        [SerializeField]
        private ScriptableReplayOptions replayOptions;

        [SerializeField]
        private float speed = 0.6f;

        private Labyrinth labyrinth;

        private Vector2Int labyrinthPosition;

        private Vector2Int lastLabyrinthPosition;

        private Vector3 targetPosition;

        private Course course;

        private Vector3 lastPosition;

        private TileColor lastColor;

        [SerializeField]
        private float drawTime = 0.6f;

        [SerializeField]
        private float normalWidth = 1.25f;

        [SerializeField]
        private float backtrackWidth = 2f;

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

        private List<Segment> segments;

        private Dictionary<Vector2Int, Stack<Segment>> dictionary;

        // We use a list because 'ReturnToDivergent' has many segments to undo when reverting
        private Stack<List<Segment>> stack;

        private int moveIndex = 0;

        public int MoveCount = 0;

        private bool isPlaying = false;

        private bool isBacktracking = false;

        private bool isMovingBackward = false;

        private Mutex mutex;

        private int index = 0;

        private int total = 0;

        private float maxOffset = 0f;

        public PlayerSequence Create(
            Course course,
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
            sequence.course = course;
            sequence.MoveCount = sequence.course.Actions.Aggregate(0, (x, y) => IsMovement((GameAction)y) ? x + 1 : x);
            sequence.material.color = course.Team.TeamColor;            
            sequence.backtrackMaterial.color = course.Team.TeamColor;
            sequence.arrowHead.GetComponentInChildren<SpriteRenderer>().color = sequence.material.color;            

            return sequence;
        }

        public void Adjust(int index, int total, float maxOffset)
        {
            this.index = index;
            this.total = total;
            this.maxOffset = maxOffset;

            float amount = ((float)index) / total;

            //Segment sgm;
            foreach (Segment sgm in segments)
            {
                sgm.AdjustOffset(amount, maxOffset);                
            }
        }

        public void Awake()
        {
            mutex = new Mutex();
            stack = new Stack<List<Segment>>();
            dictionary = new Dictionary<Vector2Int, Stack<Segment>>();
            segments = new List<Segment>();

            material = new Material(templateMaterial);          
            backtrackMaterial = new Material(templateBacktrackMaterial);
        }

        // TODO: remove debug
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
                arrowHead.gameObject.SetActive(true);

                arrowHead.transform.rotation =
                    Quaternion.LookRotation(
                        currentSegment.Destination - currentSegment.Origin,
                        Vector3.up);

                arrowHead.transform.position = currentSegment.Current;
            }
            else
            {
                arrowHead.gameObject.SetActive(false);
            }
        }

        public void HandleAction(ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case ReplayAction.Previous:

                    mutex.WaitOne();

                    if (isPlaying)
                    {
                        Pause();
                    }                    

                    if (HasPrevious)
                    {
                        Reverse();
                    }

                    mutex.ReleaseMutex();

                    break;

                case ReplayAction.Next:

                    mutex.WaitOne();

                    if (isPlaying)
                    {
                        Pause();
                    }

                    if (HasNext)
                    {
                        Perform();
                    }


                    mutex.ReleaseMutex();

                    break;

                case ReplayAction.Play:
                    
                    // TODO: invalid use of mutex because 'Play' starts a coroutine
                    mutex.WaitOne();

                    Move(0);
                    Resume();

                    mutex.ReleaseMutex();

                    break;

                case ReplayAction.Resume:
                    Resume();
                    break;

                case ReplayAction.Pause:

                    mutex.WaitOne();

                    Pause();

                    mutex.ReleaseMutex();

                    break;

                case ReplayAction.Stop:

                    mutex.WaitOne();

                    Stop();

                    mutex.ReleaseMutex();

                    break;

                case ReplayAction.Slide:

                    mutex.WaitOne();

                    int current = (int)args[0];
                    Move(current);

                    mutex.ReleaseMutex();

                    break;
            }
        }

        public void OnMouseEvent()
        {
            if (replayOptions.OnSequenceSelectedHandler != null)
                replayOptions.OnSequenceSelectedHandler.Invoke(course);
        }

        public Segment AddSegment(Vector2Int lo, Vector2Int ld, Vector3 origin, Vector3 dest)
        {
            Stack<Segment> stack;
            // Check to turn off dest
            if (dictionary.TryGetValue(ld, out stack))
            {
                if (stack.Count > 0)
                {
                    stack.Peek().gameObject.SetActive(false);
                }
            }

            // Check if origin was visited,
            // Otherwise create the stack
            if (dictionary.TryGetValue(lo, out stack))
            {
                if (stack.Count > 0)
                {
                    stack.Peek().gameObject.SetActive(false);
                }
            }
            else
            {
                stack = new Stack<Segment>();
                dictionary[lo] = stack;
            }

            Quaternion rotation = Quaternion.LookRotation(
                dest - origin,
                Vector3.up);

            currentSegment = segmentTemplate.Create(
                transform,
                lo,
                origin,
                dest,
                rotation * (isMovingBackward ? -Vector3.right : Vector3.right),
                isBacktracking ? backtrackMaterial : material,
                drawTime,
                isBacktracking ? backtrackWidth : normalWidth);

            currentSegment.OnMouseEvent += OnMouseEvent;

            // Add to history
            List<Segment> list = new List<Segment>();
            this.stack.Push(list);
            list.Add(currentSegment);
            segments.Add(currentSegment);

            // Add to segment layout
            dictionary[lo].Push(currentSegment);

            return currentSegment;
        }


        // TODO: Use in return to divergent location
        private void Redraw(Tile[] tiles)
        {
            positions = tiles.Select
                    (x => labyrinth.GetLabyrinthPositionInWorldPosition(x.Position)).ToArray();

            Segment segment;
            Stack<Segment> stack;
            for (int i = 1; i < tiles.Length; i++)
            {
                Vector3 offset = Vector3.zero;
                if (dictionary.TryGetValue(tiles[i].Position, out stack))
                {
                    stack.Peek().gameObject.SetActive(false);
                }
                else
                {
                    stack = new Stack<Segment>();
                    dictionary[tiles[i].Position] = stack;
                }

                Vector3 origin = labyrinth.GetLabyrinthPositionInWorldPosition(tiles[i - 1].Position);
                Vector3 dest = labyrinth.GetLabyrinthPositionInWorldPosition(tiles[i].Position);

                Segment sgm = AddSegment(tiles[i - 1].Position, tiles[i].Position, origin, dest);
                currentSegment.Draw();
                currentSegment.AdjustOffset(((float)index) / total, maxOffset);
            }
        }


        private void Draw(Vector2Int labOrigin, Vector2Int labDest, Vector3 origin, Vector3 dest)
        {
            Segment sgm = AddSegment(labOrigin, labDest, origin, dest);
            sgm.Draw();
            sgm.AdjustOffset(((float)index) / total, maxOffset);
        }

        private IEnumerator DrawCoroutine(Vector2Int lo, Vector2Int ld, Vector3 origin, Vector3 dest)
        {
            Segment sgm = AddSegment(lo, ld, origin, dest);
            yield return StartCoroutine(sgm.DrawCoroutine());
        }

        private bool IsMovement(GameAction action)
        {
            switch (action)
            {
                case GameAction.MoveUp:
                case GameAction.MoveDown:
                case GameAction.MoveLeft:
                case GameAction.MoveRight:
                case GameAction.PaintFloorRed:
                case GameAction.PaintFloorYellow:
                    return true;
                default:
                    return false;
            }
        }

        private void Move(int target)
        {
            if (target == moveIndex)
                return;

            if (Mathf.Sign(target - moveIndex) < 0)
            {
                while (HasPrevious)
                {
                    if (moveIndex <= target)
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

        public void OnSegmentClicked()
        {

        }

        private bool HasPrevious
        {
            get
            {
                if (course.Actions.Length == 0)
                    return false;

                return moveIndex > 0;
            }
        }

        private int GetPreviousMovementAction()
        {
            int index = course.CurrentActionIndex - 1;
            while (index >= 0 && !IsMovement((GameAction)course.Actions[index]))
            {
                index--;
            }

            return index < 0 ? 0 : index;
        }

        public void Reverse()
        {
            course.CurrentActionIndex = GetPreviousMovementAction();

            DoReverse(
                (GameAction)course.Actions[course.CurrentActionIndex],
                course.ActionValues[course.CurrentActionIndex]);

            moveIndex--;
            if(replayOptions.OnProgressHandler != null)
            replayOptions.OnProgressHandler.Invoke(moveIndex);
        }

        private void DoReverse(GameAction gameAction, string info)
        {
            Segment sgm = null;
            Stack<Segment> stk;

            switch (gameAction)
            {
                case GameAction.MoveUp:

                    isMovingBackward = true;

                    if (dictionary.TryGetValue(labyrinthPosition, out stk))
                    {
                        if (stk.Count != 0)
                        {
                            stk.Peek().gameObject.SetActive(true);
                        }
                    }

                    if (stack.Count != 0)
                    {
                        sgm = stack.Pop().First();
                        segments.Remove(sgm);
                        dictionary[sgm.LOrigin].Pop();
                        labyrinthPosition = sgm.LOrigin;
                        Destroy(sgm.gameObject);
                    }

                    if (stack.Count != 0)
                    {
                        currentSegment = stack.Peek().First();
                    }


                    break;

                case GameAction.MoveDown:

                    isMovingBackward = false;

                    if (dictionary.TryGetValue(labyrinthPosition, out stk))
                    {
                        if (stk.Count != 0)
                        {
                            stk.Peek().gameObject.SetActive(true);
                        }
                    }

                    // undo history
                    // TODO: handle 'ReturnTODivergent'

                    if (stack.Count != 0)
                    {
                        sgm = stack.Pop().First();
                        segments.Remove(sgm);
                        dictionary[sgm.LOrigin].Pop();
                        labyrinthPosition = sgm.LOrigin;
                        Destroy(sgm.gameObject);
                    }

                    if (stack.Count != 0)
                    {
                        currentSegment = stack.Peek().First();
                    }

                    break;

                case GameAction.MoveLeft:

                    isMovingBackward = true;

                    if (dictionary.TryGetValue(labyrinthPosition, out stk)) 
                    {
                        if (stk.Count != 0)
                        {
                            stk.Peek().gameObject.SetActive(true);
                        }
                    }

                    // undo history
                    // TODO: handle 'ReturnTODivergent'
                    if (stack.Count != 0)
                    {
                        sgm = stack.Pop().First();
                        segments.Remove(sgm);
                        dictionary[sgm.LOrigin].Pop();
                        labyrinthPosition = sgm.LOrigin;                        
                        Destroy(sgm.gameObject);
                    }

                    if (stack.Count != 0)
                    {
                        currentSegment = stack.Peek().First();
                    }


                    break;

                case GameAction.MoveRight:

                    isMovingBackward = false;

                    if (dictionary.TryGetValue(labyrinthPosition, out stk))
                    {
                        if (stk.Count != 0)
                        {
                            stk.Peek().gameObject.SetActive(true);
                        }
                    }

                    // undo history
                    // TODO: handle 'ReturnTODivergent'
                    if (stack.Count != 0)
                    {
                        sgm = stack.Pop().First();
                        segments.Remove(sgm);
                        dictionary[sgm.LOrigin].Pop();
                        labyrinthPosition = sgm.LOrigin;
                        Destroy(sgm.gameObject);
                    }

                    if (stack.Count != 0)
                    {
                        currentSegment = stack.Peek().First();
                    }
                    else
                    {
                        currentSegment = null;
                    }

                    break;

                case GameAction.PaintFloorRed:
                    isBacktracking = true;

                    break;

                case GameAction.PaintFloorYellow:
                    isBacktracking = false;

                    break;
            }
        }

        private bool HasNext
        {
            get
            {

                if (course.Actions.Length == 0)
                    return false;
                
                return moveIndex < MoveCount;
            }
        }

        private int GetNextMovementAction()
        {
            int index = course.CurrentActionIndex + 1;
            while (index < course.Actions.Length && !IsMovement((GameAction)course.Actions[index]))
            {
                index++;
            }

            return index >= course.Actions.Length ? course.Actions.Length - 1 : index;     
        }


        private void Perform()
        {
            DoPerform(
                (GameAction)course.Actions[course.CurrentActionIndex],
                course.ActionValues[course.CurrentActionIndex]);

            course.CurrentActionIndex = GetNextMovementAction();

            moveIndex++;

            if (replayOptions.OnProgressHandler != null)
            {
                replayOptions.OnProgressHandler.Invoke(moveIndex);
            }
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
                        labyrinthPosition,
                        origin,
                        dest);

                    break;

                case GameAction.MoveRight:

                    labyrinthPosition.x += 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    Draw(
                        lastLabyrinthPosition,
                        labyrinthPosition,
                        origin,
                        dest);

                    break;

                case GameAction.MoveDown:

                    labyrinthPosition.y += 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    Draw(
                        lastLabyrinthPosition,
                        labyrinthPosition,
                        origin,
                        dest);

                    break;

                case GameAction.MoveLeft:

                    labyrinthPosition.x -= 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    Draw(
                        lastLabyrinthPosition,
                        labyrinthPosition,
                        origin,
                        dest);

                    break;

                case GameAction.TurnLeft:
                    break;

                case GameAction.TurnRight:
                    break;

                case GameAction.PaintFloorRed:
                    isBacktracking = true;

                    break;

                case GameAction.PaintFloorYellow:
                    isBacktracking = false;

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

        private IEnumerator PerformCoroutine()
        {
            yield return StartCoroutine(DoPerformCoroutine(
                (GameAction)course.Actions[course.CurrentActionIndex],
                course.ActionValues[course.CurrentActionIndex]));

            course.CurrentActionIndex = GetNextMovementAction();

            moveIndex++;
            if(replayOptions.OnProgressHandler != null)
            replayOptions.OnProgressHandler.Invoke(moveIndex);
        }

        private IEnumerator DoPerformCoroutine(GameAction gameAction, string info)
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
                        labyrinthPosition,
                        origin,
                        dest));

                    break;

                case GameAction.MoveRight:

                    labyrinthPosition.x += 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    yield return StartCoroutine(DrawCoroutine(
                        lastLabyrinthPosition,
                        labyrinthPosition,
                        origin,
                        dest));

                    break;

                case GameAction.MoveDown:

                    labyrinthPosition.y += 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    yield return StartCoroutine(DrawCoroutine(
                        lastLabyrinthPosition,
                        labyrinthPosition,
                        origin,
                        dest));

                    break;

                case GameAction.MoveLeft:

                    labyrinthPosition.x -= 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    yield return StartCoroutine(DrawCoroutine(
                        lastLabyrinthPosition,
                        labyrinthPosition,
                        origin,
                        dest));

                    break;

                case GameAction.TurnLeft:
                    break;

                case GameAction.TurnRight:
                    break;

                case GameAction.PaintFloorRed:
                    isBacktracking = true;

                    break;

                case GameAction.PaintFloorYellow:
                    isBacktracking = false;

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


        private void Resume()
        {
            StartCoroutine(ResumeCoroutine());
        }

        private void Pause()
        {
            isPlaying = false;
            StopAllCoroutines();
            Move(moveIndex - 1);
        }


        private void Stop()
        {
            isPlaying = false;
            StopAllCoroutines();
            Move(0);
        }

        private IEnumerator ResumeCoroutine()
        {
            isPlaying = true;

            while (HasNext)
            {
                yield return StartCoroutine(PerformCoroutine());
            }

            isPlaying = false;

            if (replayOptions.OnSequenceFinishedHandler != null)
            {
                replayOptions.OnSequenceFinishedHandler.Invoke();
            }            
        }

    }
}