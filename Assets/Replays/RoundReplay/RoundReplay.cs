using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;
using System.Linq;
using Cirrus;

namespace UdeS.Promoscience.Replays
{
    public class RoundReplay : BaseReplay
    {
        public ObservableValue<Course> CurrentCourse = new ObservableValue<Course>();

        public Event<Course> OnCourseAddedHandler;

        public Event<Course> OnCourseRemovedHandler;

        public Event<Course, bool> OnCourseToggledHandler;

        public ObservableValue<bool> IsToggleAlgorithm = new ObservableValue<bool>();

        public ObservableValue<bool> IsToggleGreyboxLabyrinth = new ObservableValue<bool>();

        private Round round;

        public Labyrinths.ILabyrinth Labyrinth => round.Labyrinth;

        public Labyrinths.LabyrinthObject labyrinthObject;

        public Labyrinths.LabyrinthObject LabyrinthObject => labyrinthObject;

        protected Vector2Int lposition;

        protected Vector3 wposition;

        private Dictionary<int, PlayerSequence> playerSequences = new Dictionary<int, PlayerSequence>();

        private AlgorithmSequence algorithmSequence;

        private List<PlayerSequence> activeSequences = new List<PlayerSequence>();

        private List<Course> Courses => round.Courses;

        private System.Threading.Mutex mutex = new System.Threading.Mutex();

        List<Coroutine> started = new List<Coroutine>();

        private bool isDirtyToggled = false;

        private bool isPlaying = false;

        // TODO remove
        public RoundReplay(
            ReplayControlsAsset controls,
            Round round) : 
            base(controls)
        {
            this.round = round;

            CurrentCourse.OnValueChangedHandler += OnCourseSelected;
            IsToggleAlgorithm.OnValueChangedHandler += OnAlgorithmToggled;
            IsToggleGreyboxLabyrinth.OnValueChangedHandler += OnGreyBoxToggled;
        }

        public void CreateLabyrinth()
        {
            labyrinthObject = Labyrinths.Resources.Instance
                .GetLabyrinthObject(round.Labyrinth)
                .Create(round.Labyrinth);

            labyrinthObject.GenerateLabyrinthVisual();

            labyrinthObject.Init(enableCamera: true);

            labyrinthObject.Camera.OutputToTexture = true;

            UI.ReplayDisplay.Instance.ViewRawImage.texture = labyrinthObject.Camera.RenderTexture;

            lposition = labyrinthObject.GetLabyrithStartPosition();

            wposition = labyrinthObject.GetLabyrinthPositionInWorldPosition(lposition);
        }


        public override void Start()
        {
            CreateLabyrinth();

            algorithmSequence =
                Resources.Instance.AlgorithmSequence.Create(
                this,
                labyrinthObject,
                round.Algorithm,
                lposition
                );

            foreach (Course course in Courses)
            {
                AddCourse(course);
            }

            controls.PlaybackSpeed = 2f;
        }

        public override void OnPlaybackSpeedChanged(float speed)
        {
            foreach (var sq in playerSequences.Values)
            {
                sq.PlaybackSpeed = speed;
            }

            algorithmSequence.PlaybackSpeed = speed;
        }

        public override void Resume()
        {
            isPlaying = true;
            Server.Instance.StartCoroutine(ResumeCoroutine());
        }

        public IEnumerator ResumeCoroutine()
        {
            isPlaying = true;

            while (HasNext)
            {
                started.Clear();

                //next in algorithm
                if (algorithmSequence.WithinBounds)
                {
                    algorithmSequence.StartNextCoroutine();
                    started.Add(algorithmSequence.NextCoroutineResult);
                }

                // next in player sequence
                foreach (var sq in activeSequences)
                {
                    if (sq.WithinBounds)
                    {
                        sq.StartNextCoroutine();
                        started.Add(sq.NextCoroutineResult);
                    }
                }

                foreach (var sq in started)
                {
                    yield return sq;
                }

                GlobalMoveIndex++;
                yield return null;
            }

            isPlaying = false;
        }

        public override void Next()
        {
            if (isPlaying)
            {
                Pause();
            }

            if (algorithmSequence.WithinBounds) algorithmSequence.Next();            

            foreach (var sq in activeSequences)
            {
                if (sq.WithinBounds)
                {
                    sq.Next();
                    sq.UpdateArrowHead();
                }

            }

            GlobalMoveIndex++;
        }

        public override void Previous()
        {
            if (isPlaying)
            {
                Pause();
            }

            GlobalMoveIndex--;

            if (algorithmSequence.WithinBounds) algorithmSequence.Previous();

            foreach (var sq in activeSequences)
            {
                if (sq.WithinBounds)
                {
                    sq.Previous();
                    sq.UpdateArrowHead();
                }
            }
        }

        public override void Move(int target)
        {
            GlobalMoveIndex = target;

            algorithmSequence.Move(target);

            foreach (var sq in activeSequences)
            {
                sq.Move(target);
                sq.UpdateArrowHead();
            }
        }

        public override void Pause()
        {
            if (!isPlaying)
                return;

            algorithmSequence.Stop();

            foreach (var sq in activeSequences)
            {
                sq.Stop();
            }

            Move(GlobalMoveIndex);

            isPlaying = false;
        }

        public override void Stop()
        {
            algorithmSequence.Stop();

            foreach (var sq in activeSequences)
            {
                sq.Stop();
            }

            Move(0);

            isPlaying = false;
        }

        bool isGreyboxToggled = false;

        public void EnableGreybox(bool enable)
        {
            isGreyboxToggled = enable;

            labyrinthObject.GenerateLabyrinthVisual(
                isGreyboxToggled ?
                Labyrinths.Resources.Instance.GreyboxSkin :
                null);

            EnableAlgorithm(isDirtyToggled);
        }

        public void EnableAlgorithm(bool enable)
        {
            isDirtyToggled = enable;

            algorithmSequence.Show(!enable);
        }

        public void EnableOptions(bool enable) { }

        public void OnGreyBoxToggled(bool toggled)
        {
            EnableGreybox(toggled);
        }

        public void OnAlgorithmToggled(bool toggled)
        {
            EnableAlgorithm(toggled);
        }

        public void OnCourseToggled(Course course, bool enabled)
        {
            playerSequences[course.Id].gameObject.SetActive(enabled);

            if (!enabled)
            {
                activeSequences.Remove(playerSequences[course.Id]);
            }
            else
            {
                activeSequences.Add(playerSequences[course.Id]);
            }

            if (activeSequences.Count != 0)
            {
                // Adjust move count to biggest sequence
                TrySetMoveCount(activeSequences.Max(x => x.LocalMoveCount));
                Move(Mathf.Clamp(GlobalMoveIndex, 0, GlobalMoveCount));

                AdjustOffsets();
            }
        }


        public override void OnReplayControlAction(ReplayControlAction action)
        {
            switch (action)
            {
                case ReplayControlAction.Play:
                    Resume();
                    break;

                case ReplayControlAction.Resume:
                    Resume();
                    break;

                case ReplayControlAction.Pause:

                    //mutex.WaitOne();

                    //Pause();

                    //mutex.ReleaseMutex();

                    break;

                case ReplayControlAction.Next:

                    mutex.WaitOne();

                    Next();

                    mutex.ReleaseMutex();

                    break;

                case ReplayControlAction.Previous:

                    mutex.WaitOne();

                    Previous();

                    mutex.ReleaseMutex();

                    break;

                case ReplayControlAction.Stop:

                    //mutex.WaitOne();

                    //Stop();

                    //mutex.ReleaseMutex();

                    break;
            }
        }

        public void OnCourseSelected(Course course)
        {
            //foreach (AlgorithmSequence sq in algorithmSequences.Values)
            //{
            //    sq.Show(false);
            //}
            //algorithmSequence.Show(false);

            //algorithmSequence.Show(true);

        }

        public void TrySetMoveCount(int candidateMvcnt)
        {
            if (candidateMvcnt > GlobalMoveCount)
                GlobalMoveCount = candidateMvcnt;

            Debug.Log(GlobalMoveCount);
        }

        public virtual float GetOffsetAmount(float idx)
        {
            return
                // origin of segment at the center, move it to the left
                (-Labyrinths.Utils.TileSize / 2) +
                // number of offsets (minimum 1 to align back at the center if no other sgms)
                (idx + 1) *
                // width of the level divided by the amount of sequences we are trying to fit
                // Number of player sequences + 1 (active sequences contains algorithm)
                (Labyrinths.Utils.TileSize / (activeSequences.Count + 1));
        }

        public virtual void AdjustOffsets()
        {
            for (int i = 0; i < activeSequences.Count; i++)
            {
                activeSequences[i].AdjustOffset(GetOffsetAmount(i));
            }
        }

        protected override void OnSequenceFinished()
        {
            //if (OnSequenceFinishedHandler != null)
            //{
            //    OnSequenceFinishedHandler.Invoke();
            //}
        }

        public override void Clear()
        {
            first = null;

            if (labyrinthObject != null)
            {

                labyrinthObject.gameObject.Destroy();
                labyrinthObject = null;

            }



            playerSequences.Clear();

            activeSequences.Clear();
        }

        Course first = null;

        public void AddCourse(Course course)
        {
            if (!playerSequences.ContainsKey(course.Id))
            {
                var sequence =
                    Resources.Instance.PlayerSequence.Create(
                        this,                            
                        course,
                        labyrinthObject,
                        lposition);

                playerSequences.Add(course.Id, sequence);

                activeSequences.Add(sequence);

                TrySetMoveCount(sequence.LocalMoveCount);

                AdjustOffsets();

                OnCourseAddedHandler?.Invoke(course);

                if (first == null)
                {
                    first = course;
                    CurrentCourse.Value = course;
                }
            }
        }

        public void RemoveCourse(Course course)
        {
            activeSequences.Remove(playerSequences[course.Id]);
            playerSequences.Remove(course.Id);

            TrySetMoveCount(activeSequences.Max(x => x.LocalMoveCount));

            AdjustOffsets();

            OnCourseRemovedHandler?.Invoke(course);
        }
    }
}