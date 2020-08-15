using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction 
    {
        [SerializeField]
        private float weaponRange = 2.0f;

        private Transform target = null;

        private void Update()
        {
            if (this.target == null)
            {
                return;
            }

            if (!this.GetIsRange())
            {
                this.GetComponent<Mover>().MoveTo(this.target.position);
            }
            else
            {
                this.GetComponent<Mover>().Cancel();
            }
        }

        private bool GetIsRange()
        {
            Debug.Assert(this.target != null);
            return Vector3.Distance(this.transform.position, this.target.position) < this.weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            Debug.Assert(combatTarget);

            this.GetComponent<ActionScheduler>().StartAction(this);
            this.target = combatTarget.transform;
        }

        public void Cancel()
        {
            this.target = null;
        }
    }
}