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

        /// <summary>
        /// Contains the list of all segments (Active or innactive)
        /// We use this to adjust offset when needed
        /// </summary>
        private List<Segment> segments;

        /// <summary>
        /// contains the layout of the map. Used to hide places where we have already visited
        /// </summary>
        private Dictionary<Vector2Int, Stack<Segment>> dictionary;

        /// <summary>
        /// Contains the list of segments added in a particular step (usually 1, return to divergent many)
        /// </summary>
        private Stack<List<Segment>> added;

        /// <summary>
        /// Contains the list of segments added in a particular step (usually 1, return to divergent many)
        /// </summary>
        private Stack<List<Segment>> removed;

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

            added = new Stack<List<Segment>>();
            removed = new Stack<List<Segment>>();
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

        public Segment AddSegment(List<Segment> list, Vector2Int lo, Vector2Int ld, Vector3 origin, Vector3 dest)
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
                        
            segments.Add(currentSegment); // List<Segment> segments
            stack.Push(currentSegment); // Dictionary<Vector2int, Segment> dictionary
            list.Add(currentSegment); // Stack<List<Segment>> stack

            return currentSegment;
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
                if(dictionary.TryGetValue(wrong[i].Position, out stk))
                {
                    stk.Peek().gameObject.SetActive(false);
                    removed.Add(stk.Peek());

                    this.removed.Push(removed);
                }
            }

            // Redraw
            List<Segment> added = new List<Segment>();
            for (int i = 1; i < playerSteps.Length; i++)
            {
                Vector3 origin = labyrinth.GetLabyrinthPositionInWorldPosition(playerSteps[i - 1].Position);
                Vector3 dest = labyrinth.GetLabyrinthPositionInWorldPosition(playerSteps[i].Position);

                Segment sgm = AddSegment(added, playerSteps[i - 1].Position, playerSteps[i].Position, origin, dest);
                currentSegment.Draw();
                currentSegment.AdjustOffset(((float)index) / total, maxOffset);
            }

            this.added.Push(added);
        }

        private void Draw(Vector2Int labOrigin, Vector2Int labDest, Vector3 origin, Vector3 dest)
        {
            List<Segment> list = new List<Segment>();
            Segment sgm = AddSegment(list, labOrigin, labDest, origin, dest);
            added.Push(list);
            removed.Push(null);

            sgm.Draw();
            sgm.AdjustOffset(((float)index) / total, maxOffset);
        }

        private IEnumerator DrawCoroutine(Vector2Int lo, Vector2Int ld, Vector3 origin, Vector3 dest)
        {
            List<Segment> addedList = new List<Segment>();
            Segment sgm = AddSegment(addedList, lo, ld, origin, dest);
            added.Push(addedList);
            removed.Push(null);

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
                case GameAction.ReturnToDivergencePoint:
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
            isBacktracking = value.tile.color == TileColor.Red;
            isMovingBackward = gameAction == GameAction.MoveUp || gameAction == GameAction.MoveLeft;

            Stack<Segment> stk;
            if (dictionary.TryGetValue(labyrinthPosition, out stk))
            {
                if (stk.Count != 0)
                {
                    stk.Peek().gameObject.SetActive(true);
                }
            }

            // Remove added
            if (added.Count != 0)
            {
                List<Segment> sgms = added.Pop();

                foreach (Segment sgm in sgms)
                {
                    dictionary[sgm.LOrigin].Pop();
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
                currentSegment = added.Peek().First();
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

                    labyrinthPosition = value.position;
                    Redraw(value.playerSteps, value.wrongTiles);

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

                    Redraw(actionInfo.playerSteps, actionInfo.wrongTiles);
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