using Cirrus.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UnityEngine;

namespace UdeS.Promoscience.Replays
{
    public delegate void OnSequenceEvent(PlayerSequence sequence);

    public class PlayerSequence : Sequence
    {
        public class State
        {
            // Simply hold the references to the segments gameobject
            // Does not contain reference to new segments per state
            public Dictionary<Vector2Int, Segment> Segments = new Dictionary<Vector2Int, Segment>();

            public Dictionary<Vector2Int, Segment> Errors = new Dictionary<Vector2Int, Segment>();

            public Segment Head;

            public Vector2Int PrevLPos;

            public Vector2Int LPos;

            public Vector2Int NextLPos;

            public GameAction Action;

            public ActionValue ActionValue;

            public bool IsBacktracking = false;
        }

        private Course course;

        [SerializeField]
        protected Error errorIndicatorTemplate;

        [SerializeField]
        private float drawTime = 0.6f;

        [SerializeField]
        private float normalWidth = 1.25f;

        [SerializeField]
        private float backtrackWidth = 2f;

        [SerializeField]
        private float cameraHeightDifference = 10f;

        private Segment CurrentSegment
        {
            get
            {
                if (CurrentState == null)
                    return null;

                return CurrentState.Head;
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
        private List<State> states;

        private int stateIndex = 0;

        private State CurrentState
        {
            get
            {
                if (stateIndex >= states.Count)
                    return null;

                return states[stateIndex];
            }
        }

        private State PreviousState
        {
            get
            {                
                return stateIndex == 0 ? states[stateIndex] : states[stateIndex - 1];
            }
        }

        // List of all segments added so that they can be adjusted
        private List<Segment> segments;


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
                return course.CurrentMoveIndex;
            }
        }

        private float offsetAmount = 0f;

        public PlayerSequence Create(
            ReplayManagerAsset replay,
            Course course,
            Labyrinths.Labyrinth labyrinth,
            Vector2Int startPosition)
        {
            PlayerSequence sequence = this.Create(
                labyrinth.GetLabyrinthPositionInWorldPosition(startPosition));

            sequence.replay = replay;
            sequence.labyrinth = labyrinth;
            sequence.course = course;
            sequence.material.color = course.Team.TeamColor;
            sequence.backtrackMaterial.color = course.Team.TeamColor;
            sequence.materialAlpha.color = course.Team.TeamColor.SetA(previousSegmentAlpha);
            sequence.backtrackMaterialAlpha.color = course.Team.TeamColor.SetA(previousSegmentAlpha);

            sequence.arrowHead.GetComponentInChildren<SpriteRenderer>().color = sequence.material.color;

            sequence.states.Add(new State
            {
                PrevLPos = startPosition,
                LPos = startPosition,
                NextLPos = startPosition

            });

            return sequence;
        }

        public override void AdjustOffset(float amount)// maxOffset)
        {
            this.offsetAmount = amount;

            //Segment sgm;
            foreach (Segment sgm in segments)
            {
                if (sgm == null)
                    continue;

                sgm.AdjustOffset(amount);
            }
        }

        public override void Awake()
        {
            base.Awake();

            // Never destroy segments (merely deactivate them
            states = new List<State>();

            material = new Material(templateMaterial);

            backtrackMaterial = new Material(templateBacktrackMaterial);

            materialAlpha = new Material(templateMaterial);

            backtrackMaterialAlpha = new Material(templateBacktrackMaterial);

            segments = new List<Segment>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (CurrentSegment != null)
            {
                arrowHead.gameObject.SetActive(true);

                arrowHead.transform.rotation = CurrentSegment.Rotation;

                arrowHead.transform.position = CurrentSegment.Position;
            }
            else
            {
                arrowHead.gameObject.SetActive(false);
            }
        }

        // Use in return to divergent location
        private void ReturnToDivergent(State state, Vector2Int fromlpos, Tile[] playerSteps, Tile[] wrong)
        {
            // Hide path
            Segment sgm;

            for (int i = 0; i < wrong.Length; i++)
            {
                if (state.Segments.TryGetValue(wrong[i].Position, out sgm))
                {
                    if (sgm != null)
                    {
                        sgm.gameObject.SetActive(false);
                    }

                    state.Segments.Remove(wrong[i].Position);
                }
            }

            // Hide current position
            if (state.Segments.TryGetValue(fromlpos, out sgm))
            {
                if (sgm != null)
                {
                    sgm.gameObject.SetActive(false);
                }

                state.Segments.Remove(fromlpos);
            }

            bool isBacktracking = playerSteps[0].Color == TileColor.Red;
            Vector2Int prevlpos = playerSteps[0].Position;
            Vector2Int lpos = prevlpos;
            Vector2Int nextlpos = prevlpos;

            // n-1 steps
            for (int i = 1; i < playerSteps.Length; i++)
            {
                prevlpos = lpos;
                lpos = nextlpos;
                nextlpos = playerSteps[i].Position;
                isBacktracking = playerSteps[i].Color == TileColor.Red;

                UpdateState(
                    CurrentState,
                    prevlpos,
                    lpos,
                    nextlpos,
                    isBacktracking);
            }
        }


        public void UpdateState(
            State state,
            Vector2Int prevlpos,
            Vector2Int lpos,
            Vector2Int nextlpos,
            bool isBacktrack,
            bool isError = false)
        {
            Direction direction = Utils.GetDirection(prevlpos, lpos);

            bool isInversed = false;

            Vector3 middle = labyrinth.GetLabyrinthPositionInWorldPosition(lpos);
            Vector3 origin = middle + Utils.GetTileEdgePositionFromDirection(Utils.GetDirection(lpos, prevlpos));
            Vector3 destination = middle + Utils.GetTileEdgePositionFromDirection(Utils.GetDirection(lpos, nextlpos));

            bool isTurn =
                Utils.GetDirection(prevlpos, lpos) !=
                Utils.GetDirection(lpos, nextlpos);

            var time = StepTime;

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

            Segment sgm;

            if (isError)
            {
                // Remove error
                if (state.Segments.TryGetValue(lpos, out sgm))
                {
                    if (sgm != null)
                    {
                        sgm.gameObject.SetActive(false);
                    }

                    state.Errors.Remove(lpos);
                }

                // Add error
                sgm = errorIndicatorTemplate.Create(
                    transform, 
                    origin, 
                    middle, 
                    destination, 
                    isInversed, 
                    isTurn,
                    labyrinth.transform.position.y + labyrinth.Camera.HeightOffset - cameraHeightDifference);

                state.Errors.Add(lpos, sgm);
                segments.Add(sgm);
            }

            // Remove segment
            if (state.Segments.TryGetValue(lpos, out sgm))
            {
                if (sgm != null)
                {
                    isInversed = Utils.IsOppositeDirection(direction, sgm.Direction);
                    sgm.gameObject.SetActive(false);
                }

                state.Segments.Remove(lpos);
            }

            // Add segment
            sgm = isTurn ?
                segmentTemplate.CreateTurn(
                    transform,
                    origin,
                    middle,
                    destination,
                    direction,
                    isInversed,
                    isBacktrack ? backtrackMaterial : material,
                    isBacktrack ? backtrackMaterialAlpha : materialAlpha,
                    StepTime,
                    isBacktrack ? backtrackWidth : normalWidth,
                    labyrinth.transform.position.y + labyrinth.Camera.HeightOffset - cameraHeightDifference) :
                segmentTemplate.Create(
                    transform,
                    origin,
                    destination,
                    direction,
                    isInversed,
                    isBacktrack ? backtrackMaterial : material,
                    isBacktrack ? backtrackMaterialAlpha : materialAlpha,
                    StepTime,
                    isBacktrack ? backtrackWidth : normalWidth,
                    labyrinth.transform.position.y + labyrinth.Camera.HeightOffset - cameraHeightDifference);

            state.Segments.Add(lpos, sgm);
            segments.Add(state.Head); // List<Segment> segments

            state.Head = sgm;
        }

        public void DrawState(State state)
        {
            foreach (var sgm in state.Segments.Values)
            {
                if (sgm == null)
                    return;

                sgm.gameObject.SetActive(true);
                sgm.AdjustOffset(offsetAmount);
                sgm.Draw();
            }

            foreach (var sgm in state.Errors.Values)
            {
                if (sgm == null)
                    return;

                sgm.gameObject.SetActive(true);
                sgm.AdjustOffset(offsetAmount);
                sgm.Draw();
            }
        }

        public void HideState(State state)
        {
            foreach (var sgm in state.Segments.Values)
            {
                if (sgm == null)
                    return;

                sgm.gameObject.SetActive(false);
            }

            foreach (var sgm in state.Errors.Values)
            {
                sgm.gameObject.SetActive(false);
            }
        }

        public void AddState()
        {
            states.Add(
                new State
                {
                    // Copy the previous state
                    Segments = new Dictionary<Vector2Int, Segment>(PreviousState.Segments),
                    Errors = new Dictionary<Vector2Int, Segment>(PreviousState.Errors),
                    Head = PreviousState.Head,
                    ActionValue = course.CurrentActionValue,
                    Action = course.CurrentAction
                }
            );

            bool isError = false;

            if (PreviousState.Action == GameAction.ReturnToDivergencePoint)
            {
                isError = true;

                ReturnToDivergent(
                    CurrentState,
                    PreviousState.LPos,
                    PreviousState.ActionValue.playerSteps,
                    PreviousState.ActionValue.wrongTiles);

                CurrentState.PrevLPos =
                    PreviousState.ActionValue.playerSteps.Length > 1 ?
                    PreviousState.ActionValue.playerSteps[PreviousState.ActionValue.playerSteps.Length - 2].Position :
                    PreviousState.ActionValue.playerSteps[PreviousState.ActionValue.playerSteps.Length - 1].Position;

                CurrentState.LPos =
                    PreviousState.ActionValue.playerSteps[PreviousState.ActionValue.playerSteps.Length - 1].Position;

                CurrentState.NextLPos = Utils.GetMoveDestination(
                    CurrentState.LPos,
                    course.CurrentAction);
            }
            else
            {
                CurrentState.PrevLPos = PreviousState.LPos;
                CurrentState.LPos = PreviousState.NextLPos;
                CurrentState.NextLPos = Utils.GetMoveDestination(PreviousState.NextLPos, course.CurrentAction);
            }

            bool isBacktrack = course.CurrentActionValue.previousColor == TileColor.Red;

            UpdateState(
                CurrentState,
                CurrentState.PrevLPos,
                CurrentState.LPos,
                CurrentState.NextLPos,
                isBacktrack,
                isError);
        }

        protected override void DoPrevious()
        {
            HideState(CurrentState);
            DrawState(PreviousState);

            stateIndex--;

            course.Previous();

            if (course.OnPlayerSequenceProgressedHandler != null)
                course.OnPlayerSequenceProgressedHandler.Invoke();
        }

        protected override void DoNext()
        {
            HideState(CurrentState);

            stateIndex++;

            // If the state was already visited
            // Simply redraw it
            // Else We need to create the state
            if (stateIndex >= states.Count)
            {
                AddState();
            }

            DrawState(CurrentState);

            course.Next();

            if (course.OnPlayerSequenceProgressedHandler != null)
                course.OnPlayerSequenceProgressedHandler.Invoke();
        }

        protected override IEnumerator DoNextCoroutine()
        {
            DoNext();

            if (CurrentState.Head != null)
            {
                yield return StartCoroutine(CurrentState.Head.DrawCoroutine(StepTime));
            }
        }

    }
}