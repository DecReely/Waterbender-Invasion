using UnityEngine;

namespace WaterbenderInvasion.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageTextPrefab;

        private DamageText _instance;
        
        public void Spawn(float damageAmount)
        {
            _instance = Instantiate<DamageText>(damageTextPrefab, transform);
            _instance.SetValue(damageAmount);
        }
    }
}
