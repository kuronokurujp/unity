using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.Controller
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        private float chaseDistance = 5.0f;

        private GameObject player = null;
        private Fighter fighter = null;
        private Health health = null;

        private void Start()
        {
            this.player = GameObject.FindWithTag("Player");
            this.fighter = this.GetComponent<Fighter>();
            this.health = this.GetComponent<Health>();
        }

        private void Update()
        {
            if (this.health.IsDead()) return;

            if (this.IsAttackRangeOfPlayer() && this.fighter.CanAttack(this.player))
            {
                this.fighter.Attack(this.player);
            }
            else
            {
                this.fighter.Cancel();
            }
        }

        private bool IsAttackRangeOfPlayer()
        {
            if (this.player == null)
                return false;

            return (Vector3.Distance(this.player.transform.position, this.gameObject.transform.position) < this.chaseDistance);
        }

        /// <summary>
        /// Callback to draw gizmos only if the object is selected.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            var oldGizmosColor = Gizmos.color;
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(this.transform.position, this.chaseDistance);

            Gizmos.color = oldGizmosColor;
        }
    }
}