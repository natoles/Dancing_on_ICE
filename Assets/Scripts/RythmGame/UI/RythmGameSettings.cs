using UnityEngine;
using DancingICE.GameModes;

namespace DancingICE.RythmGame
{
    public static class RythmGameSettings
    {
        #region Game Mode

        public static GameMode GameMode { get; set; } = null;

        #endregion

        #region Difficulty

        // Do not change default values here !!!

        private static float _minDifficulty = 0f;
        public static float MinDifficulty
        {
            get
            {
                return _minDifficulty;
            }
            private set
            {
                // MinDifficulty is in [0, MaxDifficuly[, included in [0, MAX_FLOAT[
                _minDifficulty = Mathf.Max(0, Mathf.Min(value, _maxDifficulty - Mathf.Epsilon, float.MaxValue - Mathf.Epsilon));
            }
        }

        private static float _maxDifficulty = float.MaxValue;
        public static float MaxDifficulty
        {
            get
            {
                return _maxDifficulty;
            }
            private set
            {
                // MaxDifficulty is in ]MinDifficulty, MAX_FLOAT], included in ]0, MAX_FLOAT]
                _maxDifficulty = Mathf.Min(Mathf.Max(Mathf.Epsilon, _minDifficulty + Mathf.Epsilon, value), float.MaxValue);
            }
        }

        private static float _difficultyStep = 1f;
        public static float DifficultyStep
        {
            get
            {
                return _difficultyStep;
            }
            private set
            {
                // DifficultyStep > 0
                _difficultyStep = Mathf.Max(Mathf.Epsilon, value);
            }
        }

        private static float _difficulty = 0f;
        public static float Difficulty
        {
            get
            {
                return _difficulty;
            }

            set
            {
                _difficulty = Mathf.Round(Mathf.Clamp(value, _minDifficulty, _maxDifficulty) / _difficultyStep) * _difficultyStep;
            }
        }
        public static float DifficultyPercentage
        {
            get
            {
                return (_difficulty - _minDifficulty) / (_maxDifficulty - _minDifficulty);
            }

            set
            {
                Difficulty = Mathf.Clamp01(value) * (_maxDifficulty - _minDifficulty) + _minDifficulty;
            }
        }

        #endregion

        #region Beatmap To Load

        public static BeatmapContainer BeatmapToLoad { get; set; } = null;

        #endregion

        // Use this for proper Difficulty initialization
        static RythmGameSettings()
        {
            MinDifficulty = 1f;
            MaxDifficulty = 5f;
            DifficultyStep = 0.1f;
            Difficulty = (_maxDifficulty + _minDifficulty) / 2f;
        }
    }
}