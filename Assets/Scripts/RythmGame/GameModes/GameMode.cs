using UnityEngine;

namespace DancingICE.GameModes
{
    [CreateAssetMenu(menuName = "RythmGame/GameMode")]
    [System.Serializable]
    public class GameMode : ScriptableObject
    {
        public bool useCustomName = false;
        public string customName = null;
        public SceneField gameScene = null;
        public bool analyzeAudioSpectrum = false;
        public bool mesureCalories = false;
    }
}