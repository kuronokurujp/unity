using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Health health = null;

        [SerializeField]
        private RectTransform helathForground = null;

        [SerializeField]
        private Canvas rootCanvas = null;

        private void Update()
        {
            float helthFraction = this.health.GetFraction();
            if (Mathf.Approximately(helthFraction, 0f) || Mathf.Approximately(helthFraction, 1f))
            {
                this.rootCanvas.enabled = false;
                return;
            }

            this.rootCanvas.enabled = true;
            this.helathForground.localScale = new Vector3(this.health.GetFraction(), 1f, 1f);
        }
    }
}