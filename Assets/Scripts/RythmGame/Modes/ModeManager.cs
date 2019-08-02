using UnityEngine;
using System.Collections;

namespace DancingICE.Modes
{
    public class ModeManager : MonoBehaviour
    {
        [SerializeField]
        private Mode[] modes = null;

        [SerializeField]
        private int current = 0;

        public Mode Current { get { return modes[current]; } }

        public Mode NextMode()
        {
            current = (current + 1) % modes.Length;
            return modes[current];
        }
    }
}
