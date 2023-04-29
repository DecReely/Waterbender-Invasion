using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace WaterbenderInvasion.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] private bool shouldUseModifiers;
        [Range(1,99)] [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression;
        [SerializeField] private GameObject levelUpParticleEffect;

        private Experience _experience;
        
        private int _currentLevel = 0;

        public event Action OnLevelUp;

        private void Awake()
        {
            _experience = GetComponent<Experience>();
        }

        private void OnEnable()
        {
            if (_experience != null)
                _experience.OnExperienceGained += UpdateLevel;
        }
        
        private void OnDisable()
        {
            if (_experience != null)
                _experience.OnExperienceGained -= UpdateLevel;
        }
        
        private void Start()
        {
            _currentLevel = CalculateLevel();

            
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > _currentLevel)
            {
                _currentLevel = newLevel;
                LevelUpEffect();
                OnLevelUp();
            }
        }
        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return GetBaseStat(stat) + GetAdditiveModifier(stat);
        }
        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel()) * (1 + GetPercentageModifier(stat) / 100);
        }
        
        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            
            float total = 0;
            
            foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in modifierProvider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }
        
        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            
            float total = 0;
            
            foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in modifierProvider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        public int GetLevel()
        {
            if (_currentLevel <= 0)
                _currentLevel = CalculateLevel();

            return _currentLevel;
        }

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel; 
            
            float currentXP = GetComponent<Experience>().GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                float xpToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                
                if (xpToLevelUp > currentXP) return level;
            }
            
            return penultimateLevel + 1;
        }
    }
}
