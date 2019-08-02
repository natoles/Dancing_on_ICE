using UnityEngine;
using System;

namespace DancingICE.Modes
{
    [CreateAssetMenu(menuName = "RythmGame/Mode")]
    public class Mode : ScriptableObject
    {
        public bool useCustomName = false;
        public string customName = null;
        public SceneField gameScene = null;
        public bool analyzeAudioSpectrum = false;
    }
}