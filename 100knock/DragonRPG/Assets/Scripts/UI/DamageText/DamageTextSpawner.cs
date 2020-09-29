using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField]
        private DamageText damageTextPrefab = null;

        public void Spawn(float damageAmount)
        {
            var damageText = GameObject.Instantiate<DamageText>(this.damageTextPrefab, this.transform);
            damageText.SetValue(damageAmount);
        }
    }
}

