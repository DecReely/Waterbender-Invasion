using System;
using System.Collections.Generic;
using UnityEngine;
using WaterbenderInvasion.Attributes;
using WaterbenderInvasion.Movement;
using WaterbenderInvasion.Core;
using WaterbenderInvasion.Stats;

namespace WaterbenderInvasion.Combat
{
    public class Fighter : MonoBehaviour, IAction, IModifierProvider
    {
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private string defaultWeaponName = "Unarmed";

        private Health _target = null;
        private Weapon _currentWeapon;
        private float _timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            Weapon weapon = Resources.Load<Weapon>(defaultWeaponName);
            EquipWeapon(weapon);
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (_target == null) return;
            if (_target.IsDead()) return;
            
            if (!IsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }
        
        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            _currentWeapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            
            if (_timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();
                _timeSinceLastAttack = 0;
            }
        }

        // Animation Event
        void Hit()
        {
            if (_target == null) return;

            var damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            
            if (_currentWeapon.HasProjectile())
            {
                _currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, _target, gameObject, damage);
            }
            else
            {
                _target.TakeDamage(gameObject, damage);
            }
        }

        // Animation Event? (Yayı eklemedim ama projectile animation event adı olması gerek shoot yerine)
        void Shoot()
        {
            Hit();
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) < _currentWeapon.GetRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopTriggeringAttack();
            _target = null;
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }
        
        private void StopTriggeringAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
        
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.GetDamage();
            }
        }
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.GetPercentageBonus();
            }
        }

        public Health GetTarget()
        {
            return _target;
        }
        
    }
}
