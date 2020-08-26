using RPG.Core;
using RPG.Combat;
using UnityEngine;
using RPG.Movement;

namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private Health health = null;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            this.health = this.GetComponent<Health>();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (this.health.IsDead()) return;
            if (this.InteractWithCombat()) return;
            if (this.InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            var hits = Physics.RaycastAll(this.GetMouseRay());
            foreach (var hit in hits)
            {
                var target = hit.transform.gameObject;
                if (!this.GetComponent<Fighter>().CanAttack(target)) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    this.GetComponent<RPG.Combat.Fighter>().Attack(target);
                }

                return true;
            }

            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(this.GetMouseRay(), out raycastHit))
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(raycastHit.point, 1f);
                }

                return true;
            }

            return false;
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
