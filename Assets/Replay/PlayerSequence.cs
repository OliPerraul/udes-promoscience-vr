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

        /// <summary>
        /// contains the layout of the map. Used to hide places where we have already visited
        /// </summary>
        private Dictionary<Vector2Int, Stack<Segment>> origins;

        private Dictionary<Vector2Int, Stack<Segment>> destinations;


        /// <summary>
        /// Contains the list of segments added in a particular step (usually 1, return to divergent many)
        /// </summary>
        private Stack<List<Segment>> added;

        /// <summary>
        /// Contains the list of segments added in a particular step (usually 1, return to divergent many)
        /// </summary>
        private Stack<List<Segment>> removed;

        private int moveCount = 0;

        public override int LocalMoveCount
        {
            get
            {
                return moveCount;
            }
        }

        private bool isBacktracking = false;

        private bool isMovingBackward = false;

        private float offsetAmount = 0f;

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

            sequence.materialAlpha.color = course.Team.TeamColor.SetA(previousSegmentAlpha);
            sequence.backtrackMaterialAlpha.color = course.Team.TeamColor.SetA(previousSegmentAlpha);

            sequence.arrowHead.GetComponentInChildren<SpriteRenderer>().color = sequence.material.color;            

            return sequence;
        }

        public void Adjust(float amount)// maxOffset)
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

            origins = new Dictionary<Vector2Int, Stack<Segment>>();
            destinations = new Dictionary<Vector2Int, Stack<Segment>>();

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

        public Segment AddSegment(List<Segment> added, List<Segment> removed, Vector2Int lo, Vector2Int ld)
        {
            Vector3 originPosition = labyrinth.GetLabyrinthPositionInWorldPosition(lo);
            Vector3 destPosition = labyrinth.GetLabyrinthPositionInWorldPosition(ld);

            Stack<Segment> origins;
            Stack<Segment> destinations;


            // Check if dest was visited,
            // Otherwise create the stack


            if (this.destinations.TryGetValue(ld, out destinations))
            {
                if (destinations.Count > 0)
                {
                    if (!removed.Contains(destinations.Peek()))
                    {
                        destinations.Peek().gameObject.SetActive(false);
                        removed.Add(destinations.Peek());
                    }
                }
            }
            else
            {
                destinations = new Stack<Segment>();
                this.destinations[ld] = destinations;
            }


            // Check if origin was visited,
            // Otherwise create the stack

            if (this.origins.TryGetValue(lo, out origins))
            {
                if (origins.Count > 0)
                {
                    if (!removed.Contains(origins.Peek()))
                    {
                        origins.Peek().gameObject.SetActive(false);
                        removed.Add(origins.Peek());
                    }
                }
            }
            else
            {
                origins = new Stack<Segment>();
                this.origins[lo] = origins;
            }

            Quaternion rotation = Quaternion.LookRotation(
                destPosition - originPosition,
                Vector3.up);

            CurrentSegment = segmentTemplate.Create(
                transform,
                lo,
                ld,
                originPosition,
                destPosition,
                rotation * (isMovingBackward ? -Vector3.right : Vector3.right),
                isBacktracking ? backtrackMaterial : material,
                isBacktracking ? backtrackMaterialAlpha : materialAlpha,
                drawTime,
                isBacktracking ? backtrackWidth : normalWidth);

            CurrentSegment.OnMouseEvent += OnMouseEvent;

            origins.Push(CurrentSegment); // Dictionary<Vector2int, Segment> dictionary
            destinations.Push(CurrentSegment);

            segments.Add(CurrentSegment); // List<Segment> segments
            added.Add(CurrentSegment); // Stack<List<Segment>> stack

            return CurrentSegment;
        }

        // Use in return to divergent location
        private void Redraw(Tile[] playerSteps, Tile[] wrong)
        {
            //Vector3[] positions = wrong.Select
            //        (x => labyrinth.GetLabyrinthPositionInWorldPosition(x.Position)).ToArray();

            // Hide path
            Stack<Segment> stk;
            List<Segment> removed = new List<Segment>();
            for (int i = 0; i < wrong.Length; i++)
            {
                if(origins.TryGetValue(wrong[i].Position, out stk))
                {
                    stk.Peek().gameObject.SetActive(false);
                    removed.Add(stk.Peek());
                }
            }

            // Redraw
            List<Segment> added = new List<Segment>();
            for (int i = 1; i < playerSteps.Length; i++)
            {
                Vector3 origin = labyrinth.GetLabyrinthPositionInWorldPosition(playerSteps[i - 1].Position);
                Vector3 dest = labyrinth.GetLabyrinthPositionInWorldPosition(playerSteps[i].Position);

                AddSegment(added, removed, playerSteps[i - 1].Position, playerSteps[i].Position);
                CurrentSegment.AdjustOffset(offsetAmount);
                CurrentSegment.Draw();
            }

            this.added.Push(added);
            this.removed.Push(removed);
        }

        private void Draw(Vector2Int labOrigin, Vector2Int labDest)
        {
            List<Segment> added = new List<Segment>();
            List<Segment> removed = new List<Segment>();

            AddSegment(added, removed, labOrigin, labDest);

            this.added.Push(added);
            this.removed.Push(removed);

            CurrentSegment.AdjustOffset(offsetAmount);
            CurrentSegment.Draw();
        }

        private IEnumerator DrawCoroutine(Vector2Int lo, Vector2Int ld)
        {
            List<Segment> addedList = new List<Segment>();
            List<Segment> removedList = new List<Segment>();
            AddSegment(addedList, removedList, lo, ld);

            added.Push(addedList);
            removed.Push(removedList);

            CurrentSegment.AdjustOffset(offsetAmount);
            yield return StartCoroutine(CurrentSegment.DrawCoroutine());
        }

        private bool IsMovement(GameAction action)
        {
            switch (action)
            {
                case GameAction.MoveUp:
                case GameAction.MoveDown:
                case GameAction.MoveLeft:
                case GameAction.MoveRight:
                case GameAction.ReturnToDivergencePoint:
                    return true;
                default:
                    return false;
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

            isBacktracking = course.CurrentActionValue.tile.color == TileColor.Red;
            isMovingBackward = course.CurrentAction == GameAction.MoveUp || course.CurrentAction == GameAction.MoveLeft;

            // Remove added
            if (added.Count != 0)
            {
                List<Segment> sgms = added.Pop();

                foreach (Segment sgm in sgms)
                {
                    origins[sgm.LOrigin].Pop();
                    destinations[sgm.LDest].Pop();
                    labyrinthPosition = sgm.LOrigin;

                    segments.Remove(sgm);
                    Destroy(sgm.gameObject);
                }
            }

            // Restore removed (Used only by return to divergent)
            if (removed.Count != 0 && removed.Peek() != null)
            {
                List<Segment> sgms = removed.Pop();

                foreach (Segment sgm in sgms)
                {
                    sgm.gameObject.SetActive(true);
                }
            }

            if (added.Count != 0)
            {
                CurrentSegment = added.Peek().First();
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

        }

        private void DoPerform(GameAction gameAction, ActionValue value)
        {
            lastLabyrinthPosition = labyrinthPosition;
            lastColor = labyrinth.GetTileColor(lastLabyrinthPosition);

            isBacktracking = value.tile.color == TileColor.Red;

            switch (gameAction)
            {
                case GameAction.MoveUp:

                    isMovingBackward = true;

                    labyrinthPosition.y -= 1;

                    Draw(
                        lastLabyrinthPosition,
                        labyrinthPosition);

                    break;

                case GameAction.MoveRight:

                    isMovingBackward = false;

                    labyrinthPosition.x += 1;

                    Draw(
                        lastLabyrinthPosition,
                        labyrinthPosition);

                    break;

                case GameAction.MoveDown:

                    isMovingBackward = false;

                    labyrinthPosition.y += 1;

                    Draw(
                        lastLabyrinthPosition,
                        labyrinthPosition);

                    break;

                case GameAction.MoveLeft:

                    isMovingBackward = true;

                    labyrinthPosition.x -= 1;

                    Draw(
                        lastLabyrinthPosition,
                        labyrinthPosition);

                    break;

                case GameAction.ReturnToDivergencePoint:

                    labyrinthPosition = value.position;
                    Redraw(value.playerSteps, value.wrongTiles);
                    break;
            }
        }

        protected override IEnumerator PerformCoroutine()
        {
            yield return StartCoroutine(DoPerformCoroutine(
                course.CurrentAction,
                course.CurrentActionValue));

            course.CurrentActionIndex = GetNextMovementAction();
        }

        private IEnumerator DoPerformCoroutine(GameAction gameAction, ActionValue value)
        {

            lastLabyrinthPosition = labyrinthPosition;
            lastColor = labyrinth.GetTileColor(lastLabyrinthPosition);

            isBacktracking = value.tile.color == TileColor.Red;

            switch (gameAction)
            {
                case GameAction.MoveUp:

                    labyrinthPosition.y -= 1;

                    yield return StartCoroutine(DrawCoroutine(
                        lastLabyrinthPosition,
                        labyrinthPosition));

                    break;

                case GameAction.MoveRight:

                    labyrinthPosition.x += 1;

                    yield return StartCoroutine(DrawCoroutine(
                        lastLabyrinthPosition,
                        labyrinthPosition));

                    break;

                case GameAction.MoveDown:

                    labyrinthPosition.y += 1;

                    yield return StartCoroutine(DrawCoroutine(
                        lastLabyrinthPosition,
                        labyrinthPosition));

                    break;

                case GameAction.MoveLeft:

                    labyrinthPosition.x -= 1;

                    yield return StartCoroutine(DrawCoroutine(
                        lastLabyrinthPosition,
                        labyrinthPosition));

                    break;

                case GameAction.ReturnToDivergencePoint:

                    labyrinthPosition = value.position;
                    Redraw(value.playerSteps, value.wrongTiles);

                    yield return new WaitForSeconds(speed);

                    break;
            }

            yield return null;
        }


    }
}