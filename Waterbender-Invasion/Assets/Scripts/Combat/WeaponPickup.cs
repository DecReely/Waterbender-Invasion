using UnityEngine;
using WaterbenderInvasion.Control;

namespace WaterbenderInvasion.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Weapon weapon = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }
        private void Pickup(Fighter fighter)
        {

            fighter.GetComponent<Fighter>().EquipWeapon(weapon);
            Destroy(gameObject);
        }
        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
        public bool HandleRaycast(PlayerController controller)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(controller.GetComponent<Fighter>());
            }
            return true;
            
            
        }
    }
}
