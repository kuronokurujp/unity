using RPG.Control;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Attack;
        }

        public bool HandleRaycast(PlayerController player)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var fighter = player.GetComponent<Fighter>();
                if (!fighter.CanAttack(this.gameObject)) return false;

                fighter.Attack(this.gameObject);
            }

            return true;
        }
    }
}