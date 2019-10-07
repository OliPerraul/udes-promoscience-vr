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

    public class PlayerSequence : Sequence
    {
        private Vector2Int lastLabyrinthPosition;

        private Course course;

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
        private Material templateMaterial;

        [SerializeField]
        private Material templateBacktrackMaterial;

        private Material material;

        private Material backtrackMaterial;

        private List<Segment> segments;

        private Dictionary<Vector2Int, Stack<Segment>> dictionary;

        // We use a list because 'ReturnToDivergent' has many segments to undo when reverting
        // TODO further simplify the Reverse code using a stack of ActionValue
        private Stack<List<Segment>> stack;

        private int moveIndex = 0;

        public override int MoveIndex
        {
            get
            {
                return moveIndex;
            }
        }


        private int moveCount = 0;

        public override int MoveCount
        {
            get
            {
                return moveCount;
            }
        }

        private bool isBacktracking = false;

        private bool isMovingBackward = false;

        private int index = 0;

        private int total = 0;

        private float maxOffset = 0f;

        public PlayerSequence Create(
            Course course,
            Labyrinth labyrinth,
            Vector2Int startPosition)
        {
            PlayerSequence sequence = Instantiate(
                gameObject,
                labyrinth.GetLabyrinthPositionInWorldPosition(startPosition), Quaternion.identity)
                .GetComponent<PlayerSequence>();

            sequence.labyrinth = labyrinth;
            sequence.labyrinthPosition = startPosition;
            sequence.course = course;
            sequence.moveCount = sequence.course.Actions.Aggregate(0, (x, y) => IsMovement((GameAction)y) ? x + 1 : x);
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

        public override void Awake()
        {
            base.Awake();

            stack = new Stack<List<Segment>>();
            dictionary = new Dictionary<Vector2Int, Stack<Segment>>();
            segments = new List<Segment>();

            material = new Material(templateMaterial);          
            backtrackMaterial = new Material(templateBacktrackMaterial);
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

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
            Vector3[] positions = tiles.Select
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
                    return true;
                default:
                    return false;
            }
        }

        protected override void Move(int target)
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


        // TODO why not simply "Pop a stack"
        private int GetPreviousMovementAction()
        {
            int index = course.CurrentActionIndex - 1;
            while (index >= 0 && !IsMovement((GameAction)course.Actions[index]))
            {
                index--;
            }

            return index < 0 ? 0 : index;
        }

        protected override void Reverse()
        {
            course.CurrentActionIndex = GetPreviousMovementAction();

            DoReverse(
                (GameAction)course.Actions[course.CurrentActionIndex],
                course.CurrentActionValue);

            moveIndex--;
            if(replayOptions.OnProgressHandler != null)
            replayOptions.OnProgressHandler.Invoke(moveIndex);
        }

        private void DoReverse(GameAction gameAction, ActionValue value)
        {
            Segment sgm = null;
            isBacktracking = value.tile.color == TileColor.Red;
            isMovingBackward = gameAction == GameAction.MoveUp || gameAction == GameAction.MoveLeft;

            // TODO handle multiple segments (return to divergent)
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
                currentSegment.gameObject.SetActive(true);
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


        protected override void Perform()
        {
            DoPerform(
                (GameAction)course.Actions[course.CurrentActionIndex],
                course.CurrentActionValue);

            course.CurrentActionIndex = GetNextMovementAction();

            moveIndex++;

            if (replayOptions.OnProgressHandler != null)
            {
                replayOptions.OnProgressHandler.Invoke(moveIndex);
            }
        }

        private void DoPerform(GameAction gameAction, ActionValue value)
        {
            lastLabyrinthPosition = labyrinthPosition;
            lastColor = labyrinth.GetTileColor(lastLabyrinthPosition);

            isBacktracking = value.tile.color == TileColor.Red;

            Vector3 origin = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);
            Vector3 dest;

            switch (gameAction)
            {
                case GameAction.MoveUp:

                    isMovingBackward = true;

                    labyrinthPosition.y -= 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    Draw(
                        lastLabyrinthPosition,
                        labyrinthPosition,
                        origin,
                        dest);

                    break;

                case GameAction.MoveRight:

                    isMovingBackward = false;

                    labyrinthPosition.x += 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    Draw(
                        lastLabyrinthPosition,
                        labyrinthPosition,
                        origin,
                        dest);

                    break;

                case GameAction.MoveDown:

                    isMovingBackward = false;

                    labyrinthPosition.y += 1;
                    dest = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    Draw(
                        lastLabyrinthPosition,
                        labyrinthPosition,
                        origin,
                        dest);

                    break;

                case GameAction.MoveLeft:

                    isMovingBackward = true;

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

                    //ActionValue actionInfo = JsonUtility.FromJson<ActionValue>(info);
                    labyrinthPosition = value.position;
                    //targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    //transform.position = targetPosition;
                    transform.rotation = value.rotation;

                    Redraw(value.playerSteps);

                    break;
            }
        }

        protected override IEnumerator PerformCoroutine()
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
                    //targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

                    //transform.position = targetPosition;
                    transform.rotation = actionInfo.rotation;

                    Redraw(actionInfo.playerSteps);
                    break;
            }

            yield return null;

        }

        public override void Play()
        {
            //throw new NotImplementedException();
        }


        public override void Pause()
        {
            isPlaying = false;
            StopAllCoroutines();
            Move(moveIndex - 1);
        }


        public override void Stop()
        {
            isPlaying = false;
            StopAllCoroutines();
            Move(0);
        }

    }
}