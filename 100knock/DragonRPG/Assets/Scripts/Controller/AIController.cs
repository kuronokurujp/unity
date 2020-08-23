using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Controller
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        private float chaseDistance = 5.0f;
        [SerializeField]
        private float suspictionTime = 3.0f;
        [SerializeField]
        private PatrolPath patrolPath = null;
        [SerializeField]
        private float wayPointToLerance = 1.0f;
        [SerializeField]
        private float wayPointDewllTime = 1.0f;

        private GameObject player = null;
        private Fighter fighter = null;
        private Health health = null;
        private Mover mover = null;

        private Vector3 guardPosition = Vector3.zero;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWayPoint = Mathf.Infinity;
        private int currentWayPointIndex = 0;

        private void Start()
        {
            this.player = GameObject.FindWithTag("Player");
            this.fighter = this.GetComponent<Fighter>();
            this.health = this.GetComponent<Health>();
            this.mover = this.GetComponent<Mover>();

            this.guardPosition = this.transform.position;
        }

        private void Update()
        {
            if (this.health.IsDead()) return;

            if (this.IsAttackRangeOfPlayer() && this.fighter.CanAttack(this.player))
            {
                this.AttackBehaviour();
            }
            else if (this.timeSinceLastSawPlayer < this.suspictionTime)
            {
                this.SuspictionBehaviour();
            }
            else
            {
                this.PatrolBehaviour();
            }

            this.UpdateTimer();
        }

        private void UpdateTimer()
        {
            this.timeSinceLastSawPlayer += Time.deltaTime;
            this.timeSinceArrivedAtWayPoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            var nextPosition = this.guardPosition;
            if (this.patrolPath != null)
            {
                if (this.AtWayPoint())
                {
                    this.timeSinceArrivedAtWayPoint = 0f;
                    this.CycleWayPoint();
                }

                nextPosition = this.GetCurrentWayPoint();
            }

            if (this.timeSinceArrivedAtWayPoint >= this.wayPointDewllTime)
            {
                this.mover.StartMoveAction(nextPosition);
            }
        }

        private Vector3 GetCurrentWayPoint()
        {
            var pos = this.patrolPath.GetChildPosition(this.currentWayPointIndex);
            return pos;
        }

        private void CycleWayPoint()
        {
            this.currentWayPointIndex = this.patrolPath.GetNextIndex(this.currentWayPointIndex);
        }

        private bool AtWayPoint()
        {
            var distance = Vector3.Distance(this.transform.position, this.GetCurrentWayPoint());
            return distance < this.wayPointToLerance;
        }

        private void SuspictionBehaviour()
        {
            this.GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            this.timeSinceLastSawPlayer = 0.0f;
            this.fighter.Attack(this.player);
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