using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter fighter = null;
        private Text value;

        private void Awake()
        {
            this.fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
            this.value = this.GetComponent<Text>();
        }

        private void Update()
        {
            // 攻撃対象のHP比率を表示
            if (this.fighter.GetTarget() == null)
            {
                this.value.text = "N/T";
            }
            else
            {
                this.value.text = string.Format("{0:0}/{1:0}", this.fighter.GetTarget().GetHelath(), this.fighter.GetTarget().GetMaxHelath());
            }
        }
    }
}

