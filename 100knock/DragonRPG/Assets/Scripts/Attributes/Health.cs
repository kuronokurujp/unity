using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using System;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private float regnerationHealthPersent = 100f;
        [SerializeField]
        private TakeDamageEvent takeDamageEvent = new TakeDamageEvent();

        [Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        private SafeValue<float> helathPoints = null;
        private bool isDead = false;

        private BaseStats baseStatsComponent = null;

        private void Awake()
        {
            this.baseStatsComponent = this.GetComponent<BaseStats>();
            Debug.Assert(this.baseStatsComponent != null);

            this.helathPoints = new SafeValue<float>(() => this.baseStatsComponent.GetStats(Stats.Stats.Health));
        }

        private void Start()
        {
            this.helathPoints.ForceInit();
        }

        private void OnEnable()
        {
            this.baseStatsComponent.onLevelupEvent += this.OnLevelupEvent;
        }

        private void OnDisable()
        {
            this.baseStatsComponent.onLevelupEvent -= this.OnLevelupEvent;
        }

        private void OnLevelupEvent()
        {
            // すでにレベルは最新状態になっている
            var nowHelathPoints = this.baseStatsComponent.GetStats(Stats.Stats.Health);
            this.helathPoints.Value = nowHelathPoints * (this.regnerationHealthPersent / 100f);
            this.helathPoints.Value = Mathf.Max(this.helathPoints.Value, nowHelathPoints);
        }

        public object CaptureState()
        {
            return this.helathPoints.Value;
        }
        public bool IsDead()
        {
            return this.isDead;
        }

        public float GetHelath()
        {
            return this.helathPoints.Value;
        }

        public void Heal(float heal)
        {
            this.helathPoints.Value = Math.Min(this.helathPoints.Value + heal, this.GetMaxHelath());
        }

        public float GetMaxHelath()
        {
            return this.baseStatsComponent.GetStats(Stats.Stats.Health);
        }

        public float GetPersent()
        {
            return 100f * this.GetFraction();
        }

        public float GetFraction()
        {
            return this.helathPoints.Value / this.baseStatsComponent.GetStats(Stats.Stats.Health);
        }

        public void RestoreState(object state)
        {
            this.helathPoints.Value = (float)state;
            if (this.helathPoints.Value == 0)
            {
                this.Die();
            }
        }
        public void TakeDamge(GameObject instigator, float damage)
        {
            this.helathPoints.Value = Mathf.Max(this.helathPoints.Value - damage, 0.0f);
            if (this.helathPoints.Value == 0)
            {
                this.Die();
                this.AwardExperience(instigator);
            }
            else
            {
                this.takeDamageEvent.Invoke(damage);
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            var experiecne = instigator.GetComponent<Experience>();
            if (experiecne == null) return;

            experiecne.GainExperience(this.baseStatsComponent.GetStats(Stats.Stats.ExperienceReward));
        }

        private void Die()
        {
            if (this.isDead) return;

            GetComponent<Animator>().SetTrigger("die");
            // 死んだ時に実行中にアクションはクリアする
            // 死んだ後のアクション設定が正しい設定できるようにするため
            GetComponent<ActionScheduler>().CancelCurrentAction();

            this.isDead = true;
        }
    }
}