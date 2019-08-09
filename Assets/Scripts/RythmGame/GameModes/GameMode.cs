using UnityEngine;

namespace DancingICE.GameModes
{
    [CreateAssetMenu(menuName = "RythmGame/GameMode")]
    [System.Serializable]
    public class GameMode : ScriptableObject
    {
        public string modeName = null;
        public SceneField gameScene = null;
        public bool useTwitchIntegration = false;
        public bool analyzeAudioSpectrum = false;
        public bool mesureCalories = false;
    }
}