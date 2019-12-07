using UnityEngine;
using System.Collections;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;
using System.Threading;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays
{
    public class AlgorithmSequence : Sequence
    {
        private Algorithms.AlgorithmExecution execution;

        private BaseReplay replay;

        protected override BaseReplay Replay => replay;

        private Dictionary<Vector2Int, Stack<TileColor>> dictionary;

        public override int LocalMoveCount => execution.LocalMoveCount;

        public override int LocalMoveIndex => execution.LocalMoveIndex;

        protected override bool HasPrevious => execution.HasPrevious;

        protected override bool HasNext => execution.HasNext;

        public AlgorithmSequence Create(
            BaseReplay replay,
            Labyrinths.LabyrinthObject labyrinth,
            Algorithms.Algorithm algorithm,
            Vector2Int startPosition)
        {
            var sequence = this.Create(
                labyrinth.GetLabyrinthPositionInWorldPosition(startPosition));

            sequence.replay = replay;
            sequence.labyrinth = labyrinth;
            sequence.lposition = startPosition;
            sequence.startlposition = startPosition;
            sequence.execution = new Algorithms.AlgorithmExecution(algorithm, labyrinth);

            return sequence;
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

        private void PaintTile(Tile tile)
        {
            labyrinth.SetTileColor(tile.Position, tile.Color);
        }

        private void PaintTile(Vector2Int position, TileColor action)
        {
            labyrinth.SetTileColor(position, action);
        }

        protected override IEnumerator DoNextCoroutine()
        {
            DoNext();
            yield return new WaitForSeconds(StepTime);          
            yield return null;
        }

        private bool isHidden = false;

        public void Show(bool show=true)
        {
            isHidden = show;

            if (show)
            {
                Stack<TileColor> stack;

                foreach (var tile in execution.Steps)
                {
                    if (dictionary.TryGetValue(tile.Position, out stack))
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

        protected override void DoNext()
        {
            lposition = execution.Steps[execution.LocalMoveIndex].Position;

            Stack <TileColor> stack;

            if (!dictionary.TryGetValue(lposition, out stack))
            {
                stack = new Stack<TileColor>();
                stack.Push(TileColor.Grey); // add base color
                dictionary.Add(lposition, stack);
            }

            if(!isHidden) PaintTile(execution.Steps[execution.LocalMoveIndex]);
            stack.Push(execution.Steps[execution.LocalMoveIndex].Color);

            execution.Next();
        }

        protected override void DoPrevious()
        {
            execution.Previous();

            Stack<TileColor> stack;

            lposition = HasPrevious ? execution.Steps[execution.LocalMoveIndex].Position : startlposition;

            if (dictionary.TryGetValue(lposition, out stack))
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
        }
    }
}





