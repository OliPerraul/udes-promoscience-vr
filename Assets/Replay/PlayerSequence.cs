using Cirrus.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UnityEngine;

namespace UdeS.Promoscience.Replay
{
    public delegate void OnSequenceEvent(PlayerSequence sequence);

    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    public class State
    {
        public Dictionary<Vector2Int, Segment> Segments;
        public Dictionary<Vector2Int, Error> Errors;



    }


    public class PlayerSequence : Sequence
    {
        private Course course;

        protected Vector2Int nextlposition;

        protected Vector2Int prevlposition;

        [SerializeField]
        protected Error errorIndicatorTemplate;

        [SerializeField]
        private float drawTime = 0.6f;

        [SerializeField]
        private float normalWidth = 1.25f;

        [SerializeField]
        private float backtrackWidth = 2f;

        [SerializeField]
        private Segment currentSegment;

        private Segment CurrentSegment
        {
            get
            {
                return currentSegment;
            }

            set
            {
                if (currentSegment != null)
                {
                    currentSegment.Alpha = true;
                }

                currentSegment = value;

                if (currentSegment != null)
                {
                    currentSegment.Alpha = false;
                }

            }
        }

        protected override bool HasNext
        {
            get
            {
                return course.HasNext;
            }
        }

        protected override bool HasPrevious
        {
            get
            {
                return course.HasPrevious;
            }
        }


        [SerializeField]
        private GameObject arrowHead;

        [SerializeField]
        private Segment segmentTemplate;

        [SerializeField]
        private float previousSegmentAlpha = 0.5f;

        [SerializeField]
        private Material templateMaterial;

        [SerializeField]
        private Material templateBacktrackMaterial;

        private Material material;

        private Material backtrackMaterial;

        private Material materialAlpha;

        private Material backtrackMaterialAlpha;

        /// <summary>
        /// Contains the list of all segments (Active or innactive)
        /// We use this to adjust offset when needed
        /// </summary>
        private List<Segment> segments;

        private Dictionary<Vector2Int, Stack<Segment>> segmentDictionary;

        private Dictionary<Vector2Int, Stack<Segment>> errorDictionary;

        /// <summary>
        /// Contains the list of segments added in a particular step (usually 1, return to divergent many)
        /// </summary>
        private Stack<List<Segment>> added;

        /// <summary>
        /// Contains the list of segments added in a particular step (usually 1, return to divergent many)
        /// </summary>
        private Stack<List<Segment>> removed;

        public override int LocalMoveCount
        {
            get
            {
                return course.MoveCount;
            }
        }

        public override int LocalMoveIndex
        {
            get
            {
                return course.MoveIndex;
            }
        }

        private float offsetAmount = 0f;

        public PlayerSequence Create(
            Course course,
            Labyrinth labyrinth,
            Vector2Int startPosition)
        {
            PlayerSequence sequence = this.Create(labyrinth.GetLabyrinthPositionInWorldPosition(startPosition));

            sequence.labyrinth = labyrinth;
            sequence.startlposition = startPosition;
            sequence.lposition = startPosition;
            sequence.prevlposition = startPosition;
            sequence.nextlposition = startPosition;
            sequence.course = course;
            sequence.material.color = course.Team.TeamColor;
            sequence.backtrackMaterial.color = course.Team.TeamColor;
            sequence.materialAlpha.color = course.Team.TeamColor.SetA(previousSegmentAlpha);
            sequence.backtrackMaterialAlpha.color = course.Team.TeamColor.SetA(previousSegmentAlpha);

            sequence.arrowHead.GetComponentInChildren<SpriteRenderer>().color = sequence.material.color;

            return sequence;
        }

        public override void AdjustOffset(float amount)// maxOffset)
        {
            this.offsetAmount = amount;

            //Segment sgm;
            foreach (Segment sgm in segments)
            {
                sgm.AdjustOffset(amount);
            }
        }

        public override void Awake()
        {
            base.Awake();

            added = new Stack<List<Segment>>();
            removed = new Stack<List<Segment>>();

            segmentDictionary = new Dictionary<Vector2Int, Stack<Segment>>();
            errorDictionary = new Dictionary<Vector2Int, Stack<Segment>>();

            segments = new List<Segment>();
            material = new Material(templateMaterial);
            backtrackMaterial = new Material(templateBacktrackMaterial);
            materialAlpha = new Material(templateMaterial);
            backtrackMaterialAlpha = new Material(templateBacktrackMaterial);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (currentSegment != null)
            {
                arrowHead.gameObject.SetActive(true);

                arrowHead.transform.rotation = currentSegment.Rotation;

                arrowHead.transform.position = currentSegment.Position;
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

        public Vector3 GetTileEdgePositionFromDirection(Direction action)
        {
            switch (action)
            {
                case Direction.Down:
                    return (Vector3.forward * -Constants.TILE_SIZE / 2);

                case Direction.Up:
                    return (Vector3.forward * Constants.TILE_SIZE / 2);

                case Direction.Left:
                    return (Vector3.right * -Constants.TILE_SIZE / 2);

                case Direction.Right:
                    return (Vector3.right * Constants.TILE_SIZE / 2);
            }

            return Vector3.zero;
        }

        public float GetDirectionSideOffset(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    return 1f;
                case Direction.Up:
                    return -1f;

                case Direction.Right:
                case Direction.Left:
                    return 0;
            }

            return 0;
        }

        public float GetDirectionFrontOffset(Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return -1f;
                case Direction.Left:
                    return 1f;

                case Direction.Down:
                case Direction.Up:
                    return 0;
            }

            return 0;
        }

        public Direction GetDirection(Vector2Int origin, Vector2Int destination)
        {
            if (origin.y < destination.y)
            {
                return Direction.Down;
            }
            else if (origin.y > destination.y)
            {
                return Direction.Up;
            }
            else if (origin.x > destination.x)
            {
                return Direction.Left;
            }
            else
            {
                return Direction.Right;
            }
        }

        public bool IsOppositeDirection(Direction direction, Direction other)
        {
            switch (direction)
            {
                case Direction.Up:
                    return
                        other == Direction.Down;// ||
                                                //other == Direction.Right;

                case Direction.Down:
                    return
                        other == Direction.Up;// ||
                                              //other == Direction.Left;

                case Direction.Left:
                    return
                        //other == Direction.Down ||
                        other == Direction.Right;

                case Direction.Right:
                    return
                        //other == Direction.Up ||
                        other == Direction.Left;

                default:
                    return false;
            }
        }

        public void DoRemove(List<Segment> removed, Vector2Int lpos, Dictionary<Vector2Int, Stack<Segment>> source)
        {
            Stack<Segment> stack;

            // Check if dest was visited,
            // Otherwise create the stack
            if (source.TryGetValue(lpos, out stack))
            {
                // Remove destroyed segment               
                while (stack.Count != 0 && stack.Peek() == null)
                {
                    stack.Pop();
                }

                if (stack.Count > 0)
                {
                    // If segment is visible and we walk on top of it. Hide it..
                    if (!removed.Contains(stack.Peek()) &&
                        stack.Peek().gameObject.activeSelf)
                    {
                        stack.Peek().gameObject.SetActive(false);
                        removed.Add(stack.Peek());
                    }
                }
            }
            else
            {
                stack = new Stack<Segment>();
                source[lpos] = stack;
            }
        }

        public void Remove(
            List<Segment> removed,
            Vector2Int lpos)
        {
            DoRemove(removed, lpos, segmentDictionary);
            DoRemove(removed, lpos, errorDictionary);
        }

        public Segment Add(
            List<Segment> added,
            Vector2Int prevlpos,
            Vector2Int lpos,
            Vector2Int nextlpos,
            bool isBacktracking,
            bool isError=false)
        {
            Direction direction = GetDirection(prevlpos, lpos);

            bool isInversed = false;

            Vector3 middle = labyrinth.GetLabyrinthPositionInWorldPosition(lpos);
            Vector3 origin = middle + GetTileEdgePositionFromDirection(GetDirection(lpos, prevlpos));
            Vector3 destination = middle + GetTileEdgePositionFromDirection(GetDirection(lpos, nextlpos));

            bool isTurn = GetDirection(prevlpos, lpos) != GetDirection(lpos, nextlpos);

            float time = drawTime;

            // Start
            if (prevlpos == lpos)
            {
                time /= 2;
                isTurn = false;
                origin = middle;
            }
            // End
            else if (lpos == nextlpos)
            {
                time /= 2;
                isTurn = false;
                destination = middle;
            }
            // Dead-end
            else if (nextlpos == prevlpos)
            {
                time /= 2;
                isTurn = false;
                destination = middle;
            }

            CurrentSegment = isTurn ?
                segmentTemplate.CreateTurn(
                    transform,
                    origin,
                    middle,
                    destination,
                    direction,
                    isInversed,
                    isBacktracking ? backtrackMaterial : material,
                    isBacktracking ? backtrackMaterialAlpha : materialAlpha,
                    time,
                    isBacktracking ? backtrackWidth : normalWidth) :
                segmentTemplate.Create(
                    transform,
                    origin,
                    destination,
                    direction,
                    isInversed,
                    isBacktracking ? backtrackMaterial : material,
                    isBacktracking ? backtrackMaterialAlpha : materialAlpha,
                    time,
                    isBacktracking ? backtrackWidth : normalWidth);

            segmentDictionary[lpos].Push(CurrentSegment);

            if (isError)
            {
                var err = errorIndicatorTemplate.Create(transform, origin, middle, destination, isInversed, isTurn);
                errorDictionary[lpos].Push(err);
                added.Add(err);
            }

            segments.Add(CurrentSegment); // List<Segment> segments
            added.Add(CurrentSegment); // Stack<List<Segment>> stack
            
            return CurrentSegment;
        }

        // Use in return to divergent location
        private void Redraw(List<Segment> added, List<Segment> removed, Tile[] playerSteps, Tile[] wrong)
        {
            // Hide path
            for (int i = 0; i < wrong.Length; i++)
            {
                Remove(removed, wrong[i].Position);
            }

            // Hide current position
            Remove(removed, lposition);

            bool isBacktracking = playerSteps[0].color == TileColor.Red;
            Vector2Int prevlpos = playerSteps[0].Position;
            Vector2Int lpos = prevlpos;
            Vector2Int nextlpos = prevlpos;

            // n-1 steps
            for (int i = 1; i < playerSteps.Length; i++)
            {
                prevlpos = lpos;
                lpos = nextlpos;
                nextlpos = playerSteps[i].Position;
                isBacktracking = playerSteps[i].color == TileColor.Red;

                Add(
                    added,
                    prevlpos,
                    lpos,
                    nextlpos,
                    isBacktracking);
            }

            foreach (var sgm in added)
            {
                sgm.AdjustOffset(offsetAmount);
                sgm.Draw();
            }
        }

        private void Draw(
            List<Segment> added,
            List<Segment> removed,
            Vector2Int prevlpos,
            Vector2Int lpos,
            Vector2Int nextlpos,
            bool isBacktracking,
            bool isError=false)
        {
            Remove(
                removed, 
                lpos);

            Add(
                added,
                prevlpos,
                lpos,
                nextlpos,
                isBacktracking,
                isError);

            foreach (var sgm in added)
            {
                sgm.AdjustOffset(offsetAmount);
                sgm.Draw();
            }
        }
    

        private IEnumerator DrawCoroutine(
            List<Segment> addedList,
            List<Segment> removedList,
            Vector2Int prevlpos,
            Vector2Int lpos,
            Vector2Int nextlpos,
            bool isBacktracking,
            bool isError = false)
        {
            Add(
                addedList,
                prevlpos,
                lpos,
                nextlpos,
                isBacktracking,
                isError
                );

            CurrentSegment.AdjustOffset(offsetAmount);
            yield return StartCoroutine(CurrentSegment.DrawCoroutine());
        }

        protected override void DoPrevious()
        {
            if (course.CurrentAction != GameAction.Finish)
            {
                if (isReturnedToDivergent) isReturnedToDivergent = false;

                if (added.Count != 0)
                {
                    List<Segment> sgms = added.Pop();

                    foreach (Segment sgm in sgms)
                    {                   
                        segments.Remove(sgm);
                        Destroy(sgm.gameObject);
                    }
                }

                if (removed.Count != 0 && removed.Peek() != null)
                {
                    List<Segment> sgms = removed.Pop();

                    foreach (Segment sgm in sgms)
                    {
                        sgm.gameObject.SetActive(true);
                    }
                }

                nextlposition = lposition;
                lposition = prevlposition;
                prevlposition = GetMoveOrigin(lposition, course.PreviousAction);

                Stack<Segment> stk;
                if (segmentDictionary.TryGetValue(lposition, out stk))
                {
                    if (stk.Count != 0)
                    {
                        CurrentSegment = stk.Peek();
                    }
                }
            }

            course.Previous();

            if (course.OnPlayerSequenceProgressedHandler != null)
                course.OnPlayerSequenceProgressedHandler.Invoke();

        }

        public Vector2Int GetMoveDestination(Vector2Int lpos, GameAction action)
        {
            // From ReturnToDivergent, EndAction, CompleteRound
            // Simply walk to the 'nextlpos', we do not increment nextlpos

            lpos.y += action == GameAction.MoveUp ? -1 : 0;
            lpos.y += action == GameAction.MoveDown ? 1 : 0;
            lpos.x += action == GameAction.MoveLeft ? -1 : 0;
            lpos.x += action == GameAction.MoveRight ? 1 : 0;
            return lpos;
        }

        public Vector2Int GetMoveOrigin(Vector2Int lpos, GameAction action)
        {
            // From ReturnToDivergent, EndAction, CompleteRound
            // Simply walk to the 'nextlpos', we do not increment nextlpos

            lpos.y += action == GameAction.MoveUp ? 1 : 0;
            lpos.y += action == GameAction.MoveDown ? -1 : 0;
            lpos.x += action == GameAction.MoveLeft ? 1 : 0;
            lpos.x += action == GameAction.MoveRight ? -1 : 0;
            return lpos;
        }


        private bool isReturnedToDivergent = false;

        private ActionValue previousValue;

        protected override void DoNext()
        {
            if (course.CurrentAction != GameAction.Finish)
            {
                List<Segment> added = new List<Segment>();
                List<Segment> removed = new List<Segment>();

                bool isError = false;

                if (isReturnedToDivergent)
                {
                    isError = true;

                    Redraw(
                        added,
                        removed,
                        previousValue.playerSteps,
                        previousValue.wrongTiles);

                    // Are we redrawing from the start ?
                    // Yes: current == next
                    // No: current != next
                    lposition = previousValue.playerSteps.Length > 1 ?
                        previousValue.playerSteps[previousValue.playerSteps.Length - 2].Position :
                        previousValue.playerSteps[previousValue.playerSteps.Length - 1].Position;

                    nextlposition = previousValue.playerSteps[previousValue.playerSteps.Length - 1].Position;
                }

                isReturnedToDivergent = course.CurrentAction == GameAction.ReturnToDivergencePoint;
                previousValue = course.CurrentActionValue;

                bool isBacktracking = course.CurrentActionValue.previousColor == TileColor.Red;

                prevlposition = lposition;
                lposition = nextlposition;
                nextlposition = GetMoveDestination(lposition, course.CurrentAction);

                Draw(
                    added, 
                    removed, 
                    prevlposition, 
                    lposition, 
                    nextlposition,
                    isBacktracking,
                    isError);

                this.added.Push(added);
                this.removed.Push(removed);

                course.Next();

                if (course.OnPlayerSequenceProgressedHandler != null)
                    course.OnPlayerSequenceProgressedHandler.Invoke();
            }
        }

        protected override IEnumerator DoNextCoroutine()
        {
            yield return null;
            //if (course.CurrentAction != GameAction.Finish)
            //{
            //    List<Segment> added = new List<Segment>();
            //    List<Segment> removed = new List<Segment>();

            //    if (isReturnedToDivergent)
            //    {
            //        Redraw(
            //            added,
            //            removed,
            //            previousValue.playerSteps,
            //            previousValue.wrongTiles);

            //        // Are we redrawing from the start ?
            //        // Yes: current == next
            //        // No: current != next
            //        lposition = 
            //            previousValue.playerSteps.Length > 1 ?
            //            previousValue.playerSteps[previousValue.playerSteps.Length - 2].Position :
            //            previousValue.position;

            //        nextlposition = previousValue.position;
            //    }

            //    previousValue = course.CurrentActionValue;
            //    isBacktracking = course.CurrentActionValue.previousColor == TileColor.Red;

            //    prevlposition = lposition;
            //    lposition = nextlposition;
            //    nextlposition = GetMoveDestination(lposition, course.CurrentAction);

            //    yield return StartCoroutine(DrawCoroutine(added, removed, prevlposition, lposition, nextlposition));

            //    if (isReturnedToDivergent)
            //    {
            //        errorIndicatorTemplate.Create(added.Last().transform);
            //    }
                
            //    isReturnedToDivergent = course.CurrentAction == GameAction.ReturnToDivergencePoint;

            //    this.added.Push(added);
            //    this.removed.Push(removed);

            //    if (course.OnPlayerSequenceProgressedHandler != null)
            //        course.OnPlayerSequenceProgressedHandler.Invoke();
            //}

            //course.Next();
        }

    }
}