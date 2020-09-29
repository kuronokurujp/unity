using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Control
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
        [SerializeField]
        private float patrolMoveSpeedFraction = 0.2f;
        [SerializeField]
        private float aggrevatTimelimit = 5f;
        [SerializeField]
        private float aggrevatNearDistance = 2f;

        private GameObject player = null;
        private Fighter fighter = null;
        private Health health = null;
        private Mover mover = null;

        private SafeValue<Vector3> guardPosition = null; 
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWayPoint = Mathf.Infinity;
        private float timeSinceAggrevat = Mathf.Infinity;
        private int currentWayPointIndex = 0;

        private void Awake()
        {
            this.player = GameObject.FindWithTag("Player");
            this.fighter = this.GetComponent<Fighter>();
            this.health = this.GetComponent<Health>();
            this.mover = this.GetComponent<Mover>();

            this.guardPosition = new SafeValue<Vector3>(() => this.transform.position);
        }

        private void Start()
        {
            this.guardPosition.ForceInit();
        }

        private void Update()
        {
            if (this.health.IsDead()) return;

            if (this.IsAggrevated() && this.fighter.CanAttack(this.player))
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
            this.timeSinceAggrevat += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            var nextPosition = this.guardPosition.Value;
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
                this.mover.StartMoveAction(nextPosition, this.patrolMoveSpeedFraction);
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

            // 近くにいる敵に攻撃を実行
            this.AggrevatNearByEnemy();
        }

        private void AggrevatNearByEnemy()
        {
            var hits = Physics.SphereCastAll(this.transform.position, this.aggrevatNearDistance, Vector3.up, 0);
            foreach (var hit in hits)
            {
                var aiCtrl = hit.collider.gameObject.GetComponent<AIController>();
                if (aiCtrl == null) continue;

                aiCtrl.Aggrevat();
            }

        }

        private bool IsAttackRangeOfPlayer()
        {
            if (this.player == null)
                return false;

            return (Vector3.Distance(this.player.transform.position, this.gameObject.transform.position) < this.chaseDistance);
        }

        public void Aggrevat()
        {
            this.timeSinceAggrevat = 0f;
        }

        private bool IsAggrevated()
        {
            if (this.timeSinceAggrevat < this.aggrevatTimelimit) return true;
            return this.IsAttackRangeOfPlayer();
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