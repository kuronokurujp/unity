using UnityEngine;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifiterProvider
    {
        [SerializeField]
        private float timeBetweenAttacks = 1.0f;
        [SerializeField]
        private Transform rightHandTransform = null;
        [SerializeField]
        private Transform leftHandTransform = null;
        [SerializeField]
        private WeaponConfig defaultWeaponData = null;
        private WeaponConfig currentWeaponConfig = null;
        private SafeValue<Weapon> currentWeapon = null;
        private Health target = null;
        private float timeSinceLastAttack = 0.0f;

        private void Awake()
        {
            this.currentWeapon = new SafeValue<Weapon>(this.SetDefaultWeapon);
        }

        private Weapon SetDefaultWeapon()
        {
            return this.EquipWeapon(this.defaultWeaponData); 
        }
         
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            this.currentWeapon.ForceInit();
        }

        public Health GetTarget() { return this.target; }

        public bool CanAttack(GameObject target)
        {
            if (target == null) return false;
            // 自身が攻撃対象でないかチェック
            if (this.gameObject == target) return false;

            if(
                !this.gameObject.GetComponent<Mover>().CanMoveTo(target.transform.position) &&
                !this.GetIsRange(target.transform))
            {
                return false;
            }

            var targetToTest = target.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public Weapon EquipWeapon(WeaponConfig weaponConfig)
        {
            this.currentWeaponConfig = weaponConfig;
            this.currentWeapon.Value = this.currentWeaponConfig.Spawn(this.rightHandTransform, this.leftHandTransform, this.GetComponent<Animator>());
            return this.currentWeapon.Value;
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

            if (!this.GetIsRange(this.target.transform))
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

            if (this.currentWeapon.Value != null)
            {
                this.currentWeapon.Value.OnHit();
            }

            float damage = this.GetComponent<BaseStats>().GetStats(Stats.Stats.Damage);
            this.target.TakeDamge(this.gameObject, damage);
        }

        // このメソッドはAnimationEvent
        private void Shoot()
        {
            if (this.target == null) return;
            if (this.currentWeaponConfig.HaveProjectile() == false) return;

            if (this.currentWeapon.Value != null)
            {
                this.currentWeapon.Value.OnHit();
            }

            float damage = this.GetComponent<BaseStats>().GetStats(Stats.Stats.Damage);
            this.currentWeaponConfig.LaunchProjectile(this.rightHandTransform, this.leftHandTransform, this.target, this.gameObject, damage);
        }

        private bool GetIsRange(Transform target)
        {
            Debug.Assert(this.target != null);
            return Vector3.Distance(this.transform.position, target.position) < this.currentWeaponConfig.GetRange();
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
            return this.currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            var weaponName = (string)state;
            var weaponData = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            this.EquipWeapon(weaponData); 
        }

        public IEnumerable<float> GetAdditiveModifiters(Stats.Stats stats)
        {
            if (stats == Stats.Stats.Damage)
            {
                yield return this.currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiters(Stats.Stats stats)
        {
            if (stats == Stats.Stats.Damage)
            {
                yield return this.currentWeaponConfig.GetPercentageBonus();
            }
        }
    }
}