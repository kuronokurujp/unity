using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestoryAfterEffect : MonoBehaviour
    {
        private void Update()
        {
            if (!this.GetComponent<ParticleSystem>().IsAlive())
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}

