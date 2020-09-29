using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        private BaseStats baseStats = null;
        private Text value;

        private void Awake()
        {
            this.baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
            this.value = this.GetComponent<Text>();
        }

        private void Update()
        {
            this.value.text = string.Format("{0:0}", this.baseStats.GetLevel());
        }
    }
}

