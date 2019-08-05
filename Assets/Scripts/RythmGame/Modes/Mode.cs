using UnityEngine;

namespace DancingICE.Modes
{
    [CreateAssetMenu(menuName = "RythmGame/Mode")]
    [System.Serializable]
    public class Mode : ScriptableObject
    {
        public bool useCustomName = false;
        public string customName = null;
        public SceneField gameScene = null;
        public bool analyzeAudioSpectrum = false;
        public bool mesureCalories = false;
    }
}