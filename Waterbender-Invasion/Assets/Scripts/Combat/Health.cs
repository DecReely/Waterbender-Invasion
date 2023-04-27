using UnityEngine;
using UnityEngine.Serialization;

namespace WaterbenderInvasion.Combat
{
    public class Health : MonoBehaviour
    {
        [FormerlySerializedAs("health")] [SerializeField] private float healthPoints;
        private bool isDead;

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (!isDead)
            {
                GetComponent<Animator>().SetTrigger("die");
                
                isDead = true;
            }
        }

        public bool IsDead()
        {
            return isDead;
        }
    }
}
