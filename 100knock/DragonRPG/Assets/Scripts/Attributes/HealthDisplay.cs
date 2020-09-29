using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health health = null;
        private Text value;

        private void Awake()
        {
            this.health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            this.value = this.GetComponent<Text>();
        }

        private void Update()
        {
            this.value.text = string.Format("{0:0}/{1:0}", this.health.GetHelath(), this.health.GetMaxHelath());
        }
    }
}

