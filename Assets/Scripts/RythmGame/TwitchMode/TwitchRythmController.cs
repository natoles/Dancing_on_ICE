using UnityEngine;
using Kinect = Windows.Kinect;

namespace DancingICE.RythmGame.TwitchMode
{
    public class TwitchRythmController : RythmGameController
    {
        [SerializeField]
        private Camera mainCamera = null;

        #region Node Spawning

        private NodeCreation creator = null;
        private Bounds bounds = default;

        //private readonly float sliderPlotTime = 0.01f;
        private readonly float bx = 0.225f;
        private readonly float dx = 0.150f;
        private readonly float by = 0.275f;
        private readonly float dy = 0.375f;

        private readonly float minApproachTime = 2f;
        private readonly float maxApproachTime = 0.8f;
        private float ApproachTime { get { return Mathf.Lerp(minApproachTime, maxApproachTime, RythmGameSettings.DifficultyPercentage); } }

        private readonly float minSpeed = 1.75f;
        private readonly float maxSpeed = 3.5f;
        private float Speed { get { return Mathf.Lerp(minSpeed, maxSpeed, RythmGameSettings.DifficultyPercentage); } }

        private readonly float minSpawnDelay = 0.75f;
        private readonly float maxSpawnDelay = 0.15f;
        private float SpawnDelay { get { return Mathf.Lerp(minSpawnDelay, maxSpawnDelay, RythmGameSettings.DifficultyPercentage); } }

        private int currPeak = 0;
        private float previousNodeSpawning = float.MinValue;

        private Vector3 ComputePos(Kinect.JointType joint, float time)
        {
            return new Vector3(
                bounds.center.x + (joint == Kinect.JointType.HandLeft ? -1f : 1f) * (bx + dx * Mathf.Cos(time * Speed)) * bounds.extents.x,
                bounds.center.y + (1) * (by + dy * Mathf.Sin(time * Speed)) * bounds.extents.y
                );
        }

        #endregion

        protected override void Start()
        {
            base.Start();

            creator = new NodeCreation();
        }

        protected override void OnLoaded()
        {
            bounds = mainCamera.OrthographicBounds();
        }

        protected override void OnUpdate()
        {
            while (currPeak < Peaks.Count && Peaks[currPeak].time <= AudioPlayer.time + ApproachTime)
            {
                if (Time.time > previousNodeSpawning + SpawnDelay)
                {
                    previousNodeSpawning = Time.time;
                    GameObject node = creator.CreateBasicNode(NodeCreation.Joint.LeftHand, Peaks[currPeak].time - AudioPlayer.time, ComputePos(Kinect.JointType.HandLeft, previousNodeSpawning));
                    node.GetComponent<Node>().destroyOnTouch = true;
                }
                currPeak++;
            }
        }
    }
}