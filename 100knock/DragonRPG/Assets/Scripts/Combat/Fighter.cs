using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction 
    {
        [SerializeField]
        private float timeBetweenAttacks = 1.0f;
        [SerializeField]
        private Transform handTransform = null;
        [SerializeField]
        private Weapon defaultWeaponData = null;
        private Weapon currentWeaponData = null;
        private Health target = null;
        private float timeSinceLastAttack = 0.0f;
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            // 初期装備
            this.EquipWeapon(this.defaultWeaponData); 
        }
        public bool CanAttack(GameObject target)
        {
            if (target == null) return false;
            // 自身が攻撃対象でないかチェック
            if (this.gameObject == target) return false;

            var targetToTest = target.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        public void EquipWeapon(Weapon weaponData)
        {
            this.currentWeaponData = weaponData;
            this.currentWeaponData.Spawn(this.handTransform, this.GetComponent<Animator>());
        }
        private void Update()
        {
            this.timeSinceLastAttack += Time.deltaTime;
            if (this.target == null)
            {
                return;
            }

            if (this.target.IsDead())
            {
                return;
            }

            if (!this.GetIsRange())
            {
                this.GetComponent<Mover>().MoveTo(this.target.transform.position, 1f);
            }
            else
            {
                this.GetComponent<Mover>().Cancel();
                this.AttackBehaviour();
            }
        }
        private void AttackBehaviour()
        {
            this.transform.LookAt(this.target.transform);
            if (this.timeSinceLastAttack > this.timeBetweenAttacks)
            {
                this.TriggerAttack();
                this.timeSinceLastAttack = 0.0f;
            }
        }
        private void TriggerAttack()
        {
            var animator = this.GetComponent<Animator>();
            Debug.Assert(animator);

            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");
        }

        /// <summary>
        /// Animation Event.
        /// </summary>
        private void Hit()
        {
            if (this.target == null) return;

            this.target.TakeDamge(this.defaultWeaponData.GetDamage());
        }
        private bool GetIsRange()
        {
            Debug.Assert(this.target != null);
            return Vector3.Distance(this.transform.position, this.target.transform.position) < this.defaultWeaponData.GetRange();
        }
        public void Attack(GameObject combatTarget)
        {
            Debug.Assert(combatTarget);

            this.GetComponent<ActionScheduler>().StartAction(this);
            this.target = combatTarget.GetComponent<Health>();
        }
        public void Cancel()
        {
            this.target = null;
            this.StopAttack();
            this.GetComponent<Mover>().Cancel();
        }
        private void StopAttack()
        {
            var animator = this.GetComponent<Animator>();

            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }
    }
}