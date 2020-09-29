using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Experience experience = null;
        private Text value;

        private void Awake()
        {
            this.experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
            this.value = this.GetComponent<Text>();
        }

        private void Update()
        {
            this.value.text = string.Format("{0:0}", this.experience.GetPoints());
        }
    }
}

