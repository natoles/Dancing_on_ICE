using UnityEngine;
using System;

namespace DancingICE.Modes
{
    [Serializable]
    public class Mode
    {
        public string name = null;
        public bool showDifficultySlider = false;
        public GameObject buttonsToShow = null;
        public SceneField sceneToLoad = null;
        public bool analyzeAudioSpectrum = false;
    }
}