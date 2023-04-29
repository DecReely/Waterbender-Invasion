using System;
using UnityEngine;
using WaterbenderInvasion.Core;
using WaterbenderInvasion.Stats;

namespace WaterbenderInvasion.Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float regenerationPercentage = 70;
        
        private float _healthPoints = -1;
        private bool _isDead;

        private void Start()
        {
            if (_healthPoints < 0)
                _healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
        }
        
        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
        }
        
        // TODO: Hatalı olabilir?
        private void RegenerateHealth()
        {
            float regenerateHealthPoints = (GetComponent<BaseStats>().GetStat(Stat.Health)) * ( 1 + regenerationPercentage / 100);

            _healthPoints = Mathf.Max(_healthPoints, regenerateHealthPoints);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);

            if (_healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthPoints()
        {
            return _healthPoints;
        }
        
        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        
        public float GetPercentage()
        {
            return 100 * (_healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        private void Die()
        {
            if (_isDead) return;
            
            _isDead = true;
            
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public bool IsDead()
        {
            return _isDead;
        }
    }
}
