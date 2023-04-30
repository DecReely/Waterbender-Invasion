using UnityEngine;
using WaterbenderInvasion.Attributes;
using WaterbenderInvasion.Control;

namespace WaterbenderInvasion.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
        public bool HandleRaycast(PlayerController controller)
        {
            if (!controller.GetComponent<Fighter>().CanAttack(gameObject)) return false;
                            
            if (Input.GetMouseButtonDown(0))
            { 
                controller.GetComponent<Fighter>().Attack(gameObject);
            }
                            
            return true;
            
        }
    }
}
