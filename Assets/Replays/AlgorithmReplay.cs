using UnityEngine;
using System.Collections;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;
using System.Threading;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays
{
    public class AlgorithmReplay : LabyrinthReplay
    {
        private bool isHidden = false;

        [SerializeField]
        private Algorithms.AlgorithmExecution execution;

        private BaseReplay parent;

        protected override BaseReplay Parent => parent;

        private Dictionary<Vector2Int, Stack<TileColor>> dictionary;

        public Cirrus.Event OnChangedHandler;

        public Algorithms.Id Algorithm {
            set
            {

                int previousMoveIndex = MoveIndex;

                DoReset();

                execution =
                    new Algorithms.AlgorithmExecution(
                        Algorithms.Resources.Instance.GetAlgorithm(value),
                        labyrinth);

                Move(previousMoveIndex);

                OnChangedHandler?.Invoke();
            }
        }


        public override int MoveCount => execution.MoveCount;

        public override int MoveIndex => execution.MoveIndex;

        protected override bool HasPrevious => execution.HasPrevious;

        protected override bool HasNext => execution.HasNext;

        public AlgorithmReplay Create(
            BaseReplay parent,
            Labyrinths.LabyrinthObject labyrinth,
            Algorithms.Algorithm algorithm,
            Vector2Int startPosition)
        {
            var replay = this.Create(
                labyrinth.GetLabyrinthPositionInWorldPosition(startPosition));

            replay.parent = parent;
            replay.labyrinth = labyrinth;
            replay.lposition = startPosition;
            replay.startlposition = startPosition;
            replay.execution = new Algorithms.AlgorithmExecution(algorithm, labyrinth);

            parent.OnResumeHandler += replay.OnResume;
            parent.OnMoveIndexChangedHandler += replay.OnMoveIndexChanged;

            return replay;
        }

        public override void Awake()
        {
            base.Awake();

            dictionary = new Dictionary<Vector2Int, Stack<TileColor>>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public void OnResume()
        {
            resumeCoroutineResult = StartCoroutine(ResumeCoroutine());
        }

        public void OnMoveIndexChanged(int index)
        {
            Move(index);
        }

        private void PaintTile(Tile tile)
        {
            labyrinth.SetTileColor(tile.Position, tile.Color);
        }

        private void PaintTile(Vector2Int position, TileColor action)
        {
            labyrinth.SetTileColor(position, action);
        }

        protected IEnumerator ResumeCoroutine()
        {
            if (HasNext) DoNext();
            yield return new WaitForSeconds(StepTime);          
            yield return null;
        }

        public void Show(bool show=true)
        {
            isHidden = show;

            if (show)
            {
                foreach (var tile in execution.Steps)
                {
                    if (dictionary.TryGetValue(tile.Position, out Stack<TileColor>  stack))
                    {
                        PaintTile(tile.Position, stack.Peek());
                    }
                }
            }
            else
            {
                foreach (var tile in execution.Steps)
                {
                    PaintTile(tile.Position, TileColor.Grey);
                }

            }
        }

        public void DoReset()
        {

            while (HasPrevious) DoPrevious();
            execution.Reset();
            dictionary.Clear();

        }


        protected override void DoNext()
        {
            execution.Next();

            lposition = execution.Steps[execution.MoveIndex].Position;

            if (!dictionary.TryGetValue(lposition, out Stack<TileColor> stack))
            {
                stack = new Stack<TileColor>();
                stack.Push(TileColor.Grey); // add base color
                dictionary.Add(lposition, stack);
            }

            if(!isHidden) PaintTile(execution.Steps[execution.MoveIndex]);
            stack.Push(execution.Steps[execution.MoveIndex].Color);
        }

        protected override void DoPrevious()
        {
            lposition = HasPrevious ? execution.Steps[execution.MoveIndex].Position : startlposition;

            if (dictionary.TryGetValue(lposition, out Stack<TileColor> stack))
            {
                if (stack.Count != 0)
                {
                    stack.Pop();
                    if (stack.Count != 0)
                    {
                        if(!isHidden) PaintTile(lposition, stack.Peek());
                    }
                }
            }

            execution.Previous();
        }
    }
}





