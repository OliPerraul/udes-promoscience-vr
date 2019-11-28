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
        private Course course;

        private Dictionary<Vector2Int, Stack<TileColor>> dictionary;

        public override int LocalMoveCount => course.AlgorithmMoveCount;

        public override int LocalMoveIndex => course.CurrentAlgorithmMoveIndex;


        protected override bool HasPrevious => course.AlgorithmHasPrevious;

        protected override bool HasNext => course.AlgorithmHasNext;

        public AlgorithmSequence Create(
            ReplayControllerAsset replay,
            Labyrinths.Labyrinth labyrinth,
            Course course,
            Vector2Int startPosition)
        {
            var sequence = this.Create(labyrinth.GetLabyrinthPositionInWorldPosition(startPosition));

            sequence.replay = replay;
            sequence.labyrinth = labyrinth;
            sequence.lposition = startPosition;
            sequence.startlposition = startPosition;
            sequence.course = course;

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

                foreach (var tile in course.AlgorithmSteps)
                {
                    if (dictionary.TryGetValue(tile.Position, out stack))
                    {
                        PaintTile(tile.Position, stack.Peek());
                    }
                }
            }
            else
            {
                foreach (var tile in course.AlgorithmSteps)
                {
                    PaintTile(tile.Position, TileColor.Grey);
                }

            }
        }

        protected override void DoNext()
        {
            lposition = course.AlgorithmSteps[course.CurrentAlgorithmMoveIndex].Position;

            Stack <TileColor> stack;

            if (!dictionary.TryGetValue(lposition, out stack))
            {
                stack = new Stack<TileColor>();
                stack.Push(TileColor.Grey); // add base color
                dictionary.Add(lposition, stack);
            }

            if(!isHidden) PaintTile(course.AlgorithmSteps[course.CurrentAlgorithmMoveIndex]);
            stack.Push(course.AlgorithmSteps[course.CurrentAlgorithmMoveIndex].Color);

            course.AlgorithmNext();
        }

        protected override void DoPrevious()
        {
            course.AlgorithmPrevious();

            Stack<TileColor> stack;

            lposition = HasPrevious ? course.AlgorithmSteps[course.CurrentAlgorithmMoveIndex].Position : startlposition;

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





