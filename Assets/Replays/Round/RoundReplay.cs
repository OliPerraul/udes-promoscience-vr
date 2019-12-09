using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;
using System.Linq;
using Cirrus;

namespace UdeS.Promoscience.Replays
{
    public class RoundReplay : ControlReplay
    {
        private Round round;

        public Labyrinths.ILabyrinth Labyrinth => round.Labyrinth;

        public Labyrinths.LabyrinthObject labyrinthObject;

        public Labyrinths.LabyrinthObject LabyrinthObject => labyrinthObject;

        protected Vector2Int lposition;

        protected Vector3 wposition;

        private Dictionary<int, TeamReplay> playerSequences = new Dictionary<int, TeamReplay>();

        private AlgorithmReplay algorithmSequence;

        private List<TeamReplay> activeSequences = new List<TeamReplay>();

        private List<Course> Courses => round.Courses;

        protected override IEnumerable<IReplayWorker> Workers => activeSequences.Concat(new List<IReplayWorker>{algorithmSequence});

        private System.Threading.Mutex mutex = new System.Threading.Mutex();

        List<Coroutine> started = new List<Coroutine>();

        // TODO remove
        private bool isDirtyToggled = false;

        private bool isPlaying = false;

        private AlgorithmSelectionAsset algorithmSelection;

        private TeamToggleAsset teamToggle;

        private UI.SelectedTeamAsset selectedTeam;

        private UI.SidebarAsset sidebar;

        private Course first;

        public void OnCourseSelected(Course course)
        {
            if (playerSequences.TryGetValue(course.Id, out TeamReplay replay))
            {
                selectedTeam.OnReplayCourseSelectedHandler?.Invoke(replay);
            }
        }

        // TODO remove
        public RoundReplay(
            ReplayControlsAsset controls,
            AlgorithmSelectionAsset algorithmSelection,
            TeamToggleAsset teamToggle,
            UI.SelectedTeamAsset selectedTeam,
            UI.SidebarAsset sidebar,
            Round round) 
            : base(controls)
        {
            this.round = round;

            this.algorithmSelection = algorithmSelection;

            this.teamToggle = teamToggle;

            this.selectedTeam = selectedTeam;

            teamToggle.OnCourseSelectedHandler += OnCourseSelected;

            sidebar.IsToggleAlgorithm.OnValueChangedHandler += OnAlgorithmToggled;

            sidebar.IsToggleGreyboxLabyrinth.OnValueChangedHandler += OnGreyBoxToggled;
        }

        public override void Clear()
        {

            teamToggle.OnCourseSelectedHandler -= OnCourseSelected;

            teamToggle.OnCourseToggledHandler -= OnCourseToggled;

            sidebar.IsToggleAlgorithm.OnValueChangedHandler -= OnAlgorithmToggled;

            sidebar.IsToggleGreyboxLabyrinth.OnValueChangedHandler -= OnGreyBoxToggled;


            base.Clear();

            first = null;

            algorithmSequence.gameObject.Destroy();

            algorithmSequence = null;

            if (labyrinthObject != null)
            {
                labyrinthObject.gameObject.Destroy();
                labyrinthObject = null;
            }

            playerSequences.Clear();

            activeSequences.Clear();
        }


        public void SetMoveCount()
        {
            int algMvCnt = algorithmSequence.MoveCount;
            int teamMvCnt = activeSequences.Max(x => x.MoveCount);
            controls.ReplayMoveCount.Value = Mathf.Max(algMvCnt, teamMvCnt);
        }

        public override void Initialize()
        {
            base.Initialize();

            controls.PlaybackSpeed = 2f;

            labyrinthObject = Labyrinths.Resources.Instance
                .GetLabyrinthObject(round.Labyrinth)
                .Create(round.Labyrinth);

            labyrinthObject.GenerateLabyrinthVisual();

            labyrinthObject.Init(enableCamera: true);

            labyrinthObject.Camera.OutputToTexture = true;

            UI.ReplayDisplay.Instance.ViewRawImage.texture = labyrinthObject.Camera.RenderTexture;

            lposition = labyrinthObject.GetLabyrithStartPosition();

            wposition = labyrinthObject.GetLabyrinthPositionInWorldPosition(lposition);

            algorithmSequence =
                Resources.Instance.AlgorithmSequence.Create(
                this,
                labyrinthObject,
                round.Algorithm,
                lposition
                );

            foreach (Course course in round.Courses)
            {
                AddCourse(course);
            }

        }


        public override void Start()
        {
            Server.Instance.State.Set(ServerState.RoundReplay);
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
                SetMoveCount();
                //OnSlideValueChanged(Mathf.Clamp(GlobalMoveIndex, 0, GlobalMoveCount));

                AdjustOffsets();
            }
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

        //protected override void OnSequenceFinished()
        //{
        //    //if (OnSequenceFinishedHandler != null)
        //    //{
        //    //    OnSequenceFinishedHandler.Invoke();
        //    //}
        //}

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

                AdjustOffsets();

                teamToggle.OnReplayCourseAddedHandler?.Invoke(course);

                if (first == null)
                {
                    first = course;
                    teamToggle.OnCourseSelectedHandler?.Invoke(course);
                }

                SetMoveCount();
            }
        }

        public void RemoveCourse(Course course)
        {
            activeSequences.Remove(playerSequences[course.Id]);

            playerSequences.Remove(course.Id);

            SetMoveCount();

            AdjustOffsets();

            teamToggle.OnReplayCourseRemovedHandler?.Invoke(course);

            SetMoveCount();
        }



        //public override void OnPlaybackSpeedChanged(float speed)
        //{
        //    foreach (var sq in playerSequences.Values)
        //    {
        //        sq.PlaybackSpeed = speed;
        //    }

        //    if (algorithmSequence != null)
        //    {
        //        algorithmSequence.PlaybackSpeed = speed;
        //    }
        //}

        //public override void Resume()
        //{
        //    isPlaying = true;
        //    Server.Instance.StartCoroutine(ResumeCoroutine());
        //}

        //public IEnumerator ResumeCoroutine()
        //{
        //    isPlaying = true;

        //    while (HasNext)
        //    {
        //        started.Clear();

        //        //next in algorithm
        //        //if (algorithmSequence.WithinBounds)
        //        {
        //            algorithmSequence.StartNextCoroutine();
        //            started.Add(algorithmSequence.NextCoroutineResult);
        //        }

        //        // next in player sequence
        //        foreach (var sq in activeSequences)
        //        {
        //            //if (sq.WithinBounds)
        //            {
        //                sq.StartNextCoroutine();
        //                started.Add(sq.NextCoroutineResult);
        //            }
        //        }

        //        foreach (var sq in started)
        //        {
        //            yield return sq;
        //        }

        //        //GlobalMoveIndex++;
        //        yield return null;
        //    }

        //    isPlaying = false;
        //}

        //public override void Next()
        //{
        //    if (isPlaying)
        //    {
        //        Pause();
        //    }

        //    //if (algorithmSequence.WithinBounds) algorithmSequence.Next();            

        //    foreach (var sq in activeSequences)
        //    {
        //        //if (sq.WithinBounds)
        //        {
        //            sq.Next();
        //            sq.UpdateArrowHead();
        //        }

        //    }

        //    //GlobalMoveIndex++;
        //}

        //public override void Previous()
        //{
        //    if (isPlaying)
        //    {
        //        Pause();
        //    }

        //    //GlobalMoveIndex--;

        //    //if (algorithmSequence.WithinBounds) algorithmSequence.Previous();

        //    foreach (var sq in activeSequences)
        //    {
        //        //if (sq.WithinBounds)
        //        {
        //            sq.Previous();
        //            sq.UpdateArrowHead();
        //        }
        //    }
        //}

        //public override void OnSlideValueChanged(int target)
        //{
        //    //GlobalMoveIndex = target;

        //    algorithmSequence.Move(target);

        //    foreach (var sq in activeSequences)
        //    {
        //        sq.Move(target);
        //        sq.UpdateArrowHead();
        //    }
        //}

        //public override void Pause()
        //{
        //    if (!isPlaying)
        //        return;

        //    algorithmSequence.Stop();

        //    foreach (var sq in activeSequences)
        //    {
        //        sq.Stop();
        //    }

        //    //OnSlideValueChanged(GlobalMoveIndex);

        //    isPlaying = false;
        //}

        //public override void Stop()
        //{
        //    algorithmSequence.Stop();

        //    foreach (var sq in activeSequences)
        //    {
        //        sq.Stop();
        //    }

        //    OnSlideValueChanged(0);

        //    isPlaying = false;
        //}
    }
}