using UnityEngine;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField]
        private float timeBetweenAttacks = 1.0f;
        [SerializeField]
        private Transform rightHandTransform = null;
        [SerializeField]
        private Transform leftHandTransform = null;
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
            if (this.currentWeaponData == null)
            {
                this.EquipWeapon(this.defaultWeaponData); 
            }
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
            this.currentWeaponData.Spawn(this.rightHandTransform, this.leftHandTransform, this.GetComponent<Animator>());
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

        // このメソッドはAnimationEvent
        private void Hit()
        {
            if (this.target == null) return;

            this.target.TakeDamge(this.currentWeaponData.GetDamage());
        }
        // このメソッドはAnimationEvent
        private void Shoot()
        {
            if (this.target == null) return;
            if (this.currentWeaponData.HaveProjectile() == false) return;
            this.currentWeaponData.LaunchProjectile(this.rightHandTransform, this.leftHandTransform, this.target);
        }
        private bool GetIsRange()
        {
            Debug.Assert(this.target != null);
            return Vector3.Distance(this.transform.position, this.target.transform.position) < this.currentWeaponData.GetRange();
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

        public object CaptureState()
        {
            return this.currentWeaponData.name;
        }

        public void RestoreState(object state)
        {
            var weaponName = (string)state;
            var weaponData = Resources.Load<Weapon>(weaponName);
            this.EquipWeapon(weaponData); 
        }
    }
}