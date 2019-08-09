using UnityEngine;
using Kinect = Windows.Kinect;
using System.Collections.Generic;
using System.Linq;

namespace DancingICE.RythmGame.TwitchMode
{
    public class TwitchRythmController : RythmGameController
    {
        [SerializeField]
        private Camera mainCamera = null;

        #region Node Spawning

        private NodeCreation creator = null;
        private Bounds bounds = default;
        
        // Parameters of the circle (b = center, d = deviation / radius)
        // Those values are percentages, relative to the position of the GameObject
        // Example : bx = 0.5f means 50% of bounds.extents.x to the left (left hand) or right (right hand) from the center of the GameObject
        private readonly float bx = 0.225f;
        private readonly float dx = 0.150f;
        private readonly float by = 0.275f;
        private readonly float dy = 0.375f;

        // Time the node stays on screen
        private readonly float minApproachTime = 1.6f;
        private readonly float maxApproachTime = 0.5f;
        private float ApproachTime { get { return Mathf.Lerp(minApproachTime, maxApproachTime, RythmGameSettings.DifficultyPercentage); } }
        
        private readonly float minSpeed = 0.2f; // rotation per second
        private readonly float maxSpeed = 1.0f;
        private float Speed { get { return Mathf.Lerp(minSpeed, maxSpeed, RythmGameSettings.DifficultyPercentage); } }

        private readonly float minSpawnDelay = 0.675f;
        private readonly float maxSpawnDelay = 0.175f;
        private float SpawnDelay { get { return Mathf.Lerp(minSpawnDelay, maxSpawnDelay, RythmGameSettings.DifficultyPercentage); } }

        private int currPeak = 0;
        private int currJoint = 0;
        private float previousNodeSpawning = float.MinValue;

        private Vector3 ComputePos(Kinect.JointType joint, float time)
        {
            return new Vector3(
                bounds.center.x + (joint == Kinect.JointType.HandLeft ? -1f : 1f) * (bx + dx * Mathf.Cos(time * Speed * 2 * Mathf.PI)) * bounds.extents.x,
                bounds.center.y + (1) * (by + dy * Mathf.Sin(time * Speed * 2 * Mathf.PI)) * bounds.extents.y
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
                currJoint = 1 - currJoint;
                Kinect.JointType JointType = currJoint == 0 ? Kinect.JointType.HandLeft : Kinect.JointType.HandRight;
                NodeCreation.Joint Joint = currJoint == 0 ? NodeCreation.Joint.LeftHand : NodeCreation.Joint.RightHand;

                float current = currPeak;
                float nodeTime = Peaks[currPeak].time;
                previousNodeSpawning = nodeTime;
                currPeak++;

                // Search for the last of a group of "close" nodes starting with the current one
                while (currPeak < Peaks.Count && Peaks[currPeak].time < previousNodeSpawning + SpawnDelay)
                {
                    previousNodeSpawning = Peaks[currPeak].time;
                    currPeak++;
                }

                if (nodeTime + SpawnDelay >= previousNodeSpawning) // spawn basic node when we have an isolated node, or when line node would be too short
                {
                    GameObject node = creator.CreateBasicNode(Joint, nodeTime - AudioPlayer.time, ComputePos(JointType, nodeTime));
                    node.GetComponent<Node>().destroyOnTouch = true;
                }
                else
                {
                    List<float> lineTimes = new List<float>();
                    for (float f = nodeTime + 0.025f; f < previousNodeSpawning; f += 0.025f)
                        lineTimes.Add(f);
                    lineTimes.Add(previousNodeSpawning);
                    Vector3[] line = lineTimes.Select(time => ComputePos(JointType, time)).ToArray();
                    GameObject node = creator.CreateLineNode(Joint, nodeTime - AudioPlayer.time, lineTimes.Last() - nodeTime, ComputePos(JointType, nodeTime), line);
                }
            }
        }
    }
}