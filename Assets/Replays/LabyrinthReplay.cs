using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;
using System.Linq;

namespace UdeS.Promoscience.Replays
{
    public class LabyrinthReplay : BaseReplay
    {
        public Labyrinths.IData LabyrinthData => labyrinth.Data;

        public Labyrinths.IData labyrinthData;

        public Labyrinths.Labyrinth labyrinth;

        public Labyrinths.Labyrinth Labyrinth => labyrinth;

        protected Vector2Int lposition;

        protected Vector3 wposition;

        private Dictionary<int, PlayerSequence> playerSequences = new Dictionary<int, PlayerSequence>();

        private Dictionary<Algorithms.Id, AlgorithmSequence> algorithmSequences = new Dictionary<Algorithms.Id, AlgorithmSequence>();

        private List<PlayerSequence> activeSequences = new List<PlayerSequence>();

        private List<Course> courses;

        private System.Threading.Mutex mutex = new System.Threading.Mutex();

        List<Coroutine> started = new List<Coroutine>();

        private bool isDirtyToggled = false;

        private bool isPlaying = false;

        // TODO remove
        public LabyrinthReplay(
            List<Course> courses,
            Labyrinths.IData labyrinth) : base()
        {
            this.courses = courses;
            this.labyrinthData = labyrinth;

            //controller.OnActionHandler += OnReplayAction;
            //controller.OnPlaybackSpeedHandler += OnPlaybackSpeedChanged;
            CurrentCourse.OnValueChangedHandler += OnCourseSelected;
            IsToggleAlgorithm.OnValueChangedHandler += OnAlgorithmToggled;
            IsToggleGreyboxLabyrinth.OnValueChangedHandler += OnGreyBoxToggled;
            //controller.OnCourseAddedHandler += OnCourseAdded;
        }

        public LabyrinthReplay(
            List<Course> courses,
            Labyrinths.Labyrinth labyrinth) : base()
        {
            this.labyrinth = labyrinth;

            //controller.OnActionHandler += OnReplayAction;
            //controller.OnPlaybackSpeedHandler += OnPlaybackSpeedChanged;
            CurrentCourse.OnValueChangedHandler += OnCourseSelected;
            IsToggleAlgorithm.OnValueChangedHandler += OnAlgorithmToggled;
            IsToggleGreyboxLabyrinth.OnValueChangedHandler += OnGreyBoxToggled;
            //controller.OnCourseAddedHandler += OnCourseAdded;
        }

        public override void Start()
        {
            // TODO remove
            if (labyrinth == null)
            {
                labyrinth = Labyrinths.Resources.Instance
                    .GetLabyrinthTemplate(labyrinthData)
                    .Create(labyrinthData);

                labyrinthData = null;

                labyrinth.GenerateLabyrinthVisual();

                labyrinth.Init(enableCamera: true);

                labyrinth.Camera.OutputToTexture = true;
            }
            else
            {
                labyrinth.gameObject.SetActive(true);
            }

            UI.ReplayDisplay.Instance.ViewRawImage.texture = labyrinth.Camera.RenderTexture;

            lposition = labyrinth.GetLabyrithStartPosition();

            wposition = labyrinth.GetLabyrinthPositionInWorldPosition(lposition);

            EnableDirty(true);

            foreach (Course course in courses)
            {
                AddCourse(course, true);
            }

            PlaybackSpeed = 2f;
        }


        //public override void Clear()
        //{
        //    if (labyrinth != null)
        //    {
        //        GameObject.Destroy(labyrinth.gameObject);
        //        labyrinth = null;
        //    }
        //}

        public override void OnPlaybackSpeedChanged(float speed)
        {
            foreach (var sq in playerSequences.Values)
            {
                sq.PlaybackSpeed = speed;
            }

            foreach (var sq in algorithmSequences.Values)
            {
                sq.PlaybackSpeed = speed;
            }
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
                foreach (var sq in algorithmSequences.Values)
                {
                    if (sq.WithinBounds)
                    {
                        sq.StartNextCoroutine();
                        started.Add(sq.NextCoroutineResult);
                    }
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

            foreach (var sq in algorithmSequences.Values)
            {
                if (sq.WithinBounds) sq.Next();
            }

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

            foreach (var sq in algorithmSequences.Values)
            {
                if (sq.WithinBounds) sq.Previous();
            }

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

            foreach (var sq in algorithmSequences.Values)
            {
                sq.Move(target);
            }


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

            foreach (var sq in algorithmSequences.Values)
            {
                sq.Stop();
            }

            foreach (var sq in activeSequences)
            {
                sq.Stop();
            }

            Move(GlobalMoveIndex);

            isPlaying = false;
        }

        public override void Stop()
        {
            foreach (var sq in algorithmSequences.Values)
            {
                sq.Stop();// (target);
            }

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

            labyrinth.GenerateLabyrinthVisual(
                isGreyboxToggled ?
                Labyrinths.Resources.Instance.GreyboxSkin :
                null);

            EnableDirty(isDirtyToggled);
        }

        public void EnableDirty(bool enable)
        {
            isDirtyToggled = enable;

            foreach (var alg in algorithmSequences.Values)
            {
                alg.Show(!enable);
            }

        }

        public void EnableOptions(bool enable)
        {

        }

        public void OnGreyBoxToggled(bool toggled)
        {
            EnableGreybox(toggled);
        }

        public void OnAlgorithmToggled(bool toggled)
        {
            EnableDirty(toggled);
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


        public override void OnReplayAction(ReplayControlAction action, params object[] args)
        {
            Course course;

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

                case ReplayControlAction.Slide:

                    mutex.WaitOne();

                    int current = (int)args[0];
                    Move(current);

                    mutex.ReleaseMutex();

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
            foreach (AlgorithmSequence sq in algorithmSequences.Values)
            {
                sq.Show(false);
            }

            AlgorithmSequence sequence;

            if (algorithmSequences.TryGetValue(course.Algorithm.Id, out sequence))
            {
                sequence.Show(true);
            }
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
            if (OnSequenceFinishedHandler != null)
            {
                OnSequenceFinishedHandler.Invoke();
            }
        }

        public override void Clear()
        {
            first = null;

            if (labyrinth != null)
            {

                labyrinth.gameObject.Destroy();
                labyrinth = null;

            }

            foreach (Sequence sq in algorithmSequences.Values)
            {
                if (sq != null)
                {
                    Object.Destroy(sq.gameObject);
                }
            }

            foreach (Sequence sq in playerSequences.Values)
            {
                if (sq != null)
                {
                    Object.Destroy(sq.gameObject);
                }
            }

            playerSequences.Clear();

            activeSequences.Clear();

            algorithmSequences.Clear();

        }

        Course first = null;

        public override void AddCourse(Course course, bool added)
        {
            if (added)
            {

                if (!playerSequences.ContainsKey(course.Id))
                {
                    var sequence =
                        Resources.Instance.PlayerSequence.Create(
                            this,                            
                            course,
                            labyrinth,
                            lposition);

                    playerSequences.Add(course.Id, sequence);

                    activeSequences.Add(sequence);

                    TrySetMoveCount(sequence.LocalMoveCount);

                    if (!algorithmSequences.ContainsKey(course.Algorithm.Id))
                    {
                        var algorithmSeq =
                            Resources.Instance.AlgorithmSequence.Create(
                                this,
                                labyrinth,
                                course,
                                lposition
                                );

                        algorithmSequences.Add(course.Algorithm.Id, algorithmSeq);

                        TrySetMoveCount(algorithmSeq.LocalMoveCount);
                    }

                    AdjustOffsets();

                    OnCourseAddedHandler?.Invoke(course, true);

                    if (first == null)
                    {
                        first = course;
                        CurrentCourse.Value = course;
                    }
                }
            }
            else
            {
                OnCourseRemoved(course);
            }
        }

        public override void OnCourseRemoved(Course course)
        {
            activeSequences.Remove(playerSequences[course.Id]);
            playerSequences.Remove(course.Id);

            TrySetMoveCount(activeSequences.Max(x => x.LocalMoveCount));

            AdjustOffsets();

            OnCourseAddedHandler?.Invoke(course, false);
        }
    }
}