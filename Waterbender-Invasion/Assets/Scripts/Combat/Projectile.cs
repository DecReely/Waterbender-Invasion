using UnityEngine;
using WaterbenderInvasion.Attributes;

namespace WaterbenderInvasion.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private bool isHoming;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] private GameObject[] destroyOnHit;
        [SerializeField] private float lifeAfterImpact = 2;

        
        private Health _target;
        private GameObject _instigator;
        private float _damage;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (_target == null) return;

            if (isHoming && !_target.IsDead())
                transform.LookAt(GetAimLocation());
            
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            _target = target;
            _instigator = instigator;
            _damage = damage;
            
            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCollider = _target.GetComponent<CapsuleCollider>();

            if (targetCollider == null) 
                return _target.transform.position;
            else
                return _target.transform.position + Vector3.up * targetCollider.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != _target) return;    
            if (_target.IsDead()) return;
            
            _target.TakeDamage(_instigator, _damage);

            speed = 0;

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
