using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private float helathPoints = 100.0f;
        private bool isDead = false;
        public object CaptureState()
        {
            return this.helathPoints;
        }
        public bool IsDead()
        {
            return this.isDead;
        }
        public void RestoreState(object state)
        {
            this.helathPoints = (float)state;
            if (this.helathPoints == 0)
            {
                this.Die();
            }
        }
        public void TakeDamge(float damage)
        {
            this.helathPoints = Mathf.Max(this.helathPoints - damage, 0.0f);
            if (this.helathPoints == 0)
            {
                this.Die();
            }
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