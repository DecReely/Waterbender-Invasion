using System;
using UnityEngine;
using WaterbenderInvasion.Attributes;
using WaterbenderInvasion.Combat;
using WaterbenderInvasion.Core;
using WaterbenderInvasion.Movement;

namespace WaterbenderInvasion.Control
{
    public class AIControl : MonoBehaviour
    {
        [SerializeField] private float chaseDistance;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float waypointDwellTime = 1f;
        [Range(0,1)] [SerializeField] private float patrolSpeedFraction = 0.2f;

        private Fighter _fighter;
        private Health _health;
        private Mover _mover;
        
        private GameObject _player;
        private Vector3 _guardPosition;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private int _currentWaypointIndex = 0;

        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            
            _player = GameObject.FindWithTag("Player");
        }
        
        private void Start()
        {
            _guardPosition = transform.position;
        }

        private void Update()
        {
            if (_health.IsDead()) return;
            
            if (InAttackRange(_player) && _fighter.CanAttack(_player))
            {
                AttackBehaviour();
            }
            else if (_timeSinceLastSawPlayer <= suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = _guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    _timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if (_timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                _mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            _currentWaypointIndex = patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(_currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(_player);
        }

        private bool InAttackRange(GameObject player)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer <= chaseDistance;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
