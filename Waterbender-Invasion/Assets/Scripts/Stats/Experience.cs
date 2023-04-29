using System;
using UnityEngine;

namespace WaterbenderInvasion.Stats
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] private float experiencePoints;

        public event Action OnExperienceGained;
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            OnExperienceGained();
        }
        public float GetPoints()
        {
            return experiencePoints;
        }
    }
}
