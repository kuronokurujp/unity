using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        public void Attack(CombatTarget target)
        {
            Debug.LogFormat("attack target => {0}", target.gameObject.name);
        }
    }
}