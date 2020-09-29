using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private float experiencePoints = 0f;

        public event Action OnExperienceGaineed;

        public void GainExperience(float experience)
        {
            this.experiencePoints += experience;
            this.OnExperienceGaineed();
        }

        public object CaptureState()
        {
            return this.experiencePoints;
        }

        public void RestoreState(object state)
        {
            this.experiencePoints = (float)state;
        }

        public float GetPoints()
        {
            return this.experiencePoints;
        }
    }
}
